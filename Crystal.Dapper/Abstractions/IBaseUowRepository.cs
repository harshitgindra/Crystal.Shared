using MicroOrm.Dapper.Repositories;

namespace Crystal.Dapper
{
    public interface IBaseUowRepository
    {
        /// <summary>
        /// Returns IBaseRepository instance of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class;
    }
}