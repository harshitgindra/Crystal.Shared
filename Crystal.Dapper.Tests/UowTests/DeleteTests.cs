using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework.Legacy;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class DeleteTests : BaseTests
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
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecord()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            //***
            //*** When delete method is called
            //***
            await UowRepository.Repository<Product>().DeleteAsync(_sampleProduct);
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be deleted
            //***
            ClassicAssert.IsNull(product);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecordTransactionCommit()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete method is called with commit
            //***
            await UowRepository.Repository<Product>().DeleteAsync(_sampleProduct);
            await UowRepository.CommitAsync();
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be deleted
            //***
            ClassicAssert.IsNull(product);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecordTransactionRollback()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete method is called with commit
            //***
            await UowRepository.Repository<Product>().DeleteAsync(_sampleProduct);
            await UowRepository.RollbackAsync();
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be not deleted
            //***
            ClassicAssert.IsNotNull(product);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecordsExpression()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            //***
            //*** When delete method is called with an expression
            //***
            await UowRepository.Repository<Product>().DeleteAsync(x => x.ProductId == _sampleProduct.ProductId);
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be deleted
            //***
            ClassicAssert.IsNull(product);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecordsExpressionTransactionCommit()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete method is called with an expression and transaction commit
            //***
            await UowRepository.Repository<Product>().DeleteAsync(x => x.ProductId == _sampleProduct.ProductId);
            await UowRepository.CommitAsync();
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be deleted
            //***
            ClassicAssert.IsNull(product);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteRecordsExpressionTransactionRollback()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProduct);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete method is called with an expression and transaction rollback
            //***
            await UowRepository.Repository<Product>().DeleteAsync(x => x.ProductId == _sampleProduct.ProductId);
            await UowRepository.RollbackAsync();
            var product = await UowRepository.Repository<Product>().FindAsync(_sampleProduct.ProductId);
            //***
            //*** Then: record should be not deleted
            //***
            ClassicAssert.IsNotNull(product);
        }


        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteAllRecords()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            //***
            //*** When delete all method is called
            //***
            await UowRepository.Repository<Product>().DeleteAllAsync();
            var product = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: all record should be deleted
            //***
            ClassicAssert.AreEqual(0, product.Count);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteAllRecordsTransactionCommit()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete all method is called with transaction commit
            //***
            await UowRepository.Repository<Product>().DeleteAllAsync();
            await UowRepository.CommitAsync();
            var product = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: all record should be deleted
            //***
            ClassicAssert.AreEqual(0, product.Count);
        }

        [Test]
        [Category("Delete")]
        [Category("Dapper")]
        public async Task DeleteAllRecordsTransactionRollback()
        {
            //***
            //*** Given: Delete a record in the database
            //***
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
            await UowRepository.BeginTransactionAsync();
            //***
            //*** When delete all method is called with transaction rollback
            //***
            await UowRepository.Repository<Product>().DeleteAllAsync();
            await UowRepository.RollbackAsync();
            var product = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: no record should be deleted
            //***
            ClassicAssert.AreEqual(_sampleProducts.Count, product.Count);
        }


        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
