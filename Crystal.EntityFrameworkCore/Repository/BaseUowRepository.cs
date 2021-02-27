#region USING

using AutoMapper;
using Crystal.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseUowRepository(BaseContext context)
        {
            _context = context;
            _repositoryInstances = new Dictionary<Type, object>();
        }

        public BaseUowRepository(BaseContext context, MapperConfiguration mapperConfiguration) : this(context)
        {
            this.MapperConfiguration = mapperConfiguration;
        }

        public virtual IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class
        {
            return instance ??= new BaseRepository<TEntity>(this.DbContext);
        }

        private readonly IDictionary<Type, object> _repositoryInstances;

        public virtual IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //***
            //*** Check if the instance is already created
            //***
            if (_repositoryInstances.ContainsKey(typeof(TEntity)))
            {
                //***
                //*** Instance already created, return the instance from the dictionary
                //***
                return (IBaseRepository<TEntity>)_repositoryInstances[typeof(TEntity)];
            }
            else
            {
                //***
                //*** Create a new instance of base repository 
                //*** Save it to the dictionary
                //*** Return the instance
                //***
                var repo = new BaseRepository<TEntity>(this.DbContext);
                _repositoryInstances.Add(typeof(TEntity), repo);
                return repo;
            }
        }

        public virtual DbContext DbContext => this._context;

        private readonly BaseContext _context;
        protected MapperConfiguration MapperConfiguration { get; }

        public virtual async Task BeginTransactionAsync()
        {
            if (_context == null)
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
        }

        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        public virtual Task CommitBulkChangesAsync()
        {
            CommitBulkChanges();
            return Task.CompletedTask;
        }

        public virtual void CommitBulkChanges()
        {
            //***
            //*** Commit bulk changes
            //***
            _context.CommitBulkChanges();
        }

        public virtual void Rollback()
        {
            //***
            //*** rollback the changes
            //***
            _context.Rollback();
        }

        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            //***
            //*** Dispose the dB context
            //***
            _context?.Dispose();
        }
    }
}