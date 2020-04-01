using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Crystal.Core.Shared.Abstraction
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(int id);

        Task<bool> InsertAsync(TEntity item);

        Task<bool> InsertAsync(IEnumerable<TEntity> item);
    }
}
