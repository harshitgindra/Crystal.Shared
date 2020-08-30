using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string Name { get; set; }

        public int Value { get; set; }
    }
}
