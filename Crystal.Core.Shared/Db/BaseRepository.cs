#region USING
using Crystal.Core.Shared.Db.Abstraction;
using Crystal.Core.Shared.Extension;
using Crystal.Core.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion

namespace Crystal.Core.Shared.Db
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;

        private DbContext _context { get; set; }

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);
            }

            return query;
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return (query.Any(filter));
        }

        public virtual DataTableResponse<TEntity> GetAll(DataTableRequest<TEntity> request)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            DataTableResponse<TEntity> response = new DataTableResponse<TEntity>();
            if (request != null)
            {
                //***
                //*** Filter data based on custom Query
                //***
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
                    query = query.Where(request);
                }

                //response.TotalRecords = query.Count();
                response.RecordsTotal = query.Count();

                if (!string.IsNullOrEmpty(request.IncludeProperties))
                {
                    foreach (var includeProperty in request.IncludeProperties.Split
                        (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(includeProperty);
                    }
                }

                if (request.OrderByQuery != null)
                {
                    query = request.OrderByQuery(query);
                }
                else if (!request.Order.IsNullOrEmpty())
                {
                    query = query.OrderBy(request.Columns[request.Order[0].Column].Data + " " + request.Order[0].Dir);
                }

                query = query.Skip(request.Start);

                if (request.Length != 0)
                {
                    query = query.Take(request.Length);
                }
            }
            else
            {
                response.RecordsTotal = query.Count();
            }

            response.Echo = "sEcho";
            response.TotalRecords = query.Count();
            response.TotalDisplayRecords = response.RecordsTotal;
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
            foreach (var entity in entities)
            {
                _dbSet.Add(entity);
            }
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            this.Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void DeleteAll()
        {
            var entities = _dbSet;
            foreach (TEntity entity in entities)
            {
                this.Delete(entity);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public virtual Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return Task.FromResult(this.GetAll(filter, includeProperties));
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(this.Any(filter));
        }

        public virtual Task<DataTableResponse<TEntity>> GetAllAsync(DataTableRequest<TEntity> request)
        {
            return Task.FromResult(this.GetAll(request));
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

        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await _dbSet.AddAsync(entity);
            }
        }

        public virtual async Task DeleteAsync(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            await DeleteAsync(entityToDelete);
        }

        public virtual Task DeleteAsync(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAllAsync()
        {
            var entities = _dbSet;
            foreach (TEntity entity in entities)
            {
                await this.DeleteAsync(entity);
            }
        }

        public virtual Task UpdateAsync(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
