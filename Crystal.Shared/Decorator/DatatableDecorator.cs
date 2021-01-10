﻿using Crystal.Shared.Model;
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
                foreach (Column column in request.Columns.Where(x => x.Searchable && !string.IsNullOrEmpty(x.Data)))
                {
                    //***
                    //*** Fetching where clause depending on different data types
                    //***
                    var whrString = WhereQueryBuilder<TEntity>(column.Data, request.Search.Value);

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
            if (request != null && !request.Columns.IsNullOrEmpty())
            {
                //***
                //*** Prepare the query
                //*** 
                foreach (Column column in request.Columns.Where(x => x.Searchable && !string.IsNullOrEmpty(x.Search?.Value) && !string.IsNullOrEmpty(x.Data)))
                {
                    //***
                    //*** Fetching where clause depending on different data types
                    //***
                    var whereQuery = WhereQueryBuilder<TEntity>(column.Data, column.Search.Value);

                    if (!string.IsNullOrEmpty(whereQuery))
                    {
                        //***
                        //*** Appending to existing where query
                        //***
                        query = query.Where(whereQuery, column.Search.Value);
                    }
                }
            }

            return query;

        }

        private static string WhereQueryBuilder<TEntity>(string field, string value = "")
        {
            var propertyInfo = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var typeCode = propertyInfo.PropertyType;
                if (typeCode == typeof(string))
                {
                    return $"{field}.ToLower().Contains(@0)";
                }
                else if (typeCode == typeof(bool))
                {
                    if (bool.TryParse(value, out bool val))
                    {
                        return $"{field} == @0";
                    }
                }
                else if (typeCode == typeof(bool?))
                {
                    if (bool.TryParse(value, out bool val))
                    {
                        return $"{field}.HasValue && {field}.Value == @0";
                    }
                }
                else if (typeCode == typeof(int))
                {
                    return $"{field}.ToString().Contains(@0)";
                }
                else if (typeCode == typeof(int?))
                {
                    return $"{field}.HasValue && {field}.Value.ToString().Contains(@0)";
                }
                else if (typeCode == typeof(DateTime))
                {
                    //***
                    //*** Check for range
                    //*** Eg. 09/01/2020 - 10/13/2020
                    if (value.Contains(" - "))
                    {
                        var dates = value.Split(" - ");
                        if (DateTime.TryParse(dates[0], out var startDate)
                            && DateTime.TryParse(dates[1], out var endDate))
                        {
                            return $"{field} >= Convert.ToDateTime(\"{startDate}\")" +
                                   $" && {field}.Date <= Convert.ToDateTime(\"{endDate}\").Date && !string.IsNullOrEmpty(@0)";
                        }
                    }
                    else if (DateTime.TryParse(value, out var date))
                    {
                        return $"{field}.Date == \"{date.Date}\" && !string.IsNullOrEmpty(@0)";
                    }
                }
                else if (typeCode == typeof(DateTime?))
                {
                    //***
                    //*** Check for range
                    //*** Eg. 09/01/2020 - 10/13/2020
                    if (value.Contains(" - "))
                    {
                        var dates = value.Split(" - ");
                        if (DateTime.TryParse(dates[0], out var startDate)
                            && DateTime.TryParse(dates[1], out var endDate))
                        {
                            return $"{field}.HasValue && {field}.Value >= Convert.ToDateTime(\"{startDate}\")" +
                                   $" && {field}.Value.Date <= Convert.ToDateTime(\"{endDate}\").Date && !string.IsNullOrEmpty(@0)";
                        }
                    }
                    else if (DateTime.TryParse(value, out var date))
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
    }
}