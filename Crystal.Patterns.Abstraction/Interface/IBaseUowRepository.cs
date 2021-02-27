#region USING
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
#endregion

namespace Crystal.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        /// DbContext instance
        /// </summary>
        DbContext DbContext { get; }

        /// <summary>
        /// Commiting pending changes to the database
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        Task BeginTransactionAsync();

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Commit bulk changes
        /// </summary>
        /// <returns></returns>
        Task CommitBulkChangesAsync();

        /// <summary>
        /// Returns the base repository instance of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}
