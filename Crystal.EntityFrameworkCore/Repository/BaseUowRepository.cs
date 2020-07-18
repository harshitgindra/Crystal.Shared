﻿#region USING

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

        protected BaseContext DbContext { get; }
        
        public IDbContextTransaction Transaction { get; set; }

        public Task<bool> CommitAsync()
        {
            return Task.FromResult(Commit());
        }

        public void BeginTransaction()
        {
            Transaction = DbContext.Database.BeginTransaction();
        }

        public IBaseRepository<TEntity> GetInstance<TEntity>(ref IBaseRepository<TEntity> instance) where TEntity:class
        {
            if (instance == default)
            {
                instance = new BaseRepository<TEntity>(this.DbContext, Transaction);
            }

            return instance;
        }

        public bool Commit()
        {
            var returnValue = true;

             if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                returnValue = DbContext.SaveChanges() > 0;
            }

            if (Transaction != null)
            {
                Transaction.Commit();
            }

            return returnValue;
        }

        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
            }
        }


        public void Dispose()
        {
            DbContext?.Dispose();
            Transaction?.Dispose();
        }
    }
}