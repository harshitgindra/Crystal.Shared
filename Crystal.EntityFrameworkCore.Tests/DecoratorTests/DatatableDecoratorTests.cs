using NUnit.Framework;
using NUnit.Framework.Legacy;
using Crystal.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DtOrder = Crystal.Shared.Order;
using DtColumn = Crystal.Shared.Column;

namespace Crystal.EntityFrameworkCore.Tests.DecoratorTests
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
        }

        private IQueryable<SampleItem> BuildQuery() =>
            new List<SampleItem>
            {
                new SampleItem { Id = 1, Name = "Alpha", Value = 10, IsActive = true,  IsOptional = true,  OptionalInt = 5,  LongValue = 100L, OptionalLong = 200L, ShortValue = 1, OptionalShort = 2, CreatedDate = new DateTime(2024,1,1), OptionalDate = new DateTime(2024,1,1) },
                new SampleItem { Id = 2, Name = "Beta",  Value = 20, IsActive = false, IsOptional = false, OptionalInt = 10, LongValue = 200L, OptionalLong = 300L, ShortValue = 2, OptionalShort = 3, CreatedDate = new DateTime(2024,2,1), OptionalDate = new DateTime(2024,2,1) },
                new SampleItem { Id = 3, Name = "Gamma", Value = 30, IsActive = true,  IsOptional = null,  OptionalInt = null, LongValue = 300L, OptionalLong = null, ShortValue = 3, OptionalShort = null, CreatedDate = new DateTime(2024,3,1), OptionalDate = null },
                new SampleItem { Id = 4, Name = "alpha", Value = 40, IsActive = false, IsOptional = false, OptionalInt = 5,  LongValue = 400L, OptionalLong = 200L, ShortValue = 4, OptionalShort = 2, CreatedDate = new DateTime(2024,4,1), OptionalDate = new DateTime(2024,4,1) },
            }.AsQueryable();

        [Test]
        public void ToDatatable_NullRequest_ReturnsAllRecords()
        {
            var result = BuildQuery().ToDatatable<SampleItem>(null!);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_WithLength_PaginatesResults()
        {
            var request = new DataTableRequest<SampleItem> { Start = 0, Length = 2, Columns = new List<DtColumn>(), Order = new List<DtOrder>() };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.Data.Length);
        }

        [Test]
        public void ToDatatable_LengthMinusOne_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem> { Start = 0, Length = -1, Columns = new List<DtColumn>(), Order = new List<DtOrder>() };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.Data.Length);
        }

        [Test]
        public async Task ToDatatableAsync_ReturnsExpectedResult()
        {
            var request = new DataTableRequest<SampleItem> { Start = 0, Length = -1, Columns = new List<DtColumn>(), Order = new List<DtOrder>() };
            var result = await BuildQuery().ToDatatableAsync(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_GlobalSearch_FiltersResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Search = new Search { Value = "alpha" },
                Columns = new List<DtColumn> { new DtColumn { Data = "Name", Searchable = true } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_ColumnFilter_String()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Name", Searchable = true, Search = new Search { Value = "Beta" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_ColumnFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Name", Searchable = true, Search = new Search { Value = "Gamma", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_BoolFilter_Valid()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "IsActive", Searchable = true, Search = new Search { Value = "true" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_BoolFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "IsActive", Searchable = true, Search = new Search { Value = "notabool" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableBoolFilter_Valid()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "IsOptional", Searchable = true, Search = new Search { Value = "true" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableBoolFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "IsOptional", Searchable = true, Search = new Search { Value = "notabool" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_IntFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Value", Searchable = true, Search = new Search { Value = "20", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_IntFilter_Contains()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Value", Searchable = true, Search = new Search { Value = "10" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_IntFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Value", Searchable = true, Search = new Search { Value = "notanint" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableIntFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalInt", Searchable = true, Search = new Search { Value = "10", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableIntFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalInt", Searchable = true, Search = new Search { Value = "notanint" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_LongFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "LongValue", Searchable = true, Search = new Search { Value = "300", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_LongFilter_Contains()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "LongValue", Searchable = true, Search = new Search { Value = "200" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_LongFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "LongValue", Searchable = true, Search = new Search { Value = "notalong" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableLongFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalLong", Searchable = true, Search = new Search { Value = "300", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableLongFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalLong", Searchable = true, Search = new Search { Value = "notalong" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_ShortFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "ShortValue", Searchable = true, Search = new Search { Value = "3", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_ShortFilter_Contains()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "ShortValue", Searchable = true, Search = new Search { Value = "2" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.GreaterOrEqual(result.TotalRecords, 1);
        }

        [Test]
        public void ToDatatable_ShortFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "ShortValue", Searchable = true, Search = new Search { Value = "notashort" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableShortFilter_ExactMatch()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalShort", Searchable = true, Search = new Search { Value = "3", ExactMatch = true } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableShortFilter_Invalid_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalShort", Searchable = true, Search = new Search { Value = "notashort" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_DateTimeFilter_ExactDate()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "2024-01-01" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_DateTimeFilter_DateRange()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "01/01/2024 - 03/01/2024" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(3, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_DateTimeFilter_InvalidDate_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "CreatedDate", Searchable = true, Search = new Search { Value = "notadate" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeFilter_ExactDate()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "2024-02-01" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(1, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeFilter_DateRange()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "01/01/2024 - 03/01/2024" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_NullableDateTimeFilter_InvalidDate_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "OptionalDate", Searchable = true, Search = new Search { Value = "notadate" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_UnknownColumn_ReturnsAll()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "DoesNotExist", Searchable = true, Search = new Search { Value = "anything" } } },
                Order = new List<DtOrder>()
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(4, result.TotalRecords);
        }

        [Test]
        public void ToDatatable_OrderDesc_SortsResults()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 0, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Name", Searchable = false } },
                Order = new List<DtOrder> { new DtOrder { Column = 0, Dir = "desc" } }
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual("Gamma", result.Data[0].Name);
        }

        [Test]
        public void ToDatatable_WithSkip_SkipsRecords()
        {
            var request = new DataTableRequest<SampleItem>
            {
                Start = 2, Length = -1,
                Columns = new List<DtColumn> { new DtColumn { Data = "Id", Searchable = false } },
                Order = new List<DtOrder> { new DtOrder { Column = 0, Dir = "asc" } }
            };
            var result = BuildQuery().ToDatatable(request);
            ClassicAssert.AreEqual(2, result.Data.Length);
        }
    }
}
