using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crystal.EntityFrameworkCore.Tests
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}
