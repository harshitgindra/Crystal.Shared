using System;
using System.Threading.Tasks;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        void Commit();

        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Commiting bulk pending changes to the database
        /// </summary>
        void CommitBulkChanges();

        /// <summary>
        /// Commiting bulk pending changes to the database
        /// </summary>
        Task CommitBulkChangesAsync();

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        void Rollback();

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        Task RollbackAsync();

        /// <summary>
        /// Get IBaseRepository instance
        /// </summary>
        /// <param name="instance"></param>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IBaseRepository<TEntity> GetInstance<TEntity>(IBaseRepository<TEntity> instance) where TEntity : class;
    }
}