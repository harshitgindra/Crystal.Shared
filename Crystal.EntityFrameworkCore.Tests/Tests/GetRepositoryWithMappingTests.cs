using AutoMapper;
using Crystal.EntityFrameworkCore.Tests.Model;
using Crystal.Abstraction;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class GetRepositoryWithMappingTests : BaseTests
    {
        private List<Order> _testOrders;
        private MapperConfiguration _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg =>
                       cfg.CreateMap<Order, OrderDto>()
                       .ForMember(dto => dto.OrderName, conf => conf.MapFrom(ol => ol.Name)));

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
        public async Task GetAllDataAsync()
        {
            //***
            //*** When Get all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.GetAsync<OrderDto>();

            //***
            //*** Return all records
            //***
            Assert.AreEqual(_testOrders.Count, response.Count());
        }

        [Test]
        [Category("Get")]
        public async Task GetAllDataWithExpressionAsync()
        {
            //***
            //*** When Get all method is called with expression
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.GetAsync<OrderDto>(x => x.OrderId == 1);

            //***
            //*** Return all records
            //***
            Assert.AreEqual(_testOrders.Count(x => x.OrderId == 1), response.Count());
        }
        #endregion

        #region Get with expression tests
        [Test]
        [Category("Get")]
        public async Task GetDataWithIdAsync()
        {
            //***
            //*** When Get method is called with primary identifier
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.FindAsync<OrderDto>(frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.Name, response.OrderName);
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
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.FindAsync<OrderDto>(99);
            //***
            //*** Return null
            //***
            Assert.IsNull(response);
        }

        [Test]
        [Category("Get")]
        public async Task GetOneRecordWithExpressionAsync()
        {
            //***
            //*** When Get method is called with expression
            //***
            var frstRec = _testOrders.First();
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.GetAsync<OrderDto>(x => x.OrderId == frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
            Assert.AreEqual(frstRec.OrderId, response.First().OrderId);
        }
        #endregion

        #region Any tests
        [Test]
        [Category("Any")]
        public async Task AnyDataExist()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            using IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.AnyAsync(x => x.OrderId == firstRecord.OrderId);
            //***
            //*** Return true
            //***
            Assert.IsTrue(response);
        }

        [Test]
        [Category("Any")]
        public async Task AnyDataNotExistAsync()
        {
            var firstRecord = _testOrders.First();
            //***
            //*** When Any all method is called
            //***
            IBaseRepository<Order> uowRepo = new BaseRepository<Order>(DbContext, _mapper);
            var response = await uowRepo.AnyAsync(x => x.OrderId == 99);
            //***
            //*** Return false
            //***
            Assert.IsFalse(response);
        }
        #endregion
    }
}
