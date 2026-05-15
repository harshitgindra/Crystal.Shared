using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class AnyUowRepositoryTests : BaseTests
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
        [Category("Any")]
        [Category("Uow")]
        public async Task RecordsExist()
        {
            //***
            //*** When AnyAsync is called without a filter
            //***
            var result = await Uow.Order.AnyAsync();
            //***
            //*** Then: Return true
            //***
            ClassicAssert.IsTrue(result);
        }

        [Test]
        [Category("Any")]
        [Category("Uow")]
        public async Task RecordsExistWithExpression()
        {
            //***
            //*** When AnyAsync is called with a matching filter
            //***
            var firstRecord = _testOrders.First();
            var result = await Uow.Order.AnyAsync(x => x.OrderId == firstRecord.OrderId);
            //***
            //*** Then: Return true
            //***
            ClassicAssert.IsTrue(result);
        }

        [Test]
        [Category("Any")]
        [Category("Uow")]
        public async Task RecordsDoNotExistWithExpression()
        {
            //***
            //*** When AnyAsync is called with a non-matching filter
            //***
            var result = await Uow.Order.AnyAsync(x => x.OrderId == 99);
            //***
            //*** Then: Return false
            //***
            ClassicAssert.IsFalse(result);
        }
    }
}
