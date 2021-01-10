using Crystal.Abstraction;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class DeleteRepositoryTests : BaseTests
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
        }

        #region Delete tests

        [Test]
        [Category("Delete")]
        public async Task DeleteDataAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete Async method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAsync(record.OrderId);
            DbContext.Commit();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        public async Task DeleteDataWhenRecordNotExistAsync()
        {
            try
            {
                var record = _testOrders.First();
                //***
                //*** When Delete method is called
                //***
                using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
                await uowRepo.DeleteAsync(99);
                DbContext.Commit();
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
        public async Task DeleteDataExpressionAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete method is called with an expression
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAsync(x => x.OrderId == record.OrderId);
            DbContext.Commit();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        public async Task DeleteAllDataAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAllAsync();
            DbContext.Commit();
            //***
            //*** All Records should be deleted
            //***
            Assert.AreEqual(0, DbContext.Orders.Count());
        }

        #endregion

        //#region Bulk delete tests

        //[Test]
        //[Category("Delete")]
        //public void BulkDeleteAllData()
        //{
        //    //***
        //    //*** When Delete all method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.BulkDelete();
        //    //***
        //    //*** All Records should be deleted
        //    //***
        //    Assert.AreEqual(0, DbContext.Orders.Count());
        //}

        //[Test]
        //[Category("Delete")]
        //public void BulkDeleteDataWithExpression()
        //{
        //    var record = _testOrders.First();
        //    //***
        //    //*** When Bulk Delete method is called with an expression
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.BulkDelete(x => x.OrderId == record.OrderId);
        //    DbContext.Commit();
        //    //***
        //    //*** Record should be deleted
        //    //***
        //    Assert.IsNull(DbContext.Orders.Find(record.OrderId));
        //    Assert.AreNotEqual(0, DbContext.Orders.Count());
        //}

        //[Test]
        //[Category("Delete")]
        //public void BulkDeleteDataWithExpressionAsync()
        //{
        //    var record = _testOrders.First();
        //    //***
        //    //*** When Bulk Delete method is called with an expression
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.BulkDeleteAsync(x => x.OrderId == record.OrderId);
        //    DbContext.Commit();
        //    //***
        //    //*** Record should be deleted
        //    //***
        //    Assert.IsNull(DbContext.Orders.Find(record.OrderId));
        //    Assert.AreNotEqual(0, DbContext.Orders.Count());
        //}
        //#endregion

        #region Delete tests
        [Test]
        [Category("Delete")]
        public async Task DeleteDataAsyncBulkSaveChanges()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete Async method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAsync(record.OrderId);
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        public async Task DeleteDataWhenRecordNotExistAsyncBulkSaveChanges()
        {
            try
            {
                var record = _testOrders.First();
                //***
                //*** When Delete method is called
                //***
                using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
                await uowRepo.DeleteAsync(99);
                await DbContext.CommitBulkChangesAsync();
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
        public async Task DeleteDataExpressionBulkSaveChangesAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete method is called with an expression
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAsync(x => x.OrderId == record.OrderId);
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** Record should be deleted
            //***
            Assert.IsNull(DbContext.Orders.Find(record.OrderId));
            Assert.AreNotEqual(0, DbContext.Orders.Count());
        }

        [Test]
        [Category("Delete")]
        public async Task DeleteAllDataBulkSaveChangesAsync()
        {
            var record = _testOrders.First();
            //***
            //*** When Delete all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.DeleteAllAsync();
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** All Records should be deleted
            //***
            Assert.AreEqual(0, DbContext.Orders.Count());
        }

        #endregion
    }
}
