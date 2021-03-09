using MicroOrm.Dapper.Repositories;
using MicroOrm.Dapper.Repositories.DbContext;
using System;
using System.Collections.Generic;
using System.Data;

namespace Crystal.Dapper
{
    public class BaseUowRepository : DapperDbContext, IBaseUowRepository
    {
        private readonly IDictionary<Type, object> _repositoryInstances;

        /// <summary>
        /// Base unit of work constructor
        /// </summary>
        /// <param name="connection"></param>
        public BaseUowRepository(IDbConnection connection)
            : base(connection)
        {
            _repositoryInstances = new Dictionary<Type, object>();
        }

        /// <summary>
        /// Returns IBaseRepository instance of the entity
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public virtual IDapperRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            //***
            //*** Check if the instance is already created
            //***
            if (_repositoryInstances.ContainsKey(typeof(TEntity)))
            {
                //***
                //*** Instance already created, return the instance from the dictionary
                //***
                return (IDapperRepository<TEntity>)_repositoryInstances[typeof(TEntity)];
            }
            else
            {
                //***
                //*** Create a new instance of base repository 
                //*** Save it to the dictionary
                //*** Return the instance
                //***
                var repo = new DapperRepository<TEntity>(this.Connection);
                _repositoryInstances.Add(typeof(TEntity), repo);
                return repo;
            }
        }

        public new void Dispose()
        {
            //***
            //*** Dispose all base repository instances
            //***
            foreach (var item in _repositoryInstances)
            {
                //item.Value.TryDispose();
            }
            //***
            //*** Clear all repository instances
            //***
            _repositoryInstances.Clear();
            //***
            //*** Dispose dB context
            //***
            this.Connection?.Close();
            this.Connection?.Dispose();
            base.Dispose();
        }
    }
}
