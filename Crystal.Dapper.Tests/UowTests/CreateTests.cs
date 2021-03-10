using NUnit.Framework;
using System.Threading.Tasks;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class CreateTests : BaseTests
    {
        [SetUp]
        public void Setup()
        {
            this.Init();
        }

        [Test]
        [Category("Insert")]
        [Category("Uow")]
        public async Task InsertRecord()
        {
            //***
            //*** Given: Insert a new record to the dB
            //***
            var newRecord = new Product()
            {
                Value = 70,
                Name = "Sample 1"
            };
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(newRecord);
            var product = await UowRepository.Repository<Product>().FindAsync(newRecord.ProductId);
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, product.Name);
        }
    }
}
