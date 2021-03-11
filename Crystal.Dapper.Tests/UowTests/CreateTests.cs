using NUnit.Framework;
using System.Collections.Generic;
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
        [Category("Dapper")]
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

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task InsertRecordWithTransactionAndCommit()
        {
            //***
            //*** Given: Insert a new record to the dB
            //*** And: Creating and transaction and committing it
            //***
            var newRecord = new Product()
            {
                Value = 70,
                Name = "Sample 1"
            };

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(newRecord);
            await UowRepository.CommitAsync();

            var product = await UowRepository.Repository<Product>().FindAsync(newRecord.ProductId);
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(newRecord.Name, product.Name);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task InsertRecordWithTransactionAndRollback()
        {
            //***
            //*** Given: Insert a new record to the dB
            //*** And: Creating and transaction and rollback it
            //***
            var newRecord = new Product()
            {
                Value = 70,
                Name = "Sample 1"
            };

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When rollback method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(newRecord);
            await UowRepository.RollbackAsync();

            var product = await UowRepository.Repository<Product>().FindAsync(newRecord.ProductId);
            //***
            //*** Then: 0 records should be returned
            //***
            Assert.IsNull(product);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task InsertRangeRecord()
        {
            //***
            //*** Given: Insert a multiple records to the dB
            //***
            var records = new List<Product>()
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
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(records);
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(records.Count, products.Count);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task InsertRangeRecordWithTransactionAndCommit()
        {
            //***
            //*** Given: Insert a new record to the dB
            //*** And: Creating and transaction and committing it
            //***
            var records = new List<Product>()
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

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(records);
            await UowRepository.CommitAsync();

            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(records.Count, products.Count);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task InsertRangeRecordWithTransactionAndRollback()
        {
            //***
            //*** Given: Insert a new record to the dB
            //*** And: Creating and transaction and rollback it
            //***
            var records = new List<Product>()
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

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When rollback method is called
            //***
            await UowRepository.Repository<Product>().InsertAsync(records);
            await UowRepository.RollbackAsync();

            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 0 records should be returned
            //***
            Assert.AreEqual(0, products.Count);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task BulkInsertRangeRecord()
        {
            //***
            //*** Given: Insert a bulk records to the dB
            //***
            var records = new List<Product>()
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
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().BulkInsertAsync(records);
            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(records.Count, products.Count);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task BulkInsertRangeRecordWithTransactionAndCommit()
        {
            //***
            //*** Given: Insert bulk records to the dB
            //*** And: Creating and transaction and committing it
            //***
            var records = new List<Product>()
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

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When insert method is called
            //***
            await UowRepository.Repository<Product>().BulkInsertAsync(records);
            await UowRepository.CommitAsync();

            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 1 record should be saved
            //***
            Assert.AreEqual(records.Count, products.Count);
        }

        [Test]
        [Category("Insert")]
        [Category("Dapper")]
        public async Task BulkInsertRangeRecordWithTransactionAndRollback()
        {
            //***
            //*** Given: Insert a bulk records to the dB
            //*** And: Creating and transaction and rollback it
            //***
            var records = new List<Product>()
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

            await UowRepository.BeginTransactionAsync();
            //***
            //*** When rollback method is called
            //***
            await UowRepository.Repository<Product>().BulkInsertAsync(records);
            await UowRepository.RollbackAsync();

            var products = await UowRepository.Repository<Product>().GetAsync();
            //***
            //*** Then: 0 records should be returned
            //***
            Assert.AreEqual(0, products.Count);
        }

        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }
    }
}
