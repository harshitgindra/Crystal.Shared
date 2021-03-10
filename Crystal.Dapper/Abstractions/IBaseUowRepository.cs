using System.Data;
using System.Threading.Tasks;
using MicroOrm.Dapper.Repositories;

namespace Crystal.Dapper
{
    /// <summary>
    /// Base unit of work repository interface
    /// </summary>
    public interface IBaseUowRepository
    {
        /// <summary>
        /// Returns IBaseRepository instance of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
        /// <summary>
        /// Database connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Commiting pending changes to the database
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        /// <param name="isolationLevel">Specify the transaction locking level for the connection</param>
        /// <returns></returns>
        Task BeginTransactionAsync(IsolationLevel isolationLevel = default);

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        Task RollbackAsync();
    }
}