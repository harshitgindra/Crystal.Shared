using SQLite;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MicroOrm.Dapper.Repositories.Attributes;

namespace Crystal.Dapper.Tests
{
    public class Product
    {
        [Identity]
        [Key]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}
