using Microsoft.EntityFrameworkCore;

namespace Crystal.EntityFrameworkCore
{
    public class BaseContext : DbContext
    {
        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }

        protected DbContextOptionsBuilder ContextBuilder { get; set; }
    }
}