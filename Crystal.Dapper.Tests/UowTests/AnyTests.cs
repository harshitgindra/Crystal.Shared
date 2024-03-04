using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories.SqlGenerator;
using NUnit.Framework.Legacy;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class AnyTests : BaseTests
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
        [Category("Any")]
        [Category("Dapper")]
        public async Task RecordsExists()
        {
            //***
            //*** Given: Get record returns result without expression
            //***
            //***
            //*** When Check if any records exists
            //***
            var result = await UowRepository.Repository<Product>().AnyAsync();
            //***
            //*** Then: Return true
            //***
           ClassicAssert.IsTrue(result);
        }

        [Test]
        [Category("Any")]
        [Category("Dapper")]
        public async Task RecordsExistsWithExpression()
        {
            //***
            //*** Given: Get record returns result without expression
            //***
            //***
            //*** When Check if any records exists
            //***
            var result = await UowRepository.Repository<Product>().AnyAsync(x=>x.ProductId != 0);
            //***
            //*** Then: Return true
            //***
           ClassicAssert.IsTrue(result);
        }

        [Test]
        [Category("Any")]
        [Category("Dapper")]
        public async Task RecordsDoNotExistsWithExpression()
        {
            //***
            //*** Given: Get record returns result without expression
            //***
            //***
            //*** When Check if any records exists
            //***
            var result = await UowRepository.Repository<Product>().AnyAsync(x=>x.ProductId == 0);
            //***
            //*** Then: Return false
            //***
           ClassicAssert.IsFalse(result);
        }


        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
