#region USING

using AutoMapper;
using Crystal.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    /// <summary>
    /// Base unit of work repository providing functionality to perform database operations
    /// </summary>
    public class BaseUowRepository : IBaseUowRepository
    {
        /// <summary>
        /// Base unit of work constructor
        /// </summary>
        /// <param name="context"></param>
        public BaseUowRepository(BaseContext context)
        {
            _context = context;
            _repositoryInstances = new Dictionary<Type, object>();
        }
        /// <summary>
        /// Base unit of work constructor
        /// </summary>
        /// <param name="context"></param>
        /// <param name="mapperConfiguration"></param>
        public BaseUowRepository(BaseContext context, MapperConfiguration mapperConfiguration) : this(context)
        {
            this.MapperConfiguration = mapperConfiguration;
        }
        /// <summary>
        /// Returns the instance of IBaseRepository of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class
        {
            return instance ??= new BaseRepository<TEntity>(this.DbContext);
        }

        private readonly IDictionary<Type, object> _repositoryInstances;

        /// <summary>
        /// Returns IBaseRepository instance of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
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
        /// <summary>
        /// Database context instance
        /// </summary>
        public virtual DbContext DbContext => this._context;

        private readonly BaseContext _context;
        /// <summary>
        /// Auto-mapper configuration
        /// </summary>
        protected MapperConfiguration MapperConfiguration { get; }

        /// <summary>
        /// Starts a transaction which can be used across multiple data change requests
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Commit changes to database
        /// </summary>
        public virtual void Commit()
        {
            _context.Commit();
        }
        /// <summary>
        /// Commit changes to database
        /// </summary>
        public virtual Task CommitAsync()
        {
            this.Commit();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Commit bulk changes
        /// </summary>
        public virtual Task CommitBulkChangesAsync()
        {
            CommitBulkChanges();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Commit bulk changes
        /// </summary>
        public virtual void CommitBulkChanges()
        {
            //***
            //*** Commit bulk changes
            //***
            _context.CommitBulkChanges();
        }

        /// <summary>
        /// Rollback any uncommitted changes
        /// </summary>
        /// <returns></returns>
        public virtual void Rollback()
        {
            //***
            //*** rollback the changes
            //***
            _context.Rollback();
        }

        /// <summary>
        /// Rollback any uncommitted changes
        /// </summary>
        /// <returns></returns>
        public virtual Task RollbackAsync()
        {
            this.Rollback();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Dispose dB context and repository instances
        /// </summary>
        public virtual void Dispose()
        {
            //***
            //*** Clear all repository instances
            //***
            _repositoryInstances.Clear();
        }
    }
}