using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crystal.Core.Shared.Abstraction
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all entity records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();
        /// <summary>
        /// Get all entity records based on foreign key identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(int id);
        /// <summary>
        /// Insert entity record to the data source
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(TEntity item);
        /// <summary>
        /// Insert multiple entity records to the data source
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(IEnumerable<TEntity> item);
        /// <summary>
        /// Delete the entry with the id from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id);
        /// <summary>
        /// Delete the entry with the id from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       bool Delete(int id);
    }
}
