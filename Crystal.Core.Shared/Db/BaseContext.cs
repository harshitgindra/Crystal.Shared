using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Crystal.Core.Shared.Db
{
    public class BaseContext : DbContext
    {
        protected DbContextOptionsBuilder ContextBuilder { get;set;}

        public BaseContext()
        {
        }

        public BaseContext(DbContextOptions<BaseContext> options)
            : base(options)
        {
        }
    }
}
