#region USING

using AutoMapper;
using Crystal.Shared;
using MicroOrm.Dapper.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories.SqlGenerator;

#endregion

namespace Crystal.Dapper
{
    /// <summary>
    /// Base repository to perform dB operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly IDbConnection _connection;

        private readonly IDapperRepository<TEntity> _repository;
        private readonly IDbTransaction _dbTransaction;
        private readonly IMapper _mapper;

        /// <summary>
        /// Base repository constructor
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="mapper"></param>
        /// <param name="dbTransaction"></param>
        public BaseRepository(IDbConnection connection, IMapper mapper = default, IDbTransaction dbTransaction = default)
        {
            _connection = connection;
            _dbTransaction = dbTransaction;
            _mapper = mapper;

            switch (connection.GetType().Name.ToLower())
            {
                case "sqliteconnection":
                    _repository = new DapperRepository<TEntity>(connection, new SqlGenerator<TEntity>(SqlProvider.SQLite));
                    break;
                default:
                    _repository = new DapperRepository<TEntity>(connection, new SqlGenerator<TEntity>());
                    break;
            }
        }

        /// <summary>
        /// Returns IEnumerable of data based on filters
        /// </summary>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <returns>IEnumerable of data</returns>
        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            //***
            //*** Get IQueryable of data based in input
            //***
            var query = await this.QueryAsync(filter, orderBy);
            //***
            //*** Execute the query and return the results
            //***
            return query.ToList();
        }

        /// <summary>
        /// Returns IEnumerable of data based on filters
        /// </summary>
        /// <typeparam name="TModel">Model to which result should be mapped</typeparam>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <returns>IEnumerable of data</returns>
        public virtual async Task<List<TModel>> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            //***
            //*** Get IQueryable of data based in input
            //***
            var query = await this.QueryAsync<TModel>(filter, orderBy);
            //***
            //*** Map the results to TModel and return the results
            //***
            return query.ToList();
        }

        /// <summary>
        /// Returns IQueryable of data based on filters
        /// </summary>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <returns>IQueryable of data</returns>
        public virtual Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            var repo = new DapperRepository<TEntity>(_connection);

            var result = repo.FindAll(filter)
                .AsQueryable();
            //***
            //*** Order the results if order by is not null
            //***
            if (orderBy != null)
            {
                result = orderBy(result);
            }

            //***
            //*** Return IQueryable of data
            //***
            return Task.FromResult(result);
        }

        /// <summary>
        ///  Returns IQueryable of data based on filters
        /// </summary>
        /// <typeparam name="TModel">Model to which result should be mapped to</typeparam>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <returns>Maps the data to TModel and return IQueryable of data</returns>
        public virtual async Task<IQueryable<TModel>> QueryAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (_mapper == null)
            {
                throw new Exception("Auto-mapper not configured");
            }
            else
            {
                //***
                //*** Get queryable data based in input
                //***
                var query = await this.QueryAsync(filter, orderBy);
                //***
                //*** Maps the result to TModel and return the data
                //***

                return _mapper.ProjectTo<TModel>(query);
            }
        }
        /// <summary>
        /// Gets first or default record based on filter
        /// </summary>
        /// <param name="filter">Where expression to filter the data</param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            //***
            //*** Query the results based on filter and includes input
            //***
            var query = await this.QueryAsync(filter, null);
            //***
            //*** Return first or default result
            //***
            return query.FirstOrDefault();
        }
        /// <summary>
        /// Gets first or default record based on filter
        /// Maps the response to TModel
        /// </summary>
        /// <typeparam name="TModel">Model to which the response should be mapped to</typeparam>
        /// <param name="filter">Where expression to filter the data</param>
        /// <returns></returns>
        public virtual async Task<TModel> GetFirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>> filter = null)
        {
            //***
            //*** Query the results based on filter and includes input
            //***
            var query = await this.QueryAsync<TModel>(filter, null);
            //***
            //*** Return first or default
            //***
            return query.FirstOrDefault();
        }

        /// <summary>
        /// Return true/false if the record exists based on filter
        /// </summary>
        /// <param name="filter">Where expression based on filter</param>
        /// <returns></returns>
        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            //***
            //*** Check if filter is null or not
            //***
            if (filter == null)
            {
                //***
                //*** Return bool if record exists or not
                //***
                return Task.FromResult(_repository.Count() > 0);
            }
            else
            {
                //***
                //*** Return bool if record exists or not based on filter
                //***
                return Task.FromResult(_repository.Count(filter) > 0);
            }
        }

        /// <summary>
        /// Finds and returns the entity based on primary key of the table
        /// </summary>
        /// <param name="id">primary key of the table</param>
        /// <returns></returns>
        public virtual Task<TEntity> FindAsync(object id)
        {
            //***
            //*** Find the entity based on id and return the result
            //***
            return Task.FromResult(_repository.FindById(id));
        }
        /// <summary>
        /// Finds and returns the mapped model based on primary key of the table
        /// </summary>
        /// <typeparam name="TModel">Model to which the response should be mapped to</typeparam>
        /// <param name="id">primary key of the table</param>
        /// <returns></returns>
        public virtual async Task<TModel> FindAsync<TModel>(object id)
        {
            //***
            //*** Check if mapper configuration is null
            //***
            if (_mapper == null)
            {
                throw new Exception("Auto-mapper not configured");
            }
            else
            {
                //***
                //*** Find the entity based on id
                //***
                var record = await this.FindAsync(id);
                //***
                //*** Map the record to TModel and return the result
                //***
                return _mapper.Map<TModel>(record);
            }
        }

        /// <summary>
        /// Inserts a record to the database
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        /// <returns></returns>
        public virtual async Task<bool> InsertAsync(TEntity entity)
        {
            //***
            //*** Insert the entity into the database
            //***
            return await _repository.InsertAsync(entity, _dbTransaction);
        }
        /// <summary>
        /// Inserts multiple records to the database
        /// </summary>
        /// <param name="entities">List of entities to be inserted</param>
        /// <returns></returns>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple records to the entity
            //***
            foreach (var item in entities)
            {
                await _repository.InsertAsync(item, _dbTransaction);
            }
        }
        /// <summary>
        /// Bulk inserts the records to the database
        /// </summary>
        /// <param name="entities">list of entities to be inserted</param>
        /// <returns></returns>
        public virtual async Task<int> BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add bulk records to the database
            //***
            return await _repository.BulkInsertAsync(entities, _dbTransaction);
        }
        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="id">Primary identifier of the entity</param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(object id)
        {
            //***
            //*** Find the entity and delete it
            //***
            await this.DeleteAsync(await _repository.FindByIdAsync(id));
        }
        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="entity">entity to be deleted</param>
        /// <returns></returns>
        public virtual Task DeleteAsync(TEntity entity, TimeSpan timeout = default)
        {
            var tempTimeout = timeout;
            if (tempTimeout == default)
            {
                tempTimeout = TimeSpan.FromSeconds(30);
            }
            //***
            //*** Delete the entity
            //***
            _repository.Delete(entity, _dbTransaction, tempTimeout);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes all the records from the table
        /// </summary>
        /// <param name="timeout">configure timeout value for query execution</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAllAsync(TimeSpan timeout = default)
        {
            var tempTimeout = timeout;
            if (tempTimeout == default)
            {
                tempTimeout = TimeSpan.FromSeconds(30);
            }
            //***
            //*** Delete all entities
            //***
            foreach (var item in _repository.FindAll(_dbTransaction))
            {
                await _repository.DeleteAsync(item, _dbTransaction, tempTimeout);
            }

            return true;
        }
        /// <summary>
        /// Updates the entity to the database
        /// </summary>
        /// <param name="entity">Record to the updated</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            //***
            //*** Update the entity and save it
            //***
            return await _repository.UpdateAsync(entity, _dbTransaction);
        }
        /// <summary>
        /// Deletes the record from the context
        /// </summary>
        /// <param name="filter">filter to select records to be deleted</param>
        /// <param name="timeout">configure timeout value for query execution</param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter, TimeSpan timeout = default)
        {
            var tempTimeout = timeout;
            if (tempTimeout == default)
            {
                tempTimeout = TimeSpan.FromSeconds(30);
            }
            //***
            //*** Remove/Delete the entities based on filter
            //***
            return await _repository.DeleteAsync(filter, _dbTransaction, tempTimeout);
        }
        /// <summary>
        /// Disposes the dBContext
        /// </summary>
        public virtual void Dispose()
        {
            //***
            //*** Dispose the context and transaction
            //***
            _mapper?.TryDispose();
            _dbTransaction?.Dispose();
            _repository?.Dispose();
        }
        /// <summary>
        /// Updates list of records in the database
        /// </summary>
        /// <param name="entities">list of entities to be updated</param>
        /// <returns></returns>
        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Loop through all entities to be updated
            //***
            foreach (var item in entities)
            {
                //***
                //*** Update the entity
                //***
                await this.UpdateAsync(item);
            }
        }

        /// <summary>
        /// Bulk updates list of records in the database
        /// </summary>
        /// <param name="entities">list of entities to be updated</param>
        /// <returns></returns>
        public virtual async Task<bool> BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Bulk update all entity records
            //***
            return await _repository.BulkUpdateAsync(entities, _dbTransaction);
        }
    }
}