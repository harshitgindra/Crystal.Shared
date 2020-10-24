﻿#region USING

using Crystal.Patterns.Abstraction;
using Crystal.Shared.Decorator;
using Crystal.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper.QueryableExtensions;
using AutoMapper;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
        where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _context;
        private readonly IDbContextTransaction _transaction;
        private readonly MapperConfiguration _mapperConfiguration;
        private readonly IMapper _mapper;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public BaseRepository(DbContext context, MapperConfiguration mapperConfiguration)
        {
            _context = context;
            _mapperConfiguration = mapperConfiguration;
            _mapper = _mapperConfiguration?.CreateMapper();
            _dbSet = context.Set<TEntity>();
        }

        public BaseRepository(DbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _transaction = transaction;
        }

        public virtual TEntity[] GetAll(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            var query = _dbSet.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query,
                    (current, includeProperty) => current.Include(includeProperty));
            }

            return query.ToArray();
        }

        public virtual TModel[] GetAll<TModel>(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            if (_mapper != null)
            {
                var query = _dbSet.AsNoTracking();

                if (filter != null) query = query.Where(filter);

                if (!string.IsNullOrEmpty(includeProperties))
                {
                    query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query,
                        (current, includeProperty) => current.Include(includeProperty));
                }

                return query.ProjectTo<TModel>(_mapperConfiguration).ToArray();
            }
            else
            {
                throw new MapperNotConfiguredException();
            }
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.AsNoTracking().Any(filter);
        }

        public virtual DataTableResponse<TEntity> GetAll(DataTableRequest<TEntity> request)
        {
            var query = _dbSet.AsNoTracking();
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
            return response;
        }

        public virtual DataTableResponse<TModel> GetAll<TModel>(DataTableRequest<TEntity> request) where TModel : class
        {
            if (_mapper != null)
            {
                var query = _dbSet.AsNoTracking();
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
                return response;
            }
            else
            {
                throw new MapperNotConfiguredException();
            }
        }

        public virtual TModel Get<TModel>(object id)
        {
            if (_mapper != null)
            {
                return _mapper.Map<TModel>(_dbSet
               .Find(id));
            }
            else
            {
                throw new MapperNotConfiguredException();
            }
        }

        public virtual TEntity Get(object id)
        {
            return _dbSet
                .Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
        }

        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            _dbSet.AddRangeAsync(entities);
        }

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            this.Insert(entities);
            return Task.CompletedTask;
        }

        public virtual void BulkInsert(IEnumerable<TEntity> entities)
        {
            //***
            //*** Add multiple entities
            //***
            _dbSet.BulkInsert(entities);
        }

        public virtual void Delete(object id)
        {
            //***
            //*** find the entity and delete it
            //***
            Delete(_dbSet.Find(id));
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            //***
            //*** Remove the entity
            //***
            _context.Entry(entityToDelete).State = EntityState.Deleted;
        }

        public virtual void BulkDeleteAll()
        {
            //***
            //*** Bulk delete
            //***
            _dbSet.BulkDelete(_dbSet);
        }

        public virtual void DeleteAll()
        {
            //***
            //*** Delete all entities
            //***
            _dbSet.RemoveRange(_dbSet);
        }

        public virtual void BulkDelete()
        {
            //***
            //*** Bulk Delete all entities
            //***
            _dbSet.BulkDelete(_dbSet);
        }

        public virtual Task BulkDeleteAsync()
        {
            //***
            //*** Bulk Delete all entities
            //***
            this.BulkDelete();
            return Task.CompletedTask;
        }

        public virtual void BulkDelete(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Bulk Delete entities
            //***
            _dbSet.BulkDelete(_dbSet.Where(filter));
        }

        public virtual Task BulkDeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Bulk Delete entities
            //***
            this.BulkDelete(filter);
            return Task.CompletedTask;
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            //***
            //*** update the state of the entity
            //***
            if (_context.Entry(entityToUpdate).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToUpdate);
            }
            //***
            //*** Update the entity
            //***
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual Task<TModel[]> GetAllAsync<TModel>(Expression<Func<TEntity, bool>> filter = null,
           string includeProperties = "")
        {
            return Task.FromResult(GetAll<TModel>(filter, includeProperties));
        }

        public virtual Task<TEntity[]> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            return Task.FromResult(GetAll(filter, includeProperties));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(Any(filter));
        }

        public virtual Task<DataTableResponse<TModel>> GetAllAsync<TModel>(DataTableRequest<TEntity> request) where TModel : class
        {
            return Task.FromResult(GetAll<TModel>(request));
        }

        public virtual Task<DataTableResponse<TEntity>> GetAllAsync(DataTableRequest<TEntity> request)
        {
            return Task.FromResult(GetAll(request));
        }

        public virtual Task<TModel> GetAsync<TModel>(object id)
        {
            return Task.FromResult(this.Get<TModel>(id));
        }

        public virtual Task<TEntity> GetAsync(object id)
        {
            return Task.FromResult(this.Get(id));
        }

        public virtual Task InsertAsync(TEntity entity)
        {
            this.Insert(entity);
            return Task.CompletedTask;
        }

        public virtual Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            this.BulkInsert(entities);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(object id)
        {
            this.Delete(id);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(TEntity entityToDelete)
        {
            this.Delete(entityToDelete);
            return Task.CompletedTask;
        }

        public virtual Task DeleteAllAsync()
        {
            this.DeleteAll();
            return Task.CompletedTask;
        }

        public virtual Task UpdateAsync(TEntity entityToUpdate)
        {
            this.Update(entityToUpdate);
            return Task.CompletedTask;
        }

        public virtual void Update(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                this.Update(item);
            }
        }

        public virtual Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            this.Update(entities);
            return Task.CompletedTask;
        }

        public virtual void BulkUpdate(IEnumerable<TEntity> entities)
        {
            _dbSet.BulkUpdate(entities);
        }

        public virtual Task BulkUpdateAsync(IEnumerable<TEntity> entities)
        {
            this.BulkUpdate(entities);
            return Task.CompletedTask;
        }

        public Task<TModel> GetAsync<TModel>(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return Task.FromResult(this.Get<TModel>(filter, includeProperties));
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return Task.FromResult(this.Get(filter, includeProperties));
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return this.GetAll(filter, includeProperties)
                .FirstOrDefault();
        }

        public TModel Get<TModel>(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return this.GetAll<TModel>(filter, includeProperties)
                .FirstOrDefault();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> filter)
        {
            //***
            //*** Remove/Delete the entities
            //***
            _dbSet.RemoveRange(_dbSet.Where(filter));
        }

        public virtual Task DeleteAsync(Expression<Func<TEntity, bool>> filter)
        {
            this.Delete(filter);
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
    }
}