#region USING

using AutoMapper;
using AutoMapper.QueryableExtensions;
using Crystal.Patterns.Abstraction;
using Crystal.Shared.Decorator;
using Crystal.Shared.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        public DbSet<TEntity> DbSet { get; }
        private readonly DbContext _context;
        private readonly IDbContextTransaction _transaction;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        public BaseRepository(DbContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public BaseRepository(DbContext context, MapperConfiguration mapperConfiguration)
        {
            _context = context;
            _mapperConfiguration = mapperConfiguration;
            _mapper = _mapperConfiguration?.CreateMapper();
            DbSet = context.Set<TEntity>();
        }

        public virtual Task<List<TEntity>> GetAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DbSet.AsNoTracking();

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

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
                IQueryable<TEntity> query = DbSet.AsNoTracking();

                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);

                if (filter != null)
                    query = query.Where(filter);

                if (orderBy != null)
                    query = orderBy(query);

                return Task.FromResult(query.ProjectTo<TModel>(_mapperConfiguration).ToList());
            }
        }

        public virtual Task<IQueryable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = DbSet.AsNoTracking();

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
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                IQueryable<TEntity> query = DbSet.AsNoTracking();

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
            IQueryable<TEntity> query = DbSet.AsNoTracking();

            foreach (Expression<Func<TEntity, object>> include in includes)
                query = query.Include(include);

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
                IQueryable<TEntity> query = DbSet.AsNoTracking();

                foreach (Expression<Func<TEntity, object>> include in includes)
                    query = query.Include(include);

                return Task.FromResult(_mapperConfiguration
                    .CreateMapper()
                    .Map<TModel>(query.FirstOrDefault(filter)));
            }
        }


        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter == null)
            {
                return Task.FromResult(DbSet.AsNoTracking().Any());
            }
            else
            {
                return Task.FromResult(DbSet.AsNoTracking().Any(filter));
            }
        }

        public virtual Task<DataTableResponse<TEntity>> GetAsync(DataTableRequest<TEntity> request)
        {
            var query = DbSet.AsNoTracking();
            var response = new DataTableResponse<TEntity>();
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.IncludeProperties))
                {
                    query = request.IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }

                if (request.SearchQuery != null)
                {
                    query = query.Where(request.SearchQuery);
                }
                //***
                //*** Filter based on search content and search columns
                //***
                else if (request.Search != null && !string.IsNullOrEmpty(request.Search.Value))
                {
                    //***
                    //*** Custom where clause to filter data
                    //***
                    query = query.GlobalFilter(request);
                }

                query = query.ColumnFilter(request);

                response.TotalRecords = query.Count();

                if (request.OrderByQuery != null)
                {
                    query = request.OrderByQuery(query);
                    //***
                    //*** Skip the records from the filtered dataset
                    //***
                    query = query.Skip(request.Start);
                }
                else if (!request.Order.IsNullOrEmpty())
                {
                    query = query.OrderBy(request);
                    //***
                    //*** Skip the records from the filtered dataset
                    //***
                    query = query.Skip(request.Start);
                }

                //***
                //*** If length is -1, return all records
                //***
                if (request.Length != -1)
                {
                    //***
                    //*** Take selected count of records from the filtered dataset
                    //***
                    query = query.Take(request.Length);
                }
            }

            response.Echo = "sEcho";
            response.RecordsFiltered = query.Count();
            response.Data = query.ToArray();
            return Task.FromResult(response);
        }

        public virtual Task<DataTableResponse<TModel>> GetAsync<TModel>(DataTableRequest<TEntity> request) where TModel : class
        {
            if (_mapper != null)
            {
                var query = DbSet.AsNoTracking();
                var response = new DataTableResponse<TModel>();
                if (request != null)
                {
                    if (!string.IsNullOrEmpty(request.IncludeProperties))
                    {
                        query = request.IncludeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                    }

                    if (request.SearchQuery != null)
                    {
                        query = query.Where(request.SearchQuery);
                    }
                    //***
                    //*** Filter based on search content and search columns
                    //***
                    else if (request.Search != null && !string.IsNullOrEmpty(request.Search.Value))
                    {
                        //***
                        //*** Custom where clause to filter data
                        //***
                        query = query.GlobalFilter(request);
                    }

                    query = query.ColumnFilter(request);

                    response.TotalRecords = query.Count();

                    if (request.OrderByQuery != null)
                    {
                        query = request.OrderByQuery(query);
                        //***
                        //*** Skip the records from the filtered dataset
                        //***
                        query = query.Skip(request.Start);
                    }
                    else if (!request.Order.IsNullOrEmpty())
                    {
                        query = query.OrderBy(request);
                        //***
                        //*** Skip the records from the filtered dataset
                        //***
                        query = query.Skip(request.Start);
                    }

                    //***
                    //*** If length is -1, return all records
                    //***
                    if (request.Length != -1)
                    {
                        //***
                        //*** Take selected count of records from the filtered dataset
                        //***
                        query = query.Take(request.Length);
                    }
                }

                response.Echo = "sEcho";
                response.RecordsFiltered = query.Count();
                response.Data = query.ProjectTo<TModel>(_mapperConfiguration).ToArray();
                return Task.FromResult(response);
            }
            else
            {
                throw new MapperNotConfiguredException();
            }
        }

        public virtual Task<TEntity> FindAsync(object id)
        {
            return Task.FromResult(DbSet.Find(id));
        }

        public virtual Task<TModel> FindAsync<TModel>(object id)
        {
            if (_mapperConfiguration == null)
            {
                throw new Exception("Automapper not configured");
            }
            else
            {
                return Task.FromResult(_mapperConfiguration.CreateMapper().Map<TModel>(DbSet.Find(id)));
            }
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            return Task.CompletedTask;
        }

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            DbSet.AddRange(entities);
            return Task.CompletedTask;
        }

        public virtual void BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            DbSet.BulkInsert(entities);
        }

        public virtual async Task DeleteAsync(object id)
        {
            //***
            //*** find the entity and delete it
            //***
            await this.DeleteAsync(DbSet.Find(id));
        }

        public virtual Task DeleteAsync(TEntity entityToDelete)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
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
            DbSet.BulkDelete(DbSet);
        }

        public virtual Task DeleteAllAsync()
        {
            //***
            //*** Delete all entities
            //***
            DbSet.RemoveRange(DbSet);
            return Task.CompletedTask;
        }

        public virtual void BulkDeleteAsync()
        {
            //***
            //*** Bulk Delete all entities
            //***
            DbSet.BulkDelete(DbSet);
        }

        public virtual void BulkDeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Bulk Delete entities
            //***
            DbSet.BulkDelete(DbSet.Where(filter));
        }

        public virtual Task UpdateAsync(TEntity entityToUpdate)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                DbSet.Attach(entityToUpdate);
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
            DbSet.RemoveRange(DbSet.Where(filter));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //***
            //*** Dispose the context and transaction
            //***
            _context?.Dispose();
            _transaction?.Dispose();
        }

        public virtual async Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                await this.UpdateAsync(item);
            }
        }
    }
}