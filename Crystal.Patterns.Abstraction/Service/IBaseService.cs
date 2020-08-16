using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get all entity records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default);

        /// <summary>
        /// Get all entity records
        /// </summary>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(CancellationToken token = default);

        /// <summary>
        /// Get all entity records based on foreign key identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync(int id, CancellationToken token = default);

        /// <summary>
        /// Get all entity records based on foreign key identifier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetAll(int id, CancellationToken token = default);

        /// <summary>
        /// Insert entity record to the data source
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(TEntity item, CancellationToken token = default);

        /// <summary>
        /// Insert multiple entity records to the data source
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<bool> InsertAsync(IEnumerable<TEntity> item, CancellationToken token = default);

        /// <summary>
        /// Delete the entry with the id from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(int id, CancellationToken token = default);

        /// <summary>
        /// Delete the entry with the id from the system
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool Delete(int id, CancellationToken token = default);
    }
}