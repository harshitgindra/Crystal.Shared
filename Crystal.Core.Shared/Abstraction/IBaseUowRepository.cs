using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Crystal.Core.Shared.Db;

namespace Crystal.Core.Shared.Abstraction
{
    public interface IBaseUowRepository : IDisposable
    {
        BaseContext DbContext { get;set;}
        
        Task<bool> Commit();
    }
}
