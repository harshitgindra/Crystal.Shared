using System;
using System.Threading.Tasks;

namespace Crystal.Patterns.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        ///     Comming pending changes to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> Commit();
    }
}