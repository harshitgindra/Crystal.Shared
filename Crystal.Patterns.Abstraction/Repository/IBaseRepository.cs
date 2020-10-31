#region USING

using Crystal.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// dB entity
        /// </summary>
        DbSet<TEntity> Entity { get; }
        /// <summary>
        /// Get records from the database based on  input parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get records from the database based on  input parameters
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<List<TModel>> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get queryable data from database based on input parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
        /// <summary>
        /// Get queryable data from database based on input parameters
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<IQueryable<TModel>> QueryAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get records from the database based on  input parameters
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Get first record from the database based in input parameters
        /// </summary>
        /// <param name="filter">Conditions to filter the records</param>
        /// <param name="includeProperties">specify the names of the properties that needs to be added to the entity</param>
        /// <returns></returns>
        Task<TModel> GetFirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Checks if there are any records in the dB for the query
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Returns a record from the dB with the matching primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindAsync(object id);

        /// <summary>
        /// Returns a record from the dB with the matching primary key and maps it to different model
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TModel> FindAsync<TModel>(object id);

        /// <summary>
        /// Inserts a new record in the dB
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a list of records in the dB
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete record based on primary key from the dB
        /// </summary>
        /// <param name="request">Datatable request with filters, columns, order details</param>
        /// <returns>Datatable Response with filtered data and related information</returns>
        Task DeleteAsync(object id);

        /// <summary>
        /// Delete single record from the dB
        /// </summary>
        /// <param name="request">Datatable request with filters, columns, order details</param>
        /// <returns>Datatable Response with filtered data and related information</returns>
        Task DeleteAsync(TEntity entityToDelete);

        /// <summary>
        /// Delete all entries from the dB
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task DeleteAllAsync();

        /// <summary>
        /// Update single record in the dB
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entityToUpdate);

        /// <summary>
        /// delete all the records from the dB based on the expression
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Update list of records
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);
    }
}