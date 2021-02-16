using Crystal.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Samples.EfCore.Web
{
    public class SampleContext : BaseContext
    {
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }

        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
            this.Database?.EnsureCreated();
        }
    }
}
