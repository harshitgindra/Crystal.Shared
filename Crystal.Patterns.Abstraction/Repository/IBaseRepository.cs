#region USING

using Crystal.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseRepository<TEntity>: IDisposable where TEntity : class
    {
        /// <summary>
        ///     Get records from the database based on  input parameters
        /// </summary>
        /// <param name="filter">Conditions to filter the records</param>
        /// <returns></returns>
        TEntity[] GetAll(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        /// <summary>
        ///     Get records from the database based on  input parameters
        /// </summary>
        /// <param name="filter">Conditions to filter the records</param>
        /// <param name="includeProperties">specify the names of the properties that needs to be added to the entity</param>
        /// <returns></returns>
        Task<TEntity[]> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");

        /// <summary>
        ///     Fetchs the record from the database based on unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the table</param>
        /// <returns>Entity record</returns>
        TEntity Get(object id);

        /// <summary>
        ///     Fetchs the record from the database based on unique identifier
        /// </summary>
        /// <param name="id">Unique identifier of the table</param>
        /// <returns>Entity record</returns>
        Task<TEntity> GetAsync(object id);

        /// <summary>
        ///     Get records from the database based on DataTableRequest model
        /// </summary>
        /// <param name="request">Datatable request with filters, columns, order details</param>
        /// <returns>Datatable Response with filtered data and related information</returns>
        DataTableResponse<TEntity> GetAll(DataTableRequest<TEntity> request);

        /// <summary>
        ///     Get records from the database based on DataTableRequest model
        /// </summary>
        /// <param name="request">Datatable request with filters, columns, order details</param>
        /// <returns>Datatable Response with filtered data and related information</returns>
        Task<DataTableResponse<TEntity>> GetAllAsync(DataTableRequest<TEntity> request);

        /// <summary>
        /// Get the record from the dB based on expression asynchronously
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "");

        /// <summary>
        /// Get the record from the dB based on expression asynchronously
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="includeProperties"></param>
        /// <returns></returns>
        TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "");

        /// <summary>
        /// Insert a record in the table
        /// </summary>
        /// <param name="entity">entity record to be inserted</param>
        /// <returns></returns>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert list of records in the table
        /// </summary>
        /// <param name="entities">List of entity records to be inserted</param>
        /// <returns></returns>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert a record in the table
        /// </summary>
        /// <param name="entity">entity record to be inserted</param>
        /// <returns></returns>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Insert list of records in the table
        /// </summary>
        /// <param name="entities">List of entity records to be inserted</param>
        /// <returns></returns>
        Task InsertAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert list of records in the table
        /// </summary>
        /// <param name="entities">List of entity records to be inserted</param>
        /// <returns></returns>
        void BulkInsert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Insert list of records in the table
        /// </summary>
        /// <param name="entities">List of entity records to be inserted</param>
        /// <returns></returns>
        Task BulkInsertAsync(IEnumerable<TEntity> entities);

        #region Update methods
        /// <summary>
        ///     Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entities">entity record to be updated</param>
        /// <returns></returns>
        Task UpdateAsync(TEntity entities);

        /// <summary>
        /// Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entityToUpdate">entity record to be updated</param>
        /// <returns></returns>
        Task UpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entityToUpdate">entity record to be updated</param>
        /// <returns></returns>
        Task BulkUpdateAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entityToUpdate">entity record to be updated</param>
        /// <returns></returns>
        void BulkUpdate(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entityToUpdate">entity record to be updated</param>
        /// <returns></returns>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        ///     Update the entity in the database based on unique identifier
        /// </summary>
        /// <param name="entityToUpdate">entity record to be updated</param>
        /// <returns></returns>
        void Update(TEntity entityToUpdate); 
        #endregion

        #region Delete methods
        /// <summary>
        /// Delete the record entity from the databae
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        void Delete(TEntity entityToDelete);

        /// <summary>
        /// Delete the record based on the id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(object id);

        /// <summary>
        /// Delete the record entity from the databae
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        Task DeleteAsync(TEntity entityToDelete);

        /// <summary>
        /// Delete all records from the table
        /// </summary>
        /// <returns></returns>
        void DeleteAll();

        /// <summary>
        /// Delete the record based on the id from the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(object id);

        /// <summary>
        /// Delete the record based on the query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        void Delete(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Delete the record based on the query
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Delete all records from the table
        /// </summary>
        /// <returns></returns>
        Task DeleteAllAsync();

        /// <summary>
        /// Bulk delete records from the table
        /// </summary>
        void BulkDelete();

        /// <summary>
        /// Bulk delete records from the table
        /// </summary>
        /// <returns></returns>
        Task BulkDeleteAsync();

        /// <summary>
        /// Bulk delete records from the table using the expression
        /// </summary>
        /// <param name="filter"></param>
        void BulkDelete(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// Bulk delete records from the table using the expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        Task BulkDeleteAsync(Expression<Func<TEntity, bool>> filter); 
        #endregion

        /// <summary>
        /// Checks if record exists in the table based on filter
        /// </summary>
        /// <param name="filter">conditions to filter the records</param>
        /// <returns></returns>
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null);

        /// <summary>
        /// Checks if record exists in the table based on filter
        /// </summary>
        /// <param name="filter">conditions to filter the records</param>
        /// <returns></returns>
        bool Any(Expression<Func<TEntity, bool>> filter = null);
    }
}