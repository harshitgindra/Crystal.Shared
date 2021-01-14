#region USING

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crystal.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly DbContext _context;
        private readonly MapperConfiguration _mapperConfiguration;

        public virtual DbSet<TEntity> Entity { get; }

        public BaseRepository(DbContext context)
        {
            _context = context;
            Entity = context.Set<TEntity>();
        }

        public BaseRepository(DbContext context, MapperConfiguration mapperConfiguration)
        {
            _context = context;
            _mapperConfiguration = mapperConfiguration;
            Entity = context.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entity.AsNoTracking();

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return Task.FromResult(query.ToList());
        }

        public virtual Task<List<TModel>> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                IQueryable<TEntity> query = Entity.AsNoTracking();

                if (includes != null)
                {
                    foreach (Expression<Func<TEntity, object>> include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }


                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                return Task.FromResult(query.ProjectTo<TModel>(_mapperConfiguration).ToList());
            }
        }

        public virtual Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entity.AsNoTracking();

            if (includes != null)
            {
                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return Task.FromResult(query);
        }

        public virtual Task<IQueryable<TModel>> QueryAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                IQueryable<TEntity> query = Entity.AsNoTracking();

                if (includes != null)
                {
                    foreach (Expression<Func<TEntity, object>> include in includes)
                    {
                        query = query.Include(include);
                    }
                }

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                if (orderBy != null)
                {
                    query = orderBy(query);
                }

                Task.FromResult(query.ProjectTo<TModel>(_mapperConfiguration));
            }

            return null;
        }

        public virtual Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = Entity.AsNoTracking();

            foreach (Expression<Func<TEntity, object>> include in includes)
            {
                query = query.Include(include);
            }

            return Task.FromResult(query.FirstOrDefault(filter));
        }

        public virtual Task<TModel> GetFirstOrDefaultAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                IQueryable<TEntity> query = Entity.AsNoTracking();

                foreach (Expression<Func<TEntity, object>> include in includes)
                {
                    query = query.Include(include);
                }

                return Task.FromResult(_mapperConfiguration
                    .CreateMapper()
                    .Map<TModel>(query.FirstOrDefault(filter)));
            }
        }


        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return Task.FromResult(Entity.AsNoTracking().Any());
            }
            else
            {
                return Task.FromResult(Entity.AsNoTracking().Any(filter));
            }
        }

        public virtual Task<TEntity> FindAsync(object id)
        {
            return Task.FromResult(Entity.Find(id));
        }

        public virtual Task<TModel> FindAsync<TModel>(object id)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                return Task.FromResult(_mapperConfiguration.CreateMapper().Map<TModel>(Entity.Find(id)));
            }
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            Entity.Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            Entity.AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual void BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            Entity.BulkInsert(entities);
        }

        public virtual async Task DeleteAsync(object id)
        {
            //***
            //*** find the entity and delete it
            //***
            await this.DeleteAsync(Entity.Find(id));
        }

        public virtual Task DeleteAsync(TEntity entityToDelete)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                Entity.Attach(entityToDelete);
            }

            //***
            //*** Remove the entity
            //***
            _context.Entry(entityToDelete).State = EntityState.Deleted;
            return Task.CompletedTask;
        }

        public virtual void BulkDeleteAllAsync()
        {
            //***
            //*** Bulk delete
            //***
            Entity.BulkDelete(Entity);
        }

        public virtual Task DeleteAllAsync()
        {
            //***
            //*** Delete all entities
            //***
            Entity.RemoveRange(Entity);
            return Task.CompletedTask;
        }

        public virtual void BulkDeleteAsync()
        {
            //***
            //*** Bulk Delete all entities
            //***
            Entity.BulkDelete(Entity);
        }

        public virtual void BulkDeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Bulk Delete entities
            //***
            Entity.BulkDelete(Entity.Where(filter));
        }

        public virtual Task UpdateAsync(TEntity entityToUpdate)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                Entity.Attach(entityToUpdate);
            }

            //***
            //*** Update the entity
            //***
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Remove/Delete the entities
            //***
            Entity.RemoveRange(Entity.Where(filter));
            return Task.CompletedTask;
        }

        public virtual void Dispose()
        {
            //***
            //*** Dispose the context and transaction
            //***
            _context?.Dispose();
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                await this.UpdateAsync(item);
            }
        }

        public virtual Task<IQueryable<TEntity>> RunSql(string sql, params object[] paramters)
        {
            //***
            //*** Execute raw sql query with parameters against the entity
            //***
            return Task.FromResult(this.Entity.FromSqlRaw(sql, paramters));
        }
    }
}