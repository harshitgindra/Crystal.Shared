#region USING

using AutoMapper;
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
            this.DbContext = context;
        }

        public BaseUowRepository(BaseContext context, MapperConfiguration mapperConfiguration)
        {
            this.DbContext = context;
            this.MapperConfiguration = mapperConfiguration;
        }

        public IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class
        {
            return instance ??= new BaseRepository<TEntity>(this.DbContext);
        }

        protected BaseContext DbContext { get; }
        protected MapperConfiguration MapperConfiguration { get; }

        public void BeginTransaction()
        {
            if (DbContext == null)
            {
                throw new NullReferenceException("Database context is not initialized");
            }
            else
            {
                this.DbContext.Transaction = DbContext.Database.BeginTransaction();
            }
        }

        public virtual void Commit()
        {
            DbContext.Commit();
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public virtual Task CommitBulkChangesAsync()
        {
            this.DbContext.CommitBulkChanges();
            return Task.CompletedTask;
        }

        public virtual void CommitBulkChanges()
        {
            this.DbContext.CommitBulkChanges();
        }

        public virtual void Rollback()
        {
            this.DbContext.Rollback();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            DbContext.Transaction?.Dispose();
            DbContext?.Dispose();
        }
    }
}