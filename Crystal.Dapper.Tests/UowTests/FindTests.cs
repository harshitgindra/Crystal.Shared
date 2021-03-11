using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories.SqlGenerator;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class FindTests : BaseTests
    {
        [SetUp]
        public async Task Setup()
        {
            this.Init();
            _sampleProducts = new List<Product>()
            {
                new Product()
                {
                    Value = 70,
                    Name = "Sample 1"
                },
                new Product()
                {
                    Value = 70,
                    Name = "Sample 2"
                },
            };
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
        }

        private List<Product> _sampleProducts;

        [Test]
        [Category("Find")]
        [Category("Dapper")]
        public async Task FindRecords()
        {
            //***
            //*** Given: Records exists in dB
            //***
            //***
            //*** When Find method is called
            //***
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProducts.First().ProductId);
            //***
            //*** Then: Return 1 record
            //***
            Assert.AreEqual(_sampleProducts.First().Name, product.Name);
        }

        [Test]
        [Category("Find")]
        [Category("Dapper")]
        public async Task FindRecordsDoNotExists()
        {
            //***
            //*** Given: Records do not exists in dB
            //***
            //***
            //*** When Find method is called
            //***
            var product = await UowRepository.Repository<Product>().FindAsync(0);
            //***
            //*** Then: Return 1 record
            //***
            Assert.IsNull(product);
        }

        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
