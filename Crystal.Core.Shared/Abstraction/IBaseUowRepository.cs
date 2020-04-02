using Crystal.Core.Shared.Db;
using System;
using System.Threading.Tasks;

namespace Crystal.Core.Shared.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        /// <summary>
        /// DbContext Instance
        /// </summary>
        BaseContext DbContext { get; set; }
        /// <summary>
        /// Comming pending changes to the database
        /// </summary>
        /// <returns></returns>
        Task<bool> Commit();
    }
}
