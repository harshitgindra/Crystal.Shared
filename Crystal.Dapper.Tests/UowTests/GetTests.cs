using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class GetTests : BaseTests
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
        [Category("Get")]
        [Category("Dapper")]
        public async Task GetRecords()
        {
            //***
            //*** Given: Get record returns result without expression
            //***
            //***
            //*** When Update method is called
            //***
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 2 record should be saved
            //***
           ClassicAssert.Equals(_sampleProducts.Count, products.Count);
        }

        [Test]
        [Category("Get")]
        [Category("Dapper")]
        public async Task GetRecordsWithExpression()
        {
            //***
            //*** Given: Get record returns result with expression
            //***
            //***
            //*** When Update method is called
            //***
            var products = await UowRepository.Repository<Product>().GetAsync(x => x.Name == "Sample 1");
            //***
            //*** Then: 1 record should be returned
            //***
           ClassicAssert.Equals(1, products.Count);
        }

        [Test]
        [Category("Get")]
        [Category("Dapper")]
        public async Task GetFirstOrDefaultWithExpression()
        {
            //***
            //*** Given: Get first or default returns result with expression
            //***
            //***
            //*** When Update method is called
            //***
            var products = await UowRepository.Repository<Product>().GetFirstOrDefaultAsync(x => x.Name == "Sample 1");
            //***
            //*** Then: 1 record should be returned
            //***
           ClassicAssert.Equals("Sample 1", products.Name);
        }

        [Test]
        [Category("Get")]
        [Category("Dapper")]
        public async Task NoRecordsFirstOrDefault()
        {
            //***
            //*** Given: No records in the dB
            //***
            //***
            //*** When Update method is called
            //***
            var products = await UowRepository.Repository<Product>().GetFirstOrDefaultAsync(x => x.Name == "Sample 15");
            //***
            //*** Then: 1 record should be returned
            //***
            ClassicAssert.IsNull(products);
        }


        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
