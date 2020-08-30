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
    public class CreateRepositoryTests : BaseTests
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

        #region Create tests
        [Test]
        [Category("Create")]
        public void CreateRecord()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            uowRepo.Insert(newRecord);
            DbContext.SaveChanges();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord, DbContext.Orders.Find(newRecord.OrderId));
        }

        [Test]
        [Category("Create")]
        public async Task CreateRecordAsync()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(newRecord);
            DbContext.SaveChanges();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord, DbContext.Orders.Find(newRecord.OrderId));
        }

        #endregion

        #region Create tests
        [Test]
        [Category("Create")]
        public void CreateMultipleRecord()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            uowRepo.Insert(records);
            DbContext.SaveChanges();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Create")]
        public async Task CreateMultipleRecordAsync()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.InsertAsync(records);
            DbContext.SaveChanges();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Create")]
        public void BulkCreateMultipleRecord()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            uowRepo.BulkInsert(records);
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Create")]
        public async Task BulkCreateMultipleRecordAsync()
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
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            await uowRepo.BulkInsertAsync(records);
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion

    }
}
