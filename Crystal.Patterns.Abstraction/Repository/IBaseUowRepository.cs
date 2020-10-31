using System;
using System.Threading.Tasks;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        /// Commiting pending changes to the database
        /// </summary>
        Task CommitAsync();

        /// <summary>
        /// Begin a new transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Rollback the transaction
        /// </summary>
        Task RollbackAsync();
    }
}