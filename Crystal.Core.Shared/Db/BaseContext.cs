using Microsoft.EntityFrameworkCore;

namespace Crystal.Core.Shared.Db
{
    public class BaseContext : DbContext
    {
        protected DbContextOptionsBuilder ContextBuilder { get;set;}

        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }
    }
}
