#region USING

using AutoMapper;
using Crystal.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseUowRepository(BaseContext context)
        {
            _context = context;
        }

        public BaseUowRepository(BaseContext context, MapperConfiguration mapperConfiguration)
        {
            _context = context;
            this.MapperConfiguration = mapperConfiguration;
        }

        public virtual IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class
        {
            return instance ??= new BaseRepository<TEntity>(this.DbContext);
        }

        public virtual DbContext DbContext => this._context;

        private readonly BaseContext _context;
        protected MapperConfiguration MapperConfiguration { get; }

        public virtual async Task BeginTransactionAsync()
        {
            if (DbContext == null)
            {
                throw new NullReferenceException("Database context is not initialized");
            }
            else
            {
                _context.Transaction = await DbContext.Database.BeginTransactionAsync();
            }
        }

        public virtual void Commit()
        {
            _context.Commit();
            _context.SaveChanges();
            _context.ChangeTracker.Clear();
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public virtual Task CommitBulkChangesAsync()
        {
            CommitBulkChanges();
            _context.ChangeTracker.Clear();
            return Task.CompletedTask;
        }

        public virtual void CommitBulkChanges()
        {
            _context.CommitBulkChanges();
            _context.ChangeTracker.Clear();
        }

        public virtual void Rollback()
        {
            _context.Rollback();
            _context.ChangeTracker.Clear();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            _context.Transaction?.Dispose();
            DbContext?.Dispose();
        }
    }
}