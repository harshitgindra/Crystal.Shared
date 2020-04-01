using Crystal.Core.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Crystal.Core.Shared.Db.Abstraction
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task Delete(TEntity entityToDelete);
        Task Delete(object id);
        Task DeleteAll();
        Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        Task<DataTableResponse<TEntity>> GetAll(DataTableRequest<TEntity> request);
        //Task<DtResponse<TEntity>> GetAll(DtRequest request);
        Task<TEntity> Get(object id);
        Task Insert(TEntity entity);
        Task Insert(IEnumerable<TEntity> entities);
        Task Update(TEntity entityToUpdate);
        Task<bool> Any(Expression<Func<TEntity, bool>> filter = null);
    }
}
