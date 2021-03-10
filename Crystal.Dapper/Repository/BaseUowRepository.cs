using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.DbContext;
using System;
using System.Collections.Generic;
using System.Data;
using Crystal.Shared;
using System.Threading.Tasks;
using AutoMapper;

namespace Crystal.Dapper
{
    /// <summary>
    /// Base unit of work repository
    /// </summary>
    public class BaseUowRepository : DapperDbContext, IBaseUowRepository
    {
        private readonly IDictionary<Type, object> _repositoryInstances;
        private IDbTransaction _dbTransaction;
        private readonly IMapper _mapper;
        /// <summary>
        /// Base unit of work constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mapper"></param>
        public BaseUowRepository(IDbConnection connection, IMapper mapper = default)
            : base(connection)
        {
            _mapper = mapper;
            _repositoryInstances = new Dictionary<Type, object>();
        }

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
                var repo = new BaseRepository<TEntity>(this.Connection, _mapper, _dbTransaction);
                _repositoryInstances.Add(typeof(TEntity), repo);
                return repo;
            }
        }

        public new void Dispose()
        {
            //***
            //*** Dispose all base repository instances
            //***
            foreach (var item in _repositoryInstances)
            {
                item.Value.TryDispose();
            }
            //***
            //*** Clear all repository instances
            //***
            _repositoryInstances.Clear();
            //***
            //*** Dispose dB context
            //***
            this.Connection?.Close();
            this.Connection?.Dispose();
            base.Dispose();
        }

        /// <summary>
        /// Commit changes to database
        /// </summary>
        public virtual Task CommitAsync()
        {
            //***
            //*** rollback the changes
            //***
            if (_dbTransaction == null)
            {
                throw new Exception("Transaction not initialized");
            }
            _dbTransaction.Commit();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Starts a transaction which can be used across multiple data change requests
        /// </summary>
        /// <returns></returns>
        public virtual Task BeginTransactionAsync(IsolationLevel isolationLevel = default)
        {
            if (this.Connection == null)
            {
                throw new NullReferenceException("Database connection is not initialized");
            }
            else
            {
                _dbTransaction = this.Connection?.BeginTransaction(isolationLevel);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Rollback any uncommitted changes
        /// </summary>
        /// <returns></returns>
        public virtual Task RollbackAsync()
        {
            //***
            //*** rollback the changes
            //***
            if (_dbTransaction == null)
            {
                throw new Exception("Transaction not initialized");
            }
            _dbTransaction.Rollback();
            return Task.CompletedTask;
        }
    }
}
