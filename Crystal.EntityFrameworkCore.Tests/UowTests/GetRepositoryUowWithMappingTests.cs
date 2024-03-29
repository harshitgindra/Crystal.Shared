﻿using AutoMapper;
using Crystal.EntityFrameworkCore.Tests.Model;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests
{
    [TestFixture]
    public class GetRepositoryUowWithMappingTests : BaseTests
    {
        private List<Order> _testOrders;
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            _mapper = new MapperConfiguration(cfg =>
                       cfg.CreateMap<Order, OrderDto>()
                       .ForMember(dto => dto.OrderName, conf => conf.MapFrom(ol => ol.Name)))
                .CreateMapper();

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
            Uow = new UowRepository(DbContext, _mapper);
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
            var response = await Uow.Order.GetAsync<OrderDto>();

            //***
            //*** Return all records
            //***
            ClassicAssert.AreEqual(_testOrders.Count, response.Count());
        }

        [Test]
        [Category("Get")]
        [Category("Uow")]
        public async Task GetAllDataWithExpressionAsync()
        {
            //***
            //*** When Get all method is called with expression
            //***
            var response = await Uow.Order.GetAsync<OrderDto>(x => x.OrderId == 1);

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
            var response = await Uow.Order.FindAsync<OrderDto>(frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
           ClassicAssert.AreEqual(frstRec.Name, response.OrderName);
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
            var response = await Uow.Order.FindAsync<OrderDto>(99);
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
            var response = await Uow.Order.GetAsync<OrderDto>(x => x.OrderId == frstRec.OrderId);

            //***
            //*** Return 1 record
            //***
           ClassicAssert.AreEqual(frstRec.OrderId, response.First().OrderId);
        }
        #endregion

        #region Any tests
        [Test]
        [Category("Any")]
        [Category("Uow")]
        public async Task AnyDataExist()
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
