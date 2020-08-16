using System;
using System.Threading.Tasks;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitBulkChangesAsync();
        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        /// <returns></returns>
        bool CommitBulkChanges();
        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        /// <returns></returns>
        bool Commit();

        /// <summary>
        ///     Commiting pending changes to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> CommitAsync();

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