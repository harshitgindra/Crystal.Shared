using Crystal.Patterns.Abstraction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class InsertRepositoryTests : BaseTests
    {
        [SetUp]
        public void Setup()
        {
            DbContext = new TestContext();
            //***
            //*** Clean data
            //***
            DbContext.Orders.RemoveRange(DbContext.Orders.ToList());
            //***
            //*** Save changes
            //***
            DbContext.Commit();
        }

        #region Insert record tests
        [Test]
        [Category("Insert")]
        public async Task InsertRecord()
        {
            //***
            //*** Given: Insert a new record to the dB
            //***
            var newRecord = new Order()
            {
                OrderId = 1,
                Value = 70,
                Name = "Sample 1"
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(newRecord);
            DbContext.Commit();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        }

        [Test]
        [Category("Insert")]
        public async Task InsertRecordAsync()
        {
            //***
            //*** Given: Insert a new record to the dB
            //***
            var newRecord = new Order()
            {
                OrderId = 1,
                Value = 70,
                Name = "Sample 1"
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(newRecord);
            DbContext.Commit();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        }

        #endregion

        #region Insert multiple tests
        [Test]
        [Category("Insert")]
        public async Task InsertMultipleRecord()
        {
            //***
            //*** Given: Insert 2 records to the dB
            //***
            List<Order> records = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Value = 70,
                    Name = "Sample 1"
                },
                new Order()
                {
                    OrderId = 2,
                    Value = 70,
                    Name = "Sample 2"
                }
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(records);
            DbContext.Commit();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Insert")]
        public async Task InsertMultipleRecordAsync()
        {
            //***
            //*** Given: Insert 2 records to the dB
            //***
            List<Order> records = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Value = 70,
                    Name = "Sample 1"
                },
                new Order()
                {
                    OrderId = 2,
                    Value = 70,
                    Name = "Sample 2"
                }
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(records);
            DbContext.Commit();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion

        //#region Bulk insert tests
        //[Test]
        //[Category("Insert")]
        //public void BulkInsertMultipleRecord()
        //{
        //    //***
        //    //*** Given: Insert 2 records to the dB
        //    //***
        //    List<Order> records = new List<Order>()
        //    {
        //        new Order()
        //        {
        //            OrderId = 1,
        //            Value = 70,
        //            Name = "Sample 1"
        //        },
        //        new Order()
        //        {
        //            OrderId = 2,
        //            Value = 70,
        //            Name = "Sample 2"
        //        }
        //    };
        //    //***
        //    //*** When bulk insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.BulkInsert(records);
        //    //***
        //    //*** Then: 2 record should be saved
        //    //***
        //    Assert.AreEqual(records.Count, DbContext.Orders.Count());
        //}

        //[Test]
        //[Category("Insert")]
        //public async Task BulkInsertMultipleRecordAsync()
        //{
        //    //***
        //    //*** Given: Insert 2 records to the dB
        //    //***
        //    List<Order> records = new List<Order>()
        //    {
        //        new Order()
        //        {
        //            OrderId = 1,
        //            Value = 70,
        //            Name = "Sample 1"
        //        },
        //        new Order()
        //        {
        //            OrderId = 2,
        //            Value = 70,
        //            Name = "Sample 2"
        //        }
        //    };
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    await uowRepo.BulkInsertAsync(records);
        //    //***
        //    //*** Then: 2 record should be saved
        //    //***
        //    Assert.AreEqual(records.Count, DbContext.Orders.Count());
        //}

        //#endregion

        //#region Insert record tests commit bulk changes
        //[Test]
        //[Category("Insert")]
        //public void InsertRecordBulkSaveChanges()
        //{
        //    //***
        //    //*** Given: Insert a new record to the dB
        //    //***
        //    var newRecord = new Order()
        //    {
        //        OrderId = 1,
        //        Value = 70,
        //        Name = "Sample 1"
        //    };
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.Insert(newRecord);
        //    DbContext.CommitBulkChanges();
        //    //***
        //    //*** Then: 1 record should be saved
        //    //***
        //    Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        //}

        //[Test]
        //[Category("Insert")]
        //public async Task InsertRecordAsyncBulkSaveChanges()
        //{
        //    //***
        //    //*** Given: Insert a new record to the dB
        //    //***
        //    var newRecord = new Order()
        //    {
        //        OrderId = 1,
        //        Value = 70,
        //        Name = "Sample 1"
        //    };
        //    //***
        //    //*** When insert method is called
        //    //***
        //    using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
        //    uowRepo.Insert(newRecord);
        //    await DbContext.CommitBulkChangesAsync();
        //    //***
        //    //*** Then: 1 record should be saved
        //    //***
        //    Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        //}

        //#endregion

        #region Insert multiple tests commit bulk changes
        [Test]
        [Category("Insert")]
        public async Task InsertMultipleRecordBulkSaveChanges()
        {
            //***
            //*** Given: Insert 2 records to the dB
            //***
            List<Order> records = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Value = 70,
                    Name = "Sample 1"
                },
                new Order()
                {
                    OrderId = 2,
                    Value = 70,
                    Name = "Sample 2"
                }
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(records);
            DbContext.CommitBulkChanges();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Insert")]
        public async Task InsertMultipleRecordAsyncBulkSaveChanges()
        {
            //***
            //*** Given: Insert 2 records to the dB
            //***
            List<Order> records = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Value = 70,
                    Name = "Sample 1"
                },
                new Order()
                {
                    OrderId = 2,
                    Value = 70,
                    Name = "Sample 2"
                }
            };
            //***
            //*** When insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(records);
            await DbContext.CommitBulkChangesAsync();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion

        public List<Order> Records = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1,
                    Value = 70,
                    Name = "Sample 1"
                },
                new Order()
                {
                    OrderId = 2,
                    Value = 70,
                    Name = "Sample 2"
                }
            };
    }
}
