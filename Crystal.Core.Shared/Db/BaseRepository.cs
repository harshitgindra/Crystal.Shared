#region USING
using Crystal.Core.Shared.Db.Abstraction;
using Crystal.Core.Shared.Extension;
using Crystal.Core.Shared.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
#endregion

namespace Crystal.Core.Shared.Db
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {
        private DbSet<TEntity> _dbSet;

        private DbContext _context { get; set; }

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual Task<IEnumerable<TEntity>> GetAll(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

            if (orderBy != null) query = orderBy(query);

            return Task.FromResult(query.AsEnumerable());
        }

        public virtual Task<bool> Any(Expression<Func<TEntity, bool>> filter)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return Task.FromResult(query.Any(filter));
        }

        public virtual Task<DataTableResponse<TEntity>> GetAll(DataTableRequest<TEntity> request)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
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
                else if (request.Search != null && request.SearchColumns != null & !string.IsNullOrEmpty(request.Search.Value))
                {
                    query = MultipleWhere(query, request.Search.Value, request.SearchColumns);
                }

                request.TotalRecords = query.Count();

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
                    //query = query.OrderBy(request.Columns[request.Order[0].Column].Data + " " + request.Order[0].Dir);
                }

                query = query.Skip(request.Skip);

                if (request.Length != 0)
                {
                    query = query.Take(request.Length);
                }
            }
            else
            {
                request = new DataTableRequest<TEntity> { TotalRecords = query.Count() };
            }

            DataTableResponse<TEntity> response = new DataTableResponse<TEntity>
            {
                RecordsTotal = request.TotalRecords,
                Echo = "sEcho",
                TotalRecords = query.Count(),
                TotalDisplayRecords = request.TotalRecords,
                Data = query.ToList(),
            };
            return Task.FromResult(response);
        }

        public virtual async Task<TEntity> Get(object id)
        {
            return await _dbSet
                .FindAsync(id);
        }

        public virtual async Task Insert(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual async Task Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                await _dbSet.AddAsync(entity);
            }
        }

        public virtual async Task Delete(object id)
        {
            TEntity entityToDelete = await _dbSet.FindAsync(id);
            await Delete(entityToDelete);
        }

        public virtual Task Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
            return Task.CompletedTask;
        }

        public virtual async Task DeleteAll()
        {
            var entities = _dbSet;
            foreach (TEntity entity in entities)
            {
                await this.Delete(entity);
            }
        }

        public virtual Task Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public static IQueryable<TEntity> MultipleWhere(IEnumerable<TEntity> source, string propertyValue, List<Func<TEntity, string>> columnNames)
        {
            return source.Where(m =>
            {
                bool? result = false;
                foreach (var columnName in columnNames)
                {
                    try
                    {
                        result = columnName(m)?.ToLower().Contains(propertyValue);
                        if (result.HasValue && result.Value)
                        {
                            break;
                        }
                    }
                    catch
                    {
                    }
                }
                return result.Value;
            }).AsQueryable();
        }
    }
}
