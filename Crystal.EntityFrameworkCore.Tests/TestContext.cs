using Microsoft.EntityFrameworkCore;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class TestContext : BaseContext
    {
        public DbSet<Order> Orders { get; set; }

        public TestContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase($"TestDb");
        }
    }
}
