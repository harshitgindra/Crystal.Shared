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

        void Rollback();
    }
}