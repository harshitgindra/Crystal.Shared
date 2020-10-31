using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class BaseTests
    {
        protected TestContext DbContext { get; set; }
        protected IUowRepository Uow { get; set; }

        protected IUowRepository UowRepo { get; set; }

        [TearDown]
        public void TearDown()
        {
            Uow?.Dispose();
            DbContext?.Dispose();
           
        }
    }
}
