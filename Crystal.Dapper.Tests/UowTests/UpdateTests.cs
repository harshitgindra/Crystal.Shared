using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class UpdateTests : BaseTests
    {
        [SetUp]
        public void Setup()
        {
            this.Init();
            _sampleProduct = new Product()
            {
                Value = 70,
                Name = "Sample 1"
            };
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
        }

        private Product _sampleProduct;
        private List<Product> _sampleProducts;

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task UpdateRecord()
        {
            //***
            //*** Given: Update a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            _sampleProduct.Name = "Sample 2";
            //***
            //*** When Update method is called
            //***
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProduct);
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: 1 record should be saved
            //***
           ClassicAssert.Equals(_sampleProduct.Name, product.Name);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task UpdateRecordWithTransactionAndCommit()
        {
            //***
            //*** Given: Update a record in the database
            //*** And: Updating and transaction and committing it
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);

            await UowRepository.BeginTransactionAsync();
            _sampleProduct.Name = "Sample 2";
            //***
            //*** When update method is called
            //***
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProduct);
            await UowRepository.CommitAsync();

            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: 1 record should be saved
            //***
           ClassicAssert.Equals(_sampleProduct.Name, product.Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task UpdateRecordWithTransactionAndRollback()
        {
            //***
            //*** Given: Update a record in the database
            //*** And: Updating and transaction and committing it
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);

            await UowRepository.BeginTransactionAsync();
            _sampleProduct.Name = "Sample 2";
            //***
            //*** When update method is called
            //***
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProduct);
            await UowRepository.RollbackAsync();

            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: 1 record should not be updated
            //***
           ClassicAssert.Equals("Sample 1", product.Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task UpdateRangeRecord()
        {
            //***
            //*** Given: Insert a multiple records to the dB
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProducts);
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
           ClassicAssert.Equals(_sampleProducts.First().Name, products.First().Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task UpdateRangeRecordWithTransactionCommit()
        {
            //***
            //*** Given: Update multiple records to the dB
            //*** And: transaction is created and committed
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.BeginTransactionAsync();
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProducts);
            await UowRepository.CommitAsync();
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: all records should be saved
            //***
           ClassicAssert.Equals(_sampleProducts.First().Name, products.First().Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task UpdateRangeRecordWithTransactionRollback()
        {
            //***
            //*** Given: Update multiple records to the dB
            //*** And: transaction is created and rollbac
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.BeginTransactionAsync();
            await UowRepository.Repository<Product>().UpdateAsync(_sampleProducts);
            await UowRepository.RollbackAsync();
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: No records should be updated
            //***
           ClassicAssert.Equals("Sample 1", products.First().Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task BulkUpdateRangeRecord()
        {
            //***
            //*** Given: Insert a multiple records to the dB
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.Repository<Product>().BulkUpdateAsync(_sampleProducts);
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
           ClassicAssert.Equals(_sampleProducts.First().Name, products.First().Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task BulkUpdateRangeRecordWithTransactionCommit()
        {
            //***
            //*** Given: Update multiple records to the dB
            //*** And: transaction is created and committed
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.BeginTransactionAsync();
            await UowRepository.Repository<Product>().BulkUpdateAsync(_sampleProducts);
            await UowRepository.CommitAsync();
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: all records should be saved
            //***
           ClassicAssert.Equals(_sampleProducts.First().Name, products.First().Name);
        }

        [Test]
        [Category("Update")]
        [Category("Dapper")]
        public async Task BulkUpdateRangeRecordWithTransactionRollback()
        {
            //***
            //*** Given: Update multiple records to the dB
            //*** And: transaction is created and rollback
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            foreach (var item in _sampleProducts)
            {
                item.Name = $"{item.Name} updated";
            }
            //***
            //*** When update multiple records method is called
            //***
            await UowRepository.BeginTransactionAsync();
            await UowRepository.Repository<Product>().BulkUpdateAsync(_sampleProducts);
            await UowRepository.RollbackAsync();
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: No records should be updated
            //***
           ClassicAssert.Equals("Sample 1", products.First().Name);
        }


        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
