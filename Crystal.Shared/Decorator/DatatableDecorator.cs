using Crystal.Shared.Model;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Crystal.Shared
{
    public static class DatatableDecorator
    {
        public static Task<DataTableResponse<TEntity>> ToDatatableAsync<TEntity>(this IQueryable<TEntity> query, DataTableRequest<TEntity> request) where TEntity : class
        {
            return Task.FromResult(query.ToDatatable(request));
        }

        public static DataTableResponse<TEntity> ToDatatable<TEntity>(this IQueryable<TEntity> query, DataTableRequest<TEntity> request) where TEntity : class
        {
            var response = new DataTableResponse<TEntity>();
            if (request != null)
            {
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

                if (!request.Order.IsNullOrEmpty())
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
                response.TotalRecords = query.Count();
            }

            response.Echo = "sEcho";
            response.Data = query.ToArray();
            return response;
        }

        private static readonly string _orCondition = " || ";

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query, DataTableRequest<TEntity> request)
        {
            //***
            //*** Get the property name based on column name
            //***
            var propertyInfo = typeof(TEntity).GetProperty(request.Columns[request.Order[0].Column].Data,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            //***
            //*** Check if property exist in the entity
            //***
            if (propertyInfo != null)
            {
                //***
                //*** Column/property exist. Order the dataset
                //***
                query = query.OrderBy(request.Columns[request.Order[0].Column].Data + " " + request.Order[0].Dir);
            }

            return query;
        }

        public static IQueryable<TEntity> GlobalFilter<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            if (!string.IsNullOrEmpty(request?.Search?.Value))
            {
                string whereQuery = "";
                //***
                //*** Prepare the query
                //*** 
                foreach (Column column in request.Columns.Where(x => x.Searchable))
                {
                    //***
                    //*** Fetching where clause depending on different data types
                    //***
                    var whrString = _WhereQueryBuilder<TEntity>(column.Data, request.Search);

                    if (!string.IsNullOrEmpty(whereQuery) && !string.IsNullOrEmpty(whrString))
                    {
                        //***
                        //*** Adding OR clause in case of multiple Where conditions
                        //***
                        whereQuery += _orCondition;
                    }

                    if (!string.IsNullOrEmpty(whrString))
                    {
                        //***
                        //*** Appending to existing where query
                        //***
                        whereQuery += whrString;
                    }
                }

                if (!string.IsNullOrEmpty(whereQuery))
                {
                    query = query.Where(whereQuery, request.Search.Value.ToLower());
                }
            }

            return query;
        }

        public static IQueryable<TEntity> ColumnFilter<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            //***
            //*** Prepare the query
            //*** 
            foreach (Column column in request.Columns.Where(x => x.Searchable &&
                                                                 x.Search != null
                                                                 && !string.IsNullOrEmpty(x.Search?.Value)))
            {
                //***
                //*** Fetching where clause depending on different data types
                //***
                var whereQuery = _WhereQueryBuilder<TEntity>(column.Data, column.Search);

                if (!string.IsNullOrEmpty(whereQuery))
                {
                    //***
                    //*** Appending to existing where query
                    //***
                    query = query.Where(whereQuery, column.Search.Value);
                }
            }

            return query;

        }

        private static string _WhereQueryBuilder<TEntity>(string field, Search search)
        {
            var propertyInfo = _GetPropertyInfo(typeof(TEntity), field);

            if (propertyInfo != null)
            {
                var typeCode = propertyInfo.PropertyType;
                if (typeCode == typeof(string))
                {
                    if (search.ExactMatch)
                    {
                        return $"{field}.ToLower() == @0.ToLower()";
                    }
                    else
                    {
                        return $"{field}.ToLower().Contains(@0.ToLower())";
                    }
                }
                else if (typeCode == typeof(bool))
                {
                    return $"{field} == @0";
                }
                else if (typeCode == typeof(bool?))
                {
                    return $"{field}.HasValue && {field}.Value == @0";
                }
                else if (typeCode == typeof(int))
                {
                    return $"{field} == @0";
                }
                else if (typeCode == typeof(int?))
                {
                    return $"{field}.HasValue && {field} == @0";
                }
                else if (typeCode == typeof(DateTime))
                {
                    //***
                    //*** Check for range
                    //*** Eg. 09/01/2020 - 10/13/2020
                    if (search.Value.Contains(" - "))
                    {
                        var dates = search.Value.Split(" - ");
                        if (DateTime.TryParse(dates[0], out var startDate)
                            && DateTime.TryParse(dates[1], out var endDate))
                        {
                            return $"{field} >= Convert.ToDateTime(\"{startDate}\")" +
                                   $" && {field}.Date <= Convert.ToDateTime(\"{endDate}\").Date && !string.IsNullOrEmpty(@0)";
                        }
                    }
                    else if (DateTime.TryParse(search.Value, out var date))
                    {
                        return $"{field}.Date == \"{date.Date}\" && !string.IsNullOrEmpty(@0)";
                    }
                }
                else if (typeCode == typeof(DateTime?))
                {
                    //***
                    //*** Check for range
                    //*** Eg. 09/01/2020 - 10/13/2020
                    if (search.Value.Contains(" - "))
                    {
                        var dates = search.Value.Split(" - ");
                        if (DateTime.TryParse(dates[0], out var startDate)
                            && DateTime.TryParse(dates[1], out var endDate))
                        {
                            return $"{field}.HasValue && {field}.Value >= Convert.ToDateTime(\"{startDate}\")" +
                                   $" && {field}.Value.Date <= Convert.ToDateTime(\"{endDate}\").Date && !string.IsNullOrEmpty(@0)";
                        }
                    }
                    else if (DateTime.TryParse(search.Value, out var date))
                    {
                        return $"{field}.HasValue && {field}.Value.Date == \"{date.Date}\" && !string.IsNullOrEmpty(@0)";
                    }
                }
            }
            else
            {
                //***
                //*** Property not found 
                //***
            }

            return "";
        }

        /// <summary>
        /// Get properties from the Entity including nested properties
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static PropertyInfo _GetPropertyInfo(Type type, string propertyName)
        {
            //***
            //*** Check if the property name is a complex nested type
            //***
            if (propertyName.Contains("."))
            {
                //***
                //*** Get the first property name of the complex type
                //***
                var tempPropertyName = propertyName.Split(".", 2);
                //***
                //*** Check if the property exists in the type
                //***
                var prop = _GetPropertyInfo(type, tempPropertyName[0]);
                if (prop != null)
                {
                    //***
                    //*** Drill down to check if the nested property exists in the complex type
                    //***
                    return _GetPropertyInfo(prop.PropertyType, tempPropertyName[1]);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            }
        }
    }
}
