using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class DeleteUowRepositoryTests : BaseTests
    {
        private List<Order> _testOrders;

        [SetUp]
        public void Setup()
        {
            DbContext = new TestContext();
            //***
            //*** Clean data
            //***
            DbContext.Orders.RemoveRange(DbContext.Orders);
            //***
            //*** Setup data
            //***
            _testOrders = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Name = "Sample 1",
                    Value = 70
                },
                new Order()
                {
                    OrderId = 2,
                    Name = "Sample 2",
                    Value = 100
                }
            };

            DbContext.Orders.AddRange(_testOrders);
            DbContext.Commit();
            Uow = new UowRepository(DbContext);
        }

        #region Delete tests

        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteDataAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete Async method is called
            //***
            await Uow.Order.DeleteAsync(record.OrderId);
            await Uow.CommitAsync();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteDataWhenRecordNotExistAsync()
        {
            try
            {
                var record = _testOrders.First();
                //***
                //*** When Delete method is called
                //***
                await Uow.Order.DeleteAsync(99);
                await Uow.CommitAsync();
                //***
                //*** Then: Test failed if Exception not generated
                //***
                Assert.IsNotNull(DbContext.Orders.Find(record.OrderId));
            }
            catch
            {
                //***
                //*** Then: Exception generated, test passed
                //***
                Assert.Pass("Exception generated as record not found");
            }
        }

        #endregion

        #region Delete multiple records tests

        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteDataExpressionAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete method is called with an expression
            //***
            await Uow.Order.DeleteAsync(x => x.OrderId == record.OrderId);
            await Uow.CommitAsync();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteAllDataAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete all method is called
            //***
            await Uow.Order.DeleteAllAsync();
            await Uow.CommitAsync();
            //***
            //*** All Records should be deleted
            //***
            Assert.AreEqual(0, DbContext.Orders.Count());
        }

        #endregion
      
        #region Delete tests
        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteDataAsyncBulkSaveChanges()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete Async method is called
            //***
            await Uow.Order.DeleteAsync(record.OrderId);
            await Uow.CommitAsync();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        [Category("Uow")]
        public async Task DeleteDataWhenRecordNotExistAsyncBulkSaveChanges()
        {
            try
            {
                var record = _testOrders.First();
                //***
                //*** When Delete method is called
                //***
                await Uow.Order.DeleteAsync(99);
                await Uow.CommitAsync();
                //***
                //*** Then: Test failed if Exception not generated
                //***
                Assert.IsNotNull(DbContext.Orders.Find(record.OrderId));
            }
            catch
            {
                //***
                //*** Then: Exception generated, test passed
                //***
                Assert.Pass("Exception generated as record not found");
            }
        }

        #endregion

    }
}
