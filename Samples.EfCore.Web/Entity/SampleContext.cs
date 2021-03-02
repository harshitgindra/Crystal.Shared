using Crystal.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Samples.EfCore.Web
{
    public class SampleContext : BaseContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }


        public SampleContext()
        {
            this.Database?.EnsureCreated();
        }

        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
            this.Database?.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("filename=sample.sqlite");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
