using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class RollbackUowRepositoryTests : BaseTests
    {
        [SetUp]
        public void Setup()
        {
            DbContext = new TestContext();
            DbContext.Orders.RemoveRange(DbContext.Orders);
            DbContext.Commit();
            Uow = new UowRepository(DbContext);
        }

        [Test]
        [Category("Rollback")]
        public async Task RollbackAsync_AfterInsert_RevertsInsert()
        {
            await Uow.BeginTransactionAsync();
            await Uow.Order.InsertAsync(new Order { OrderId = 20, Name = "Order 1", Value = 1 });
            await Uow.RollbackAsync();

            var orders = await Uow.Order.GetAsync();
            ClassicAssert.AreEqual(0, orders.Count);
        }

        [Test]
        [Category("Rollback")]
        public async Task CommitAsync_WithTransaction_PersistsInsert()
        {
            await Uow.BeginTransactionAsync();
            await Uow.Order.InsertAsync(new Order { OrderId = 21, Name = "Order 1", Value = 1 });
            await Uow.CommitAsync();

            var orders = await Uow.Order.GetAsync();
            ClassicAssert.AreEqual(1, orders.Count);
        }
    }
}
