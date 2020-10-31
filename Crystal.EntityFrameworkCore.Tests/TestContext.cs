using Microsoft.EntityFrameworkCore;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class TestContext : BaseContext
    {
        public DbSet<Order> Orders { get; set; }

        public TestContext()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename=testdb.sqlite");
            //optionsBuilder.UseInMemoryDatabase($"TestDb");

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
