using Crystal.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Samples.EfCore.Web
{
    public class SampleContext : BaseContext
    {
        private readonly string _connectionString;

        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        //public SampleContext(string connectionString)
        //{
        //    _connectionString = connectionString;
        //}

        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
            this.Database?.EnsureCreated();
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite(_connectionString);
        //    base.OnConfiguring(optionsBuilder);
        //}
    }
}
