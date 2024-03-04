using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class GetUowRepositoryTests : BaseTests
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
            DbContext.SaveChanges();
            Uow = new UowRepository(DbContext);
        }


        #region Get all tests

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetAllDataAsync()
        {
            //***
            //*** When Get all method is called
            //***
            var response = await Uow.Order.GetAsync();

            //***
            //*** Return all records
            //***
           ClassicAssert.Equals(_testOrders.Count, response.Count());
        }

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetAllDataWithExpressionAsync()
        {
            //***
            //*** When Get all method is called with expression
            //***
            var response = await Uow.Order.GetAsync(x => x.OrderId == 1);

            //***
            //*** Return all records
            //***
            ClassicAssert.AreEqual(_testOrders.Count(x => x.OrderId == 1), response.Count());
        }
        #endregion

        #region Get with expression tests

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetDataWithIdAsync()
        {
            //***
            //*** When Get method is called with primary identifier
            //***
            var frstRec = _testOrders.First();
            var response = await Uow.Order.FindAsync(frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
           ClassicAssert.Equals(frstRec.Name, response.Name);
        }

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetDataWithIdWhenDataDoNotExistAsync()
        {
            //***
            //*** Given: Data record with id 99 do not exist
            //***
            //***
            //*** When Get method is called with primary identifier
            //***
            var response = await Uow.Order.FindAsync(99);
            //***
            //*** Return null
            //***
            ClassicAssert.IsNull(response);
        }

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetOneRecordWithExpressionAsync()
        {
            //***
            //*** When Get method is called with expression
            //***
            var frstRec = _testOrders.First();
            var response = await Uow.Order.GetFirstOrDefaultAsync(x => x.OrderId == frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
           ClassicAssert.Equals(frstRec.OrderId, response.OrderId);
        }
        #endregion

        #region Any tests

        [Test]
        [Category("Any")]
        [Category("Uow")]
        public async Task AnyDataExistAsync()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            var response = await Uow.Order.AnyAsync(x => x.OrderId == firstRecord.OrderId);
            //***
            //*** Return true
            //***
            ClassicAssert.IsTrue(response);
        }

        [Test]
        [Category("Any")]
        [Category("Uow")]
        public async Task AnyDataNotExistAsync()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            var response = await Uow.Order.AnyAsync(x => x.OrderId == 99);
            //***
            //*** Return false
            //***
            ClassicAssert.IsFalse(response);
        }
        #endregion
    }
}
