using NUnit.Framework;
using NUnit.Framework.Legacy;
using Crystal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.Dapper.Tests.DecoratorTests
{
    [TestFixture]
    public class DatatableDecoratorTests
    {
        private class SampleItem
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
            public bool IsActive { get; set; }
            public bool? IsOptional { get; set; }
            public int? OptionalInt { get; set; }
            public long LongValue { get; set; }
            public long? OptionalLong { get; set; }
            public short ShortValue { get; set; }
            public short? OptionalShort { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime? OptionalDate { get; set; }
            public NestedItem Nested { get; set; } = new NestedItem();
        }

        private class NestedItem
        {
            public string Tag { get; set; } = string.Empty;
        }

        private IQueryable<SampleItem> BuildQuery() =>
            new List<SampleItem>
            {
                new SampleItem { Id = 1, Name = "Alpha", Value = 10, IsActive = true,  IsOptional = true,  OptionalInt = 5,  LongValue = 100L, OptionalLong = 200L, ShortValue = 1, OptionalShort = 2, CreatedDate = new DateTime(2024,1,1), OptionalDate = new DateTime(2024,1,1), Nested = new NestedItem { Tag = "tagA" } },
                new SampleItem { Id = 2, Name = "Beta",  Value = 20, IsActive = false, IsOptional = false, OptionalInt = 10, LongValue = 200L, OptionalLong = 300L, ShortValue = 2, OptionalShort = 3, CreatedDate = new DateTime(2024,2,1), OptionalDate = new DateTime(2024,2,1), Nested = new NestedItem { Tag = "tagB" } },
                new SampleItem { Id = 3, Name = "Gamma", Value = 30, IsActive = true,  IsOptional = null,  OptionalInt = null, LongValue = 300L, OptionalLong = null, ShortValue = 3, OptionalShort = null, CreatedDate = new DateTime(2024,3,1), OptionalDate = null, Nested = new NestedItem { Tag = "tagC" } },
                new SampleItem { Id = 4, Name = "alpha", Value = 40, IsActive = false, IsOptional = false, OptionalInt = 5,  LongValue = 400L, OptionalLong = 200L, ShortValue = 4, OptionalShort = 2, CreatedDate = new DateTime(2024,4,1), OptionalDate = new DateTime(2024,4,1), Nested = new NestedItem { Tag = "tagA" } },
            }.AsQueryable();

        // ── Basic operations ──────────────────────────────────────────────────

        [Test]
        public void ToDatatable_NullRequest_ReturnsAllRecords()
        {
            var result = BuildQuery().ToDatatable<SampleItem>(null!);
            ClassicAssert.AreEqual(4, result.TotalRecords);
            ClassicAssert.AreEqual(4, result.Data.Length);
        }

        [Test]
        public void ToDatatable_WithLength_PaginatesResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = 2,
                Columns = new List<Column>(), Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
            ClassicAssert.AreEqual(2, result.Data.Length);
        }

        [Test]
        public void ToDatatable_LengthMinusOne_ReturnsAllRecords()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>(), Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.Data.Length);
        }

        [Test]
        public async Task ToDatatableAsync_ReturnsExpectedResult()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>(), Order = new List<Order>()
            };
            var result = await BuildQuery().ToDatatableAsync(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Global search ─────────────────────────────────────────────────────

        [Test]
        public void ToDatatable_WithGlobalSearch_FiltersResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Search = new Search { Value = "alpha" },
                Columns = new List<Column> { new Column { Data = "Name", Searchable = true } },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords); // "Alpha" and "alpha"
        }

        [Test]
        public void ToDatatable_GlobalSearch_NoSearchableColumns_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Search = new Search { Value = "alpha" },
                Columns = new List<Column> { new Column { Data = "Name", Searchable = false } },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Column filter (string) ────────────────────────────────────────────

        [Test]
        public void ToDatatable_WithColumnFilter_FiltersResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "Name", Searchable = true, Search = new Search { Value = "Beta" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
            ClassicAssert.AreEqual("Beta", result.Data[0].Name);
        }

        [Test]
        public void ToDatatable_WithExactMatchSearch_FiltersExactly()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "Name", Searchable = true, Search = new Search { Value = "Gamma", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        // ── Column filter (bool) ─────────────────────────────────────────────

        [Test]
        public void ToDatatable_BoolColumnFilter_FiltersActive()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "IsActive", Searchable = true, Search = new Search { Value = "true" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_BoolColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "IsActive", Searchable = true, Search = new Search { Value = "notabool" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableBoolColumnFilter_FiltersActive()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "IsOptional", Searchable = true, Search = new Search { Value = "true" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableBoolColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "IsOptional", Searchable = true, Search = new Search { Value = "notabool" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Column filter (int) ───────────────────────────────────────────────

        [Test]
        public void ToDatatable_IntColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "Value", Searchable = true, Search = new Search { Value = "10" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_IntColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "Value", Searchable = true, Search = new Search { Value = "20", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_IntColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "Value", Searchable = true, Search = new Search { Value = "notanint" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableIntColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalInt", Searchable = true, Search = new Search { Value = "5" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_NullableIntColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalInt", Searchable = true, Search = new Search { Value = "10", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableIntColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalInt", Searchable = true, Search = new Search { Value = "notanint" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Column filter (long) ──────────────────────────────────────────────

        [Test]
        public void ToDatatable_LongColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "LongValue", Searchable = true, Search = new Search { Value = "200" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_LongColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "LongValue", Searchable = true, Search = new Search { Value = "300", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_LongColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "LongValue", Searchable = true, Search = new Search { Value = "notalong" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableLongColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalLong", Searchable = true, Search = new Search { Value = "200" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_NullableLongColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalLong", Searchable = true, Search = new Search { Value = "300", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableLongColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalLong", Searchable = true, Search = new Search { Value = "notalong" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Column filter (short) ─────────────────────────────────────────────

        [Test]
        public void ToDatatable_ShortColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "ShortValue", Searchable = true, Search = new Search { Value = "2" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_ShortColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "ShortValue", Searchable = true, Search = new Search { Value = "3", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_ShortColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "ShortValue", Searchable = true, Search = new Search { Value = "notashort" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableShortColumnFilter_ContainsMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalShort", Searchable = true, Search = new Search { Value = "2" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_NullableShortColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalShort", Searchable = true, Search = new Search { Value = "3", ExactMatch = true } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableShortColumnFilter_InvalidValue_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalShort", Searchable = true, Search = new Search { Value = "notashort" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Column filter (DateTime) ──────────────────────────────────────────

        [Test]
        public void ToDatatable_DateTimeColumnFilter_ExactDate()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "2024-01-01" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_DateTimeColumnFilter_DateRange()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "01/01/2024 - 03/01/2024" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(3, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_DateTimeColumnFilter_InvalidDate_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "notadate" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeColumnFilter_ExactDate()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "2024-02-01" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeColumnFilter_DateRange()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "01/01/2024 - 03/01/2024" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeColumnFilter_InvalidDate_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "notadate" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Property not found ────────────────────────────────────────────────

        [Test]
        public void ToDatatable_UnknownColumnInFilter_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column>
                {
                    new Column { Data = "DoesNotExist", Searchable = true, Search = new Search { Value = "anything" } }
                },
                Order = new List<Order>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        // ── Ordering ──────────────────────────────────────────────────────────

        [Test]
        public void ToDatatable_WithOrder_SortsResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column> { new Column { Data = "Name", Searchable = false } },
                Order = new List<Order> { new Order { Column = 0, Dir = "desc" } }
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual("Gamma", result.Data[0].Name);
        }

        [Test]
        public void ToDatatable_WithOrder_UnknownColumn_StillReturnsData()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<Column> { new Column { Data = "DoesNotExist", Searchable = false } },
                Order = new List<Order> { new Order { Column = 0, Dir = "asc" } }
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.Data.Length);
        }

        // ── Skip ──────────────────────────────────────────────────────────────

        [Test]
        public void ToDatatable_WithSkip_SkipsRecords()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 2, Length = -1,
                Columns = new List<Column> { new Column { Data = "Id", Searchable = false } },
                Order = new List<Order> { new Order { Column = 0, Dir = "asc" } }
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
            ClassicAssert.AreEqual(2, result.Data.Length);
        }
    }
}
