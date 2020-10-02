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

        public static IQueryable<TItem> GlobalFilter<TItem>(this IQueryable<TItem> query,
            DataTableRequest<TItem> request)
            where TItem : class
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
                    var whrString = WhereQueryBuilder<TItem>(column.Data);

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
            foreach (Column column in request.Columns.Where(x => x.Searchable && !string.IsNullOrEmpty(x.Search?.Value)))
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
                    query = query.Where(whereQuery, column.Search.Value.ToLower());
                }
            }

            return query;
        }

        private static string WhereQueryBuilder<TEntity>(string field, string value = "")
        {
            var propertyInfo = typeof(TEntity).GetProperty(field, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.String:
                        return $"{field}.ToLower().Contains(@0)";
                    case TypeCode.Boolean:
                        return $"{field} == @0";
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return $"{field}.ToString().Contains(@0)";
                    case TypeCode.DateTime:
                        if (DateTime.TryParse(value, out var _))
                        {
                            return $"{field}.ToShortDateString() == Convert.ToDateTime(@0).ToShortDateString()";
                        }

                        return "";

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