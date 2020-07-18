#region USING

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

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly DbContext _context;
        private readonly IDbContextTransaction _transaction;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public BaseRepository(DbContext context, IDbContextTransaction transaction)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
            _transaction = transaction;
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            var query = _dbSet.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                query = includeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query,
                    (current, includeProperty) => current.Include(includeProperty));
            }

            return query;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            var query = _dbSet.AsNoTracking();
            return query.Any(filter);
        }

        public virtual DataTableResponse<TEntity> GetAll(DataTableRequest<TEntity> request)
        {
            var query = _dbSet.AsNoTracking();
            var response = new DataTableResponse<TEntity>();
            if (request != null)
            {
                if (!string.IsNullOrEmpty(request.IncludeProperties))
                {
                    query = request.IncludeProperties.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                        .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
                }

                if(request.SearchQuery != null)
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
                    query = query.Where(request);
                }

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
            else
            {
                response.TotalDisplayRecords = query.Count();
            }

            response.Echo = "sEcho";
            response.TotalDisplayRecords = query.Count();
            response.Data = query.ToList();
            return response;
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
            _context.BulkInsert(entities);
        }

        public virtual void Delete(object id)
        {
            var entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached) _dbSet.Attach(entityToDelete);

            _dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteAll()
        {
            _context.BulkDelete(_dbSet);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "")
        {
            return Task.FromResult(GetAll(filter, includeProperties));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(Any(filter));
        }

        public virtual Task<DataTableResponse<TEntity>> GetAllAsync(DataTableRequest<TEntity> request)
        {
            return Task.FromResult(GetAll(request));
        }

        public virtual async Task<TEntity> GetAsync(object id)
        {
            return await _dbSet
                .FindAsync(id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual Task InsertAsync(IEnumerable<TEntity> entities)
        {
            this.Insert(entities);
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

        public Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return Task.FromResult(this.Find(filter, includeProperties));
        }

        public TEntity Find(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            return this.GetAll(filter, includeProperties).FirstOrDefault();
        }
    }
}