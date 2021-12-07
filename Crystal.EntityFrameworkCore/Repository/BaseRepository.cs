#region USING

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    /// <summary>
    /// Base repository to perform dB operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    internal class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        /// <summary>
        /// Database entity
        /// </summary>
        public virtual DbSet<TEntity> Entity { get; }
        /// <summary>
        /// Base repository constructor
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">Auto-mapper configuration</param>
        public BaseRepository(DbContext context, IMapper mapper = default)
        {
            _mapper = mapper;
            _context = context;
            Entity = context.Set<TEntity>();
        }
        /// <summary>
        /// Returns IEnumerable of data based on filters
        /// </summary>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IEnumerable of data</returns>
        public virtual async Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            //***
            //*** Get IQueryable of data based in input
            //***
            var query = await this.QueryAsync(filter, orderBy, includes);
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
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IEnumerable of data</returns>
        public virtual async Task<List<TModel>> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            //***
            //*** Get IQueryable of data based in input
            //***
            var query = await this.QueryAsync<TModel>(filter, orderBy, includes);
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
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IQueryable of data</returns>
        public virtual Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            //***
            //*** Disable tracking on the entity
            //***
            IQueryable<TEntity> query = Entity.AsNoTracking();

            //***
            //*** Check if includes is null, 
            //***
            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    //***
                    //*** Include additional properties in the response
                    //***
                    query = query.Include(include);
                }
            }
            //***
            //*** Check if filter is null
            //***
            if (filter != null)
            {
                //***
                //*** Apply where expression to filter the data
                //***
                query = query.Where(filter);
            }
            //***
            //*** Check if order by is null
            //***
            if (orderBy != null)
            {
                //***
                //*** Order the data based on input
                //***
                query = orderBy(query);
            }
            //***
            //*** Return IQueryable of data
            //***
            return Task.FromResult(query);
        }

        /// <summary>
        ///  Returns IQueryable of data based on filters
        /// </summary>
        /// <typeparam name="TModel">Model to which result should be mapped to</typeparam>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>Maps the data to TModel and return IQueryable of data</returns>
        public virtual async Task<IQueryable<TModel>> QueryAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
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
                var query = await this.QueryAsync(filter, orderBy, includes);
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
        /// <param name="includes">Properties to be included in the response</param>
        /// <returns></returns>
        public virtual async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            //***
            //*** Query the results based on filter and includes input
            //***
            var query = await this.QueryAsync(filter, null, includes);
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
        /// <param name="includes">Properties to be included in the response</param>
        /// <returns></returns>
        public virtual async Task<TModel> GetFirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            //***
            //*** Query the results based on filter and includes input
            //***
            var query = await this.QueryAsync<TModel>(filter, null, includes);
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
            //*** Disable tracking on the entity
            //***
            var query = Entity.AsNoTracking();
            //***
            //*** Check if filter is null or not
            //***
            if (filter == null)
            {
                //***
                //*** Return bool if record exists or not
                //***
                return Task.FromResult(query.Any());
            }
            else
            {
                //***
                //*** Return bool if record exists or not based on filter
                //***
                return Task.FromResult(query.Any(filter));
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
            return Task.FromResult(Entity.Find(id));
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
        public virtual Task InsertAsync(TEntity entity)
        {
            //***
            //*** Attach entity to the tracker
            //***
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                Entity.Attach(entity);
            }

            //***
            //*** Update the entity state to added
            //***
            _context.Entry(entity).State = EntityState.Added;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Inserts multiple records to the database
        /// </summary>
        /// <param name="entities">List of entities to be inserted</param>
        /// <returns></returns>
        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple records to the entity
            //***
            Entity.AddRange(entities);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Bulk inserts the records to the database
        /// </summary>
        /// <param name="entities">list of entities to be inserted</param>
        /// <returns></returns>
        public virtual Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add bulk records to the database
            //***
            Entity.BulkInsert(entities);
            return Task.CompletedTask;
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
            await this.DeleteAsync(await Entity.FindAsync(id));
        }
        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="entity">entity to be deleted</param>
        /// <returns></returns>
        public virtual Task DeleteAsync(TEntity entity)
        {
            //***
            //*** Attach entity to the tracker
            //***
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                Entity.Attach(entity);
            }

            //***
            //*** Update the entity state to deleted
            //***
            _context.Entry(entity).State = EntityState.Deleted;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Deletes all records from the table
        /// <remarks>This method will commit the changes to the database</remarks>
        /// </summary>
        public virtual Task BulkDeleteAllAsync()
        {
            //***
            //*** Bulk delete
            //***
            Entity.BulkDelete(Entity);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes all the records from the table
        /// </summary>
        /// <returns></returns>
        public virtual Task DeleteAllAsync()
        {
            //***
            //*** Delete all entities
            //***
            Entity.RemoveRange(Entity);
            return Task.CompletedTask;
        }
        /// <summary>
        /// Updates the entity to the database
        /// </summary>
        /// <param name="entity">Record to the updated</param>
        /// <returns></returns>
        public virtual Task UpdateAsync(TEntity entity)
        {
            //***
            //*** Attach entity to the tracker
            //***
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                Entity.Attach(entity);
            }

            //***
            //*** Update the entity state to modified
            //***
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        /// <summary>
        /// Deletes the record from the context
        /// </summary>
        /// <param name="filter">filter to select records to be deleted</param>
        /// <returns></returns>
        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Remove/Delete the entities based on filter
            //***
            Entity.RemoveRange(Entity.Where(filter));
            return Task.CompletedTask;
        }
        /// <summary>
        /// Disposes the dBContext
        /// </summary>
        public virtual void Dispose()
        {
            //***
            //*** Dispose the context and transaction
            //***
            _context?.Dispose();
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
        /// Executes raw sql query on the context
        /// </summary>
        /// <param name="sql">sql statement to be executed</param>
        /// <param name="parameters">parameters to be injected in the sql query</param>
        /// <returns>IQueryable of TEntity</returns>
        public virtual Task<IQueryable<TEntity>> RunSql(string sql, params object[] parameters)
        {
            //***
            //*** Execute raw sql query with parameters against the entity
            //***
            //return Task.FromResult(this.Entity.FromSqlRaw(sql, parameters));
            return null;
        }
    }
}