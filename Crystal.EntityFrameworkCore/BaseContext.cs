using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        public IDbContextTransaction Transaction { get; set; }

        public void BeginTransaction()
        {
            Transaction = this.Database.BeginTransaction();
        }

        public override int SaveChanges()
        {
            var returnValue = base.SaveChanges();
            Transaction?.Commit();
            var entityEntries = this.ChangeTracker.Entries();

            foreach (var entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
            return returnValue;
        }

        public void Commit()
        {
            _ = this.SaveChanges();
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public void CommitBulkChanges()
        {
            this.BulkSaveChanges();
            Transaction?.Commit();
            var entityEntries = this.ChangeTracker.Entries();

            foreach (var entityEntry in entityEntries)
            {
                entityEntry.State = EntityState.Detached;
            }
        }

        public virtual Task CommitBulkChangesAsync()
        {
            this.CommitBulkChanges();
            return Task.CompletedTask;
        }

        public virtual void Rollback()
        {
            Transaction?.Rollback();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }
    }
}