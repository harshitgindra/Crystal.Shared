using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class CreateUowTransactionRepositoryTests : BaseTests
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

        [Test]
        [Category("Insert")]
        [Category("Uow")]
        public async Task InsertRecordTransactionRollback()
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
            //*** When Rollback method is called
            //***
            await Uow.BeginTransactionAsync();
            await Uow.Order.InsertAsync(newRecord);
            await Uow.RollbackAsync();
            //***
            //*** Then: 0 record should be saved
            //***
            Assert.AreEqual(0, DbContext.Orders.Count());
        }

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
