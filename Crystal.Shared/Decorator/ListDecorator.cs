#region USING

using Crystal.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Crystal.Shared.Decorator
{
    public static class ListDecorator
    {
        public static bool IsNullOrEmpty<TItem>(this IEnumerable<TItem> dataset)
        {
            var returnValue = dataset == null || !dataset.Any();
            return returnValue;
        }

        public static T PickRandom<T>(this IEnumerable<T> source)
        {
            return source.PickRandom(1).Single();
        }

        public static IEnumerable<T> PickRandom<T>(this IEnumerable<T> source, int count)
        {
            return source.Shuffle().Take(count);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => Guid.NewGuid());
        }

        public static DataTableResponse<TEntity> ToDatatable<TEntity>(this IQueryable<TEntity> query, DataTableRequest<TEntity> request) where TEntity : class
        {
            var response = new DataTableResponse<TEntity>();
            if (request != null)
            {
                if (request.SearchQuery != null)
                {
                    query = query.Where(request.SearchQuery);
                }
                //***
                //*** Filter based on search content and search columns
                //***
                if (request.Search != null && !string.IsNullOrEmpty(request.Search.Value))
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
    }
}