using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class DeleteByIdTests : BaseTests
    {
        private List<Product> _sampleProducts = null!;

        [SetUp]
        public async Task Setup()
        {
            this.Init();
            _sampleProducts = new List<Product>
            {
                new Product { Value = 10, Name = "Delete Me 1" },
                new Product { Value = 20, Name = "Delete Me 2" },
            };
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteAsync_ById_RemovesRecord()
        {
            var id = _sampleProducts[0].ProductId;
            await UowRepository.Repository<Product>().DeleteAsync((object)id);

            var remaining = await UowRepository.Repository<Product>().GetAsync();
            ClassicAssert.AreEqual(1, remaining.Count);
            ClassicAssert.AreEqual("Delete Me 2", remaining[0].Name);
        }

        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
