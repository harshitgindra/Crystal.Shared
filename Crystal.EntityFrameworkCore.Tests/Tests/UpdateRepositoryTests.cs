using Crystal.Abstraction;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class UpdateRepositoryTests : BaseTests
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

        #region Update tests

        [Test]
        [Category("Update")]
        public async Task UpdateRecordAsync()
        {
            //***
            //*** Given: Update a record
            //***
            var order = _testOrders.First();
            order.Name = "Sample Updated";
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.UpdateAsync(order);
            DbContext.Commit();
            //***
            //*** Then: 1 record should be updated
            //***
            Assert.AreEqual(order.Name, DbContext.Orders.Find(order.OrderId).Name);
        }
        #endregion

        #region Update record tests
        //[Test]
        //[Category("Update")]
        //public void UpdateRecords()
        //{
        //    //***
        //    //*** Given: Update multiple record
        //    //***
        //    var updatedRecords = _testOrders.Select(x => new Order()
        //    {
        //        Name = x.Name + " Updated",
        //        OrderId = x.OrderId,
        //        Value = x.Value
        //    });
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.Update(updatedRecords);
        //    DbContext.Commit();
        //    //***
        //    //*** Then: multiple records should be updated
        //    //***
        //    Assert.Multiple(() =>
        //    {
        //        foreach (var item in updatedRecords)
        //        {
        //            Assert.AreEqual(item.Name, DbContext.Orders.Find(item.OrderId).Name);
        //        }
        //    });
        //}

        //[Test]
        //[Category("Update")]
        //public async Task UpdateRecordsAsync()
        //{
        //    //***
        //    //*** Given: Update multiple record
        //    //***
        //    var updatedRecords = _testOrders.Select(x => new Order()
        //    {
        //        Name = x.Name + " Updated",
        //        OrderId = x.OrderId,
        //        Value = x.Value
        //    });
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    await uowRepo.UpdateAsync(updatedRecords);
        //    DbContext.Commit();
        //    //***
        //    //*** Then: multiple records should be updated
        //    //***
        //    Assert.Multiple(() =>
        //    {
        //        foreach (var item in updatedRecords)
        //        {
        //            Assert.AreEqual(item.Name, DbContext.Orders.Find(item.OrderId).Name);
        //        }
        //    });
        //}
        #endregion

        //#region Bulk update tests
        //[Test]
        //[Category("Update")]
        //public void BulkUpdateRecords()
        //{
        //    //***
        //    //*** Given: Update multiple record
        //    //***
        //    var updatedRecords = _testOrders.Select(x => new Order()
        //    {
        //        Name = x.Name + " Updated",
        //        OrderId = x.OrderId,
        //        Value = x.Value
        //    });
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.BulkUpdate(updatedRecords);
        //    //***
        //    //*** Then: multiple records should be updated
        //    //***
        //    Assert.Multiple(() =>
        //    {
        //        foreach (var item in updatedRecords)
        //        {
        //            Assert.AreEqual(item.Name, DbContext.Orders.Find(item.OrderId).Name);
        //        }
        //    });
        //}

        //[Test]
        //[Category("Update")]
        //public async Task BulkUpdateRecordsAsync()
        //{
        //    //***
        //    //*** Given: Update multiple record
        //    //***
        //    var updatedRecords = _testOrders.Select(x => new Order()
        //    {
        //        Name = x.Name + " Updated",
        //        OrderId = x.OrderId,
        //        Value = x.Value
        //    });
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    await uowRepo.BulkUpdateAsync(updatedRecords);
        //    //***
        //    //*** Then: multiple records should be updated
        //    //***
        //    Assert.Multiple(() =>
        //    {
        //        foreach (var item in updatedRecords)
        //        {
        //            Assert.AreEqual(item.Name, DbContext.Orders.Find(item.OrderId).Name);
        //        }
        //    });
        //}
        //#endregion

        #region Update tests

        [Test]
        [Category("Update")]
        public async Task UpdateRecordCommitBulkChangesAsync()
        {
            //***
            //*** Given: Update a record
            //***
            var order = _testOrders.First();
            order.Name = "Sample Updated";
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, default);
            await uowRepo.UpdateAsync(order);
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** Then: 1 record should be updated
            //***
            Assert.AreEqual(order.Name, DbContext.Orders.Find(order.OrderId).Name);
        }
        #endregion

        #region Update record tests
        [Test]
        [Category("Update")]
        public async Task UpdateRecordsCommitBulkChangesAsync()
        {
            //***
            //*** Given: Update multiple record
            //***
            var updatedRecords = _testOrders.Select(x => new Order()
            {
                Name = x.Name + " Updated",
                OrderId = x.OrderId,
                Value = x.Value
            });
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.UpdateAsync(updatedRecords);
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** Then: multiple records should be updated
            //***
            Assert.Multiple(() =>
            {
                foreach (var item in updatedRecords)
                {
                    Assert.AreEqual(item.Name, DbContext.Orders.Find(item.OrderId).Name);
                }
            });
        }
        #endregion
    }
}
