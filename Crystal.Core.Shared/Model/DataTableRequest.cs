using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Crystal.Core.Shared.Model
{
    public class DataTableRequest<TEntity>
    {
        public int Draw { get; set; }
        public Expression<Func<TEntity, bool>> SearchQuery { get; set; }
        public List<Func<TEntity, string>> SearchColumns { get; set; }
        public int Skip { get; set; }
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> OrderByQuery { get; set; }
        public string IncludeProperties { get; set; }
        public int TotalRecords { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; }
        public List<Order> Order { get; set; }
        public List<Column> Columns { get; set; }
    }

    public class Column
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public Search Search { get; set; }
    }

    public class Search
    {
        public string Value { get; set; }
        public string Regex { get; set; }
    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }
}