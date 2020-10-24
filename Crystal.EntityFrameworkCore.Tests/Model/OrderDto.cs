using System;
using System.Collections.Generic;
using System.Text;

namespace Crystal.EntityFrameworkCore.Tests.Model
{
    public class OrderDto
    {
        public int OrderId { get; set; }

        public string OrderName { get; set; }

        public int Value { get; set; }
    }
}
