#region USING

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Crystal.Shared.Model
{
    public class DataTableRequest<TEntity>
    {
        public List<string> SearchColumns { get; set; }

        /// <summary>
        ///     Paging first record indicator. This is the start point in the current data set
        ///     (0 index based - i.e. 0 is the first record)
        /// </summary>
        public int Start { get; set; }

        /// <summary>
        ///     Number of records that the table can display in the current draw. It is expected
        ///     that the number of records returned will be equal to this number, unless the
        ///     server has fewer records to return. Note that this can be -1 to indicate that
        ///     all records should be returned (although that negates any benefits of
        ///     server-side processing!)
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        ///     Global Search for the table
        /// </summary>
        public Search Search { get; set; }

        /// <summary>
        ///     Collection of all column indexes and their sort directions
        /// </summary>
        public List<Order> Order { get; set; }

        /// <summary>
        ///     Collection of all columns in the table
        /// </summary>
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        /// <summary>
        ///     Column's data source
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        ///     Column's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Flag to indicate if this column is searchable (true) or not (false)
        /// </summary>
        public bool Searchable { get; set; }

        /// <summary>
        ///     Flag to indicate if this column is orderable (true) or not (false)
        /// </summary>
        public bool Orderable { get; set; }

        /// <summary>
        ///     Search to apply to this specific column.
        /// </summary>
        public Search Search { get; set; }
    }

    public class Search
    {
        /// <summary>
        ///     Global search value. To be applied to all columns which have searchable as true
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     true if the global filter should be treated as a regular expression for advanced
        ///     searching, false otherwise. Note that normally server-side processing scripts
        ///     will not perform regular expression searching for performance reasons on large
        ///     data sets, but it is technically possible and at the discretion of your script
        /// </summary>
        public string Regex { get; set; }

        /// <summary>
        /// Specifies if the filter needs to be an exact match
        /// </summary>
        public bool ExactMatch { get; set; }
    }

    public class Order
    {
        /// <summary>
        ///     Column to which ordering should be applied. This is an index reference to the
        ///     columns array of information that is also submitted to the server
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        ///     Ordering direction for this column. It will be asc or desc to indicate ascending
        ///     ordering or descending ordering, respectively
        /// </summary>
        public string Dir { get; set; }
    }
}