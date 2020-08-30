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
    public class GetRepositoryTests : BaseTests
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
        }


        #region Get all tests
        [Test]
        [Category("Get")]
        public void GetAllData()
        {
            //***
            //*** When Get all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.GetAll();

            //***
            //*** Return all records
            //***
            Assert.AreEqual(_testOrders.Count, response.Count());
        }

        [Test]
        [Category("Get")]
        public async Task GetAllDataAsync()
        {
            //***
            //*** When Get all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.GetAllAsync();

            //***
            //*** Return all records
            //***
            Assert.AreEqual(_testOrders.Count, response.Count());
        }

        [Test]
        [Category("Get")]
        public void GetAllDataWithExpression()
        {
            //***
            //*** When Get all method is called with expression
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.GetAll(x => x.OrderId == 1);

            //***
            //*** Return selected records
            //***
            Assert.AreEqual(_testOrders.Count(x => x.OrderId == 1), response.Count());
        }

        [Test]
        [Category("Get")]
        public async Task GetAllDataWithExpressionAsync()
        {
            //***
            //*** When Get all method is called with expression
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.GetAllAsync(x => x.OrderId == 1);

            //***
            //*** Return all records
            //***
            Assert.AreEqual(_testOrders.Count(x => x.OrderId == 1), response.Count());
        }
        #endregion

        #region Get with expression tests
        [Test]
        [Category("Get")]
        public void GetDataWithId()
        {
            //***
            //*** When Get method is called with primary identifier
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.Get(frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.Name, response.Name);
        }

        [Test]
        [Category("Get")]
        public async Task GetDataWithIdAsync()
        {
            //***
            //*** When Get method is called with primary identifier
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.GetAsync(frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.Name, response.Name);
        }

        [Test]
        [Category("Get")]
        public void GetDataWithIdWhenDataDoNotExist()
        {
            //***
            //*** Given: Data record with id 99 do not exist
            //***
            //***
            //*** When Get method is called with primary identifier
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.Get(99);

            //***
            //*** Return null
            //***
            Assert.IsNull(response);
        }

        [Test]
        [Category("Get")]
        public async Task GetDataWithIdWhenDataDoNotExistAsync()
        {
            //***
            //*** Given: Data record with id 99 do not exist
            //***
            //***
            //*** When Get method is called with primary identifier
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.GetAsync(99);
            //***
            //*** Return null
            //***
            Assert.IsNull(response);
        }

        [Test]
        [Category("Get")]
        public void GetOneRecordWithExpression()
        {
            //***
            //*** When Get method is called with expression
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.Get(x => x.OrderId == frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.OrderId, response.OrderId);
        }

        [Test]
        [Category("Get")]
        public async Task GetOneRecordWithExpressionAsync()
        {
            //***
            //*** When Get method is called with expression
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.GetAsync(x => x.OrderId == frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.OrderId, response.OrderId);
        }
        #endregion

        #region Any tests
        [Test]
        [Category("Any")]
        public void AnyDataExist()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.Any(x => x.OrderId == firstRecord.OrderId);
            //***
            //*** Return true
            //***
            Assert.IsTrue(response);
        }

        [Test]
        [Category("Any")]
        public async Task AnyDataExistAsync()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.AnyAsync(x => x.OrderId == firstRecord.OrderId);
            //***
            //*** Return true
            //***
            Assert.IsTrue(response);
        }

        [Test]
        [Category("Any")]
        public void AnyDataNotExist()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = uowRepo.Any(x => x.OrderId == 99);
            //***
            //*** Return false
            //***
            Assert.IsFalse(response);
        }

        [Test]
        [Category("Any")]
        public async Task AnyDataNotExistAsync()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext);
            var response = await uowRepo.AnyAsync(x => x.OrderId == 99);
            //***
            //*** Return false
            //***
            Assert.IsFalse(response);
        } 
        #endregion
    }
}
