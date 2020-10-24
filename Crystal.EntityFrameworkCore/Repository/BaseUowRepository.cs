#region USING

using Crystal.Patterns.Abstraction;
using System;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseUowRepository(BaseContext context)
        {
            DbContext = context;
        }

        public BaseUowRepository()
        {
            if (DbContext == null)
            {
                DbContext = new BaseContext();
            }
        }

        public BaseContext DbContext { get; }

        public void BeginTransaction()
        {
            this.DbContext.Transaction = DbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            DbContext.Commit();
        }

        public Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public Task CommitBulkChangesAsync()
        {
            this.DbContext.CommitBulkChanges();
            return Task.CompletedTask;
        }

        public void CommitBulkChanges()
        {
            this.DbContext.CommitBulkChanges();
        }

        public void Rollback()
        {
            this.DbContext.Rollback();
        }

        public Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DbContext.Transaction?.Dispose();
            DbContext?.Dispose();
        }
    }
}