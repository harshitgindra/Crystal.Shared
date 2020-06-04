using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Crystal.Shared.Model;

namespace Crystal.Shared.Decorator
{
    public static class LinqDecorator
    {
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            var queryString = "";
            IList<string> columnsToSort = new List<string>();
            if (!request.SearchColumns.IsNullOrEmpty())
                //***
                //*** Filter through all columns specified
                //***
                columnsToSort = request.SearchColumns;
            else
                //***
                //*** Looping through all columns from Datatable Request
                //***
                columnsToSort = request.Columns.Where(x => x.Searchable
                                                           && !string.IsNullOrEmpty(x.Data))
                    .Select(x => x.Data)
                    .ToList();

            //***
            //*** Prepare the query
            //*** 
            foreach (var columnName in columnsToSort)
            {
                if (!string.IsNullOrEmpty(queryString))
                    //***
                    //*** Adding OR clause in case of multiple Where conditions
                    //***
                    queryString += " || ";

                //***
                //*** Fetching where clause depending on different data types
                //***
                var whrString = WhereString<TEntity>(columnName);
                if (!string.IsNullOrEmpty(whrString))
                    //***
                    //*** Appending to existing where query
                    //***
                    queryString += whrString;
            }

            if (!string.IsNullOrEmpty(queryString)) query = query.Where(queryString, request.Search.Value);

            return query;
        }

        private static string WhereString<TEntity>(string field) where TEntity : class
        {
            var propertyInfo = typeof(TEntity).GetProperty(field);
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
                throw new NotSupportedException($"Property '{propertyInfo.Name}' not found.");
            }

            return string.Empty;
        }
    }
}