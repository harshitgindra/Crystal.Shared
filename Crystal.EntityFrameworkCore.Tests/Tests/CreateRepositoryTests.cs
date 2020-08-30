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
            DbContext.SaveChanges();
        }

        #region Insert record tests
        [Test]
        [Category("Insert")]
        public void InsertRecord()
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
            uowRepo.Insert(newRecord);
            DbContext.SaveChanges();
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
            DbContext.SaveChanges();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        }

        #endregion

        #region Insert multiple tests
        [Test]
        [Category("Insert")]
        public void InsertMultipleRecord()
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
            uowRepo.Insert(records);
            DbContext.SaveChanges();
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
            DbContext.SaveChanges();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion

        #region Bulk insert tests
        [Test]
        [Category("Insert")]
        public void BulkInsertMultipleRecord()
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
            //*** When bulk insert method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            uowRepo.BulkInsert(records);
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Insert")]
        public async Task BulkInsertMultipleRecordAsync()
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
            await uowRepo.BulkInsertAsync(records);
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion
    }
}
