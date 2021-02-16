using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore
{
    public class BaseContext : DbContext
    {
        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions options)
            : base(options)
        {
        }

        protected virtual DbContextOptionsBuilder ContextBuilder { get; set; }
        public virtual IDbContextTransaction Transaction { get; set; }

        public virtual void BeginTransaction()
        {
            Transaction = this.Database.BeginTransaction();
        }

        public override int SaveChanges()
        {
            Transaction?.Commit();
            var returnValue = base.SaveChanges();

            this.ChangeTracker.Clear();
            return returnValue;
        }

        public virtual void Commit()
        {
            _ = this.SaveChanges();
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public virtual void CommitBulkChanges()
        {
            this.BulkSaveChanges();
            Transaction?.Commit();

            this.ChangeTracker.Clear();
        }

        public virtual Task CommitBulkChangesAsync()
        {
            this.CommitBulkChanges();
            return Task.CompletedTask;
        }

        public virtual void Rollback()
        {
            Transaction?.Rollback();
            this.ChangeTracker.Clear();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            this.Transaction?.Dispose();
            base.Dispose();
        }
    }
}