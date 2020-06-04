using System;
using System.Threading.Tasks;

namespace Crystal.Abstraction.Repository
{
    public interface IBaseUowRepository : IDisposable
    {
        // /// <summary>
        // /// DbContext Instance
        // /// </summary>
        // DbContext DbContext { get; set; }
        /// <summary>
        ///     Comming pending changes to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> Commit();
    }
}