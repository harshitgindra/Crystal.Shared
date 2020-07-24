using Crystal.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Crystal.Shared.Decorator
{
    public static class LinqDecorator
    {
        private static readonly string _orCondition = " || ";

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
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

        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            var queryString = "";
            IList<string> columnsToFilter = default;
            if (!request.SearchColumns.IsNullOrEmpty())
            {
                //***
                //*** Filter through all columns specified
                //***
                columnsToFilter = request.SearchColumns;
            }
            else
            {
                //***
                //*** Looping through all columns from Datatable Request
                //***
                columnsToFilter = request.Columns.Where(x => x.Searchable && !string.IsNullOrEmpty(x.Data))
                    .Select(x => x.Data)
                    .ToList();
            }

            //***
            //*** Prepare the query
            //*** 
            foreach (var columnName in columnsToFilter)
            {
                //***
                //*** Fetching where clause depending on different data types
                //***
                var whrString = WhereString<TEntity>(columnName);
                if (!string.IsNullOrEmpty(whrString))
                {
                    //***
                    //*** Appending to existing where query
                    //***
                    queryString += whrString;
                    //***
                    //*** Adding OR clause in case of multiple Where conditions
                    //***
                    queryString += _orCondition;
                }
            }

            if (!string.IsNullOrEmpty(queryString) && queryString.EndsWith(_orCondition))
            {
                queryString = queryString.Remove(queryString.Length - 4, 4);
            }

            if (!string.IsNullOrEmpty(queryString))
            {
                query = query.Where(queryString, request.Search.Value);
            }

            return query;
        }

        private static string WhereString<TEntity>(string field) where TEntity : class
        {
            var propertyInfo = typeof(TEntity).GetProperty(field,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.String:
                        return $"{field}.Contains(@0)";
                    case TypeCode.Boolean:
                        return $"{field} == @0";
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return $"{field}.ToString().Contains(@0)";

                    // todo: DateTime, float, double, decimals, and other types.
                }
            }
            else
            {
                //***
                //*** Property not found 
                //***
            }

            return string.Empty;
        }
    }
}