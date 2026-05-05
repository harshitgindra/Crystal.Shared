using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crystal.Dapper.Tests.UowTests;

namespace Crystal.Dapper.Tests.UowTests
{
    [TestFixture]
    public class WorkflowTests : BaseTests
    {
        // Sample products for multi-step transactions
        private List<Product> _sampleProducts;

        [SetUp]
        public async Task Setup()
        {
            this.Init();
            _sampleProducts = new List<Product>()
            {
                new Product { Value = 70, Name = "Sample 1" },
                new Product { Value = 80, Name = "Sample 2" }
            };

            // Pre-populate multiple records before testing workflows
            await UowRepository.Repository<Product>().InsertAsync(_sampleProducts);
        }

        [TearDown]
        public void Teardown()
        {
            UowRepository.Dispose();
        }

        [Test]
        [Category("Workflow")]
        [Description("Tests a successful multi-step operation that requires an atomic commit.")]
        public async Task MultiStepWrite_Success_CommitsChanges()
        {
            // Arrange: Simulate initial state check (2 records exist)
            var countBefore = await UowRepository.Repository<Product>().GetAsync();
            ClassicAssert.AreEqual(2, countBefore.Count);

            // Act: Wrap a sequence of writes in the atomic execution block
            await UowRepository.ExecuteUnitOfWork(async () =>
            {
                // 1. Update existing record (Sample 1)
                var productToUpdate = _sampleProducts.First();
                productToUpdate.Name = "Updated Sample 1";
                await UowRepository.Repository<Product>().UpdateAsync(productToUpdate);

                // 2. Bulk Insert a new set of records (Requires manual setup for testing)
                var newRecords = new List<Product>()
                {
                    new Product { Value = 99, Name = "New Record A" },
                    new Product { Value = 100, Name = "New Record B" }
                };
                await UowRepository.Repository<Product>().BulkInsertAsync(newRecords);
            });

            // Assert: All changes should be committed and visible
            var countAfter = await UowRepository.Repository<Product>().GetAsync();
            ClassicAssert.AreEqual(4, countAfter.Count); // 2 initial + 2 new
            
            var updatedProduct = await UowRepository.Repository<Product>().FindAsync(_sampleProducts.First().ProductId);
            ClassicAssert.AreEqual("Updated Sample 1", updatedProduct.Name);
        }

        [Test]
        [Category("Workflow")]
        [Description("Tests a multi-step operation that fails, ensuring automatic rollback.")]
        public async Task MultiStepWrite_Failure_RollsBackChanges()
        {
            // Arrange: Simulate initial state check (2 records exist)
            var countBefore = await UowRepository.Repository<Product>().GetAsync();
            ClassicAssert.AreEqual(2, countBefore.Count);

            try
            {
                // Act: Attempt a sequence of writes that includes an artificial failure point
                await UowRepository.ExecuteUnitOfWork(async () =>
                {
                    // 1. Update existing record (This change should be rolled back)
                    var productToUpdate = _sampleProducts.First();
                    productToUpdate.Name = "Attempted Update";
                    await UowRepository.Repository<Product>().UpdateAsync(productToUpdate);

                    // 2. Insert records that should NOT persist due to failure
                    var newRecords = new List<Product>()
                    {
                        new Product { Value = 99, Name = "Should Not Persist" }
                    };
                    await UowRepository.Repository<Product>().BulkInsertAsync(newRecords);

                    // INTENTIONALLY FAIL HERE TO TEST ROLLBACK
                    throw new InvalidOperationException("Simulated failure during business logic execution.");
                });

            }
            catch (InvalidOperationException ex)
            {
                // Assert: Check that the expected exception was thrown
                ClassicAssert.That(ex.Message, Is.EqualTo("Simulated failure during business logic execution."));
            }


            // Assert: All changes must be rolled back. The database should revert to its initial state.
            var countAfter = await UowRepository.Repository<Product>().GetAsync();
            ClassicAssert.AreEqual(2, countAfter.Count); 

            var updatedProductCheck = await UowRepository.Repository<Product>().FindAsync(_sampleProducts.First().ProductId);
            // Ensure the name is still the original value, proving the update failed to persist
            ClassicAssert.AreEqual("Sample 1", updatedProductCheck.Name);
        }
    }
}