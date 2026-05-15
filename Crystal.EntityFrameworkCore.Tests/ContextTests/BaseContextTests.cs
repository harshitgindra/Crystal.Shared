using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Crystal.EntityFrameworkCore.Tests.ContextTests
{
    [TestFixture]
    public class BaseContextTests
    {
        private class TestEntity
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
        }

        private class BaseContextProbe : BaseContext
        {
            public BaseContextProbe(DbContextOptions<BaseContextProbe> options)
                : base(options)
            {
            }

            public DbSet<TestEntity> Entities { get; set; } = null!;
        }

        private static BaseContextProbe BuildContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<BaseContextProbe>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            return new BaseContextProbe(options);
        }

        [Test]
        public void SaveChanges_ClearsChangeTracker()
        {
            using var context = BuildContext(nameof(SaveChanges_ClearsChangeTracker));
            context.Entities.Add(new TestEntity { Name = "one" });

            var affected = context.SaveChanges();

            ClassicAssert.AreEqual(1, affected);
            ClassicAssert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }

        [Test]
        public async Task CommitAsync_WithoutTransaction_SavesAndClearsTracker()
        {
            await using var context = BuildContext(nameof(CommitAsync_WithoutTransaction_SavesAndClearsTracker));
            context.Entities.Add(new TestEntity { Name = "two" });

            await context.CommitAsync();

            ClassicAssert.AreEqual(1, await context.Entities.CountAsync());
            ClassicAssert.AreEqual(0, context.ChangeTracker.Entries().Count());
        }

        [Test]
        public void Rollback_WithoutTransaction_ClearsTrackedChanges()
        {
            using var context = BuildContext(nameof(Rollback_WithoutTransaction_ClearsTrackedChanges));
            context.Entities.Add(new TestEntity { Name = "three" });

            context.Rollback();

            ClassicAssert.AreEqual(0, context.ChangeTracker.Entries().Count());
            ClassicAssert.AreEqual(0, context.Entities.Count());
        }

        [Test]
        public async Task RollbackAsync_WithoutTransaction_ClearsTrackedChanges()
        {
            await using var context = BuildContext(nameof(RollbackAsync_WithoutTransaction_ClearsTrackedChanges));
            context.Entities.Add(new TestEntity { Name = "four" });

            await context.RollbackAsync();

            ClassicAssert.AreEqual(0, context.ChangeTracker.Entries().Count());
            ClassicAssert.AreEqual(0, await context.Entities.CountAsync());
        }

        [Test]
        public void Dispose_WithNoTransaction_DoesNotThrow()
        {
            var context = BuildContext(nameof(Dispose_WithNoTransaction_DoesNotThrow));
            Assert.DoesNotThrow(() => context.Dispose());
        }
    }
}
