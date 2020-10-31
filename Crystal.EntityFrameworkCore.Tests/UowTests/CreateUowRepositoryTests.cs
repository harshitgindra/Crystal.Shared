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
    public class CreateUowRepositoryTests : BaseTests
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
            Uow = new UowRepository(DbContext);
        }

        #region Insert record tests
        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(newRecord);
            await Uow.CommitAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        }

        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(newRecord);
            await Uow.CommitAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, DbContext.Orders.Find(newRecord.OrderId).Name);
        }

        #endregion

        #region Insert multiple tests
        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(records);
            await Uow.CommitAsync();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(records);
            await Uow.CommitAsync();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        #endregion

        #region Insert multiple tests commit bulk changes
        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(records);
            await Uow.CommitAsync();
            //***
            //*** Then: 2 record should be saved
            //***
            Assert.AreEqual(records.Count, DbContext.Orders.Count());
        }

        [Test]
        [Category("Insert")]
        [Category("Uow")]
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
            await Uow.Order.InsertAsync(records);
            await Uow.CommitAsync();
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
