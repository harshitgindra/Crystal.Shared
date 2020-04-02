using Crystal.Core.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace Crystal.Core.Shared.Extension
{
    public static class LinqExtensions
    {
        public static IQueryable<TEntity> Where<TEntity>(this IQueryable<TEntity> query,
            DataTableRequest<TEntity> request)
            where TEntity : class
        {
            string queryString = "";
            IList<string> columnsToSort = new List<string>();
            if (!request.SearchColumns.IsNullOrEmpty())
            {
                //***
                //*** Filter through all columns specified
                //***
                columnsToSort = request.SearchColumns;
            }
            else
            {
                //***
                //*** Looping through all columns from Datatable Request
                //***
                columnsToSort = request.Columns.Where(x => x.Searchable
                                && !string.IsNullOrEmpty(x.Data))
                                .Select(x => x.Data)
                                .ToList();
            }
            //***
            //*** Prepare the query
            //*** 
            foreach (var columnName in columnsToSort)
            {
                if (!string.IsNullOrEmpty(queryString))
                {
                    //***
                    //*** Adding OR clause in case of multiple Where conditions
                    //***
                    queryString += " || ";
                }
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
                }
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                query = query.Where(queryString, request.Search.Value);
            }
            return query;
        }

        private static string WhereString<TEntity>(string field) where TEntity : class
        {
            PropertyInfo propertyInfo = typeof(TEntity).GetProperty(field);
            if (propertyInfo != null)
            {
                var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
                switch (typeCode)
                {
                    case TypeCode.String:
                        return string.Format("{0}.Contains(@0)", field);
                    case TypeCode.Boolean:
                        return string.Format("{0} == @0", field);
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        return String.Format("{0}.ToString().Contains(@0)", field);

                    // todo: DateTime, float, double, decimals, and other types.
                    default:
                        break;
                }
            }
            else
            {
                throw new NotSupportedException(String.Format("Property '{0}' not found.", propertyInfo.Name));
            }
            return string.Empty;
        }
    }
}
