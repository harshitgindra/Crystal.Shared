#region USING

using Crystal.Patterns.Abstraction;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

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
        
        public IDbContextTransaction Transaction { get; set; }

        public Task<bool> CommitAsync()
        {
            return Task.FromResult(Commit());
        }

        public void BeginTransaction()
        {
            Transaction = DbContext.Database.BeginTransaction();
        }

        public IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity:class
        {
            return instance ??= new BaseRepository<TEntity>(this.DbContext, Transaction);
        }

        public bool Commit()
        {
            var returnValue = true;

             if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                returnValue = DbContext.SaveChanges() > 0;
            }

             Transaction?.Commit();

            return returnValue;
        }

        public async Task<bool> CommitBulkChangesAsync()
        {
            var returnValue = true;

            if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                await DbContext.BulkSaveChangesAsync();
            }

            Transaction?.Commit();

            return returnValue;
        }

        public bool CommitBulkChanges()
        {
            var returnValue = true;

            if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                DbContext.BulkSaveChanges();
            }

            Transaction?.Commit();

            return returnValue;
        }

        public void Rollback()
        {
            Transaction?.Rollback();
        }
        
        public Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
            Transaction?.Dispose();
        }
    }
}