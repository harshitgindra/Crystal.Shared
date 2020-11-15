using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
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
    }
}