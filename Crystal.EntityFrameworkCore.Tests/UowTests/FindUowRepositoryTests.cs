using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class FindUowRepositoryTests : BaseTests
    {
        private List<Order> _testOrders;

        [SetUp]
        public void Setup()
        {
            DbContext = new TestContext();
            DbContext.Orders.RemoveRange(DbContext.Orders.ToList());
            DbContext.SaveChanges();

            _testOrders = new List<Order>()
            {
                new Order() { OrderId = 1, Name = "Sample 1", Value = 70 },
                new Order() { OrderId = 2, Name = "Sample 2", Value = 100 }
            };

            DbContext.Orders.AddRange(_testOrders);
            DbContext.SaveChanges();
            Uow = new UowRepository(DbContext);
        }

        [Test]
        [Category("Find")]
        [Category("Uow")]
        public async Task FindRecord()
        {
            //***
            //*** Given: Records exist in the DB
            //***
            var firstRecord = _testOrders.First();
            //***
            //*** When FindAsync is called with a valid id
            //***
            var result = await Uow.Order.FindAsync(firstRecord.OrderId);
            //***
            //*** Then: Return the matching record
            //***
            ClassicAssert.AreEqual(firstRecord.Name, result.Name);
        }

        [Test]
        [Category("Find")]
        [Category("Uow")]
        public async Task FindRecordDoesNotExist()
        {
            //***
            //*** Given: Record with id 99 does not exist
            //***
            //***
            //*** When FindAsync is called with a non-existent id
            //***
            var result = await Uow.Order.FindAsync(99);
            //***
            //*** Then: Return null
            //***
            ClassicAssert.IsNull(result);
        }
    }
}
