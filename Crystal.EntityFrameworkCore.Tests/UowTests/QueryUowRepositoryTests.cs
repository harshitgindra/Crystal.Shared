using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class QueryUowRepositoryTests : BaseTests
    {
        private List<Order> _testOrders = null!;

        [SetUp]
        public void Setup()
        {
            DbContext = new TestContext();
            DbContext.Orders.RemoveRange(DbContext.Orders.ToList());
            DbContext.Commit();

            _testOrders = new List<Order>
            {
                new Order { OrderId = 10, Name = "Order A", Value = 1 },
                new Order { OrderId = 11, Name = "Order B", Value = 2 },
                new Order { OrderId = 12, Name = "Order C", Value = 3 }
            };
            DbContext.Orders.AddRange(_testOrders);
            DbContext.Commit();
            Uow = new UowRepository(DbContext);
        }

        [Test]
        [Category("Query")]
        public async Task GetFirstOrDefaultAsync_WithExpression_ReturnsRecord()
        {
            var result = await Uow.Order.GetFirstOrDefaultAsync(x => x.Name == "Order A");
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual("Order A", result!.Name);
        }

        [Test]
        [Category("Query")]
        public async Task GetFirstOrDefaultAsync_NoMatch_ReturnsNull()
        {
            var result = await Uow.Order.GetFirstOrDefaultAsync(x => x.Name == "DoesNotExist");
            ClassicAssert.IsNull(result);
        }

        [Test]
        [Category("Query")]
        public async Task QueryAsync_WithExpression_ReturnsMatchingRecords()
        {
            var results = await Uow.Order.QueryAsync(x => x.Name == "Order B");
            ClassicAssert.AreEqual(1, results!.Count());
        }

        [Test]
        [Category("Query")]
        public async Task FindAsync_ById_ReturnsRecord()
        {
            var result = await Uow.Order.FindAsync(_testOrders[0].OrderId);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(_testOrders[0].Name, result!.Name);
        }

        [Test]
        [Category("Query")]
        public async Task DeleteAsync_WithExpression_DeletesRecord()
        {
            await Uow.Order.DeleteAsync(x => x.Name == "Order A");
            await Uow.CommitAsync();
            var result = await Uow.Order.GetAsync(x => x.Name == "Order A");
            ClassicAssert.AreEqual(0, result.Count);
        }
    }
}
