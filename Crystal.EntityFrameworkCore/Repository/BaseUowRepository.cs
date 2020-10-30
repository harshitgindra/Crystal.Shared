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
            Context = context;
        }

        public IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class
        {
            return instance ??= new BaseRepository<TEntity>(this.Context);
        }

        public BaseContext Context { get; }

        public void BeginTransaction()
        {
            if (Context == null)
            {
                throw new NullReferenceException("Database context is not initialized");
            }
            else
            {
                this.Context.Transaction = Context.Database.BeginTransaction();
            }
        }

        public virtual void Commit()
        {
            Context.Commit();
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public virtual Task CommitBulkChangesAsync()
        {
            this.Context.CommitBulkChanges();
            return Task.CompletedTask;
        }

        public virtual void CommitBulkChanges()
        {
            this.Context.CommitBulkChanges();
        }

        public virtual void Rollback()
        {
            this.Context.Rollback();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            Context.Transaction?.Dispose();
            Context?.Dispose();
        }
    }
}