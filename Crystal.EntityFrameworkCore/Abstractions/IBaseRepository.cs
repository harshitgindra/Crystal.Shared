#region USING

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    /// <summary>
    /// Base repository interface to perform dB operations
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IBaseRepository<TEntity> : IDisposable
       where TEntity : class
    {
        /// <summary>
        /// Database entity
        /// </summary>
        DbSet<TEntity> Entity { get; }

        /// <summary>
        /// Returns IEnumerable of data based on filters
        /// </summary>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IEnumerable of data</returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Returns IEnumerable of data based on filters
        /// </summary>
        /// <typeparam name="TModel">Model to which result should be mapped</typeparam>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IEnumerable of data</returns>
        Task<List<TModel>> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Returns IQueryable of data based on filters
        /// </summary>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>IQueryable of data</returns>
        Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        ///  Returns IQueryable of data based on filters
        /// </summary>
        /// <typeparam name="TModel">Model to which result should be mapped to</typeparam>
        /// <param name="filter">Where expression to filer the data</param>
        /// <param name="orderBy">Specifies the sorting of data</param>
        /// <param name="includes">Specify additional columns/tables that should be include in the response</param>
        /// <returns>Maps the data to TModel and return IQueryable of data</returns>
        Task<IQueryable<TModel>> QueryAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets first or default record based on filter
        /// </summary>
        /// <param name="filter">Where expression to filter the data</param>
        /// <param name="includes">Properties to be included in the response</param>
        /// <returns></returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Gets first or default record based on filter
        /// Maps the response to TModel
        /// </summary>
        /// <typeparam name="TModel">Model to which the response should be mapped to</typeparam>
        /// <param name="filter">Where expression to filter the data</param>
        /// <param name="includes">Properties to be included in the response</param>
        /// <returns></returns>
        Task<TModel> GetFirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Return true/false if the record exists based on filter
        /// </summary>
        /// <param name="filter">Where expression based on filter</param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Finds and returns the entity based on primary key of the table
        /// </summary>
        /// <param name="id">primary key of the table</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(object id);

        /// <summary>
        /// Finds and returns the mapped model based on primary key of the table
        /// </summary>
        /// <typeparam name="TModel">Model to which the response should be mapped to</typeparam>
        /// <param name="id">primary key of the table</param>
        /// <returns></returns>
        Task<TModel> FindAsync<TModel>(object id);

        /// <summary>
        /// Inserts a record to the database
        /// </summary>
        /// <param name="entity">entity to be inserted</param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts multiple records to the database
        /// </summary>
        /// <param name="entities">List of entities to be inserted</param>
        /// <returns></returns>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="id">Primary identifier of the entity</param>
        /// <returns></returns>
        Task DeleteAsync(object id);

        /// <summary>
        /// Deletes the record from the database
        /// </summary>
        /// <param name="entity">entity to be deleted</param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes all the records from the table
        /// </summary>
        /// <returns></returns>
        Task DeleteAllAsync();

        /// <summary>
        /// Updates the entity to the database
        /// </summary>
        /// <param name="entity">Record to the updated</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Deletes the record from the context
        /// </summary>
        /// <param name="filter">filter to select records to be deleted</param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Updates list of records in the database
        /// </summary>
        /// <param name="entities">list of entities to be updated</param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Executes raw sql query on the context
        /// </summary>
        /// <param name="sql">sql statement to be executed</param>
        /// <param name="parameters">parameters to be injected in the sql query</param>
        /// <returns>IQueryable of TEntity</returns>
        //Task<IQueryable<TEntity>> RunSql(string sql, params object[] parameters);

        /// <summary>
        /// Deletes all records from the table
        /// <remarks>This method will commit the changes to the database</remarks>
        /// </summary>
        Task BulkDeleteAllAsync();

        /// <summary>
        /// Bulk inserts the records to the database
        /// </summary>
        /// <param name="entities">list of entities to be inserted</param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<TEntity> entities);
    }
}
