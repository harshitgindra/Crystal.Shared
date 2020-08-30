using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

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

        public override int SaveChanges()
        {
            var returnValue = base.SaveChanges();
            var entityEntries = this.ChangeTracker.Entries();

            foreach (var entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
            return returnValue;
        }
    }
}