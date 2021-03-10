using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Threading.Tasks;

namespace Crystal.Shared
{
    /// <summary>
    /// Datatable decorator with extension methods
    /// </summary>
    public static class DatatableDecorator
    {
        /// <summary>
        /// Performs datatable operations on the result based on input
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Collection of data</param>
        /// <param name="request">Datatable request</param>
        /// <returns>Datatable response</returns>
        public static Task<DataTableResponse<TEntity>> ToDatatableAsync<TEntity>(this IQueryable<TEntity> query, DataTableRequest<TEntity> request) where TEntity : class
        {
            return Task.FromResult(query.ToDatatable(request));
        }

        /// <summary>
        /// Performs datatable operations on the result based on input
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query">Collection of data</param>
        /// <param name="request">Datatable request</param>
        /// <returns>Datatable response</returns>
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
        /// <summary>
        /// Perform order by operation with datatable request
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Perform global filter on all searchable columns
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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
                foreach (Column column in request.Columns.Where(x => x.Searchable && !x.Data.IsNullEmptyWhiteSpace()))
                {
                    //***
                    //*** Fetching where clause depending on different data types
                    //***
                    var whrString = WhereQueryBuilder<TEntity>(column.Data, request.Search);

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

        /// <summary>
        /// Perform column level filtering based on datatable request
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static IQueryable<TEntity> ColumnFilter<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            //***
            //*** Prepare the query
            //*** 
            foreach (Column column in request.Columns.Where(x => x.Searchable &&
                                                                 x.Search != null
                                                                 && !x.Data.IsNullEmptyWhiteSpace()
                                                                 && !string.IsNullOrEmpty(x.Search?.Value)))
            {
                //***
                //*** Fetching where clause depending on different data types
                //***
                var whereQuery = WhereQueryBuilder<TEntity>(column.Data, column.Search);

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

        private static string WhereQueryBuilder<TEntity>(string field, Search search)
        {
            var propertyInfo = GetPropertyInfo(typeof(TEntity), field);

            if (propertyInfo != null)
            {
                if (!propertyInfo.CanWrite)
                {
                    //***
                    //*** Property not found 
                    //***
                    Console.WriteLine($"Property {field} is not a writable field in entity {nameof(TEntity)}");
                }
                else if (!propertyInfo.CanRead)
                {
                    //***
                    //*** Property not found 
                    //***
                    Console.WriteLine($"Property {field} is not a readable field in entity {nameof(TEntity)}");
                }
                else
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
                        if (bool.TryParse(search.Value, out _))
                        {
                            return $"{field} == @0";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(bool?))
                    {
                        if (bool.TryParse(search.Value, out _))
                        {
                            return $"{field}.HasValue && {field}.Value == @0";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(int))
                    {
                        if (int.TryParse(search.Value, out int val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field} == @0";
                            }
                            else
                            {
                                return $"{field}.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(int?))
                    {
                        if (int.TryParse(search.Value, out int val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field}.HasValue && {field} == @0";
                            }
                            else
                            {
                                return $"{field}.HasValue && {field}.Value.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(long))
                    {
                        if (long.TryParse(search.Value, out long val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field} == @0";
                            }
                            else
                            {
                                return $"{field}.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(long?))
                    {
                        if (long.TryParse(search.Value, out long val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field}.HasValue && {field} == @0";
                            }
                            else
                            {
                                return $"{field}.HasValue && {field}.Value.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(short))
                    {
                        if (short.TryParse(search.Value, out short val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field} == @0";
                            }
                            else
                            {
                                return $"{field}.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else if (typeCode == typeof(short?))
                    {
                        if (short.TryParse(search.Value, out short val))
                        {
                            if (search.ExactMatch)
                            {
                                return $"{field}.HasValue && {field} == @0";
                            }
                            else
                            {
                                return $"{field}.HasValue && {field}.Value.ToString().Contains(@0)";
                            }
                        }
                        else
                        {
                            return "";
                        }
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
            }
            else
            {
                //***
                //*** Property not found 
                //***
                Console.WriteLine($"Property {field} not found in entity {nameof(TEntity)}");
            }

            return "";
        }

        /// <summary>
        /// Get properties from the Entity including nested properties
        /// </summary>
        /// <param name="type"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private static PropertyInfo GetPropertyInfo(Type type, string propertyName)
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
                var prop = GetPropertyInfo(type, tempPropertyName[0]);
                if (prop != null)
                {
                    //***
                    //*** Drill down to check if the nested property exists in the complex type
                    //***
                    return GetPropertyInfo(prop.PropertyType, tempPropertyName[1]);
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
