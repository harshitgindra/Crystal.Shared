using Crystal.Patterns.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class UowRepository : BaseUowRepository, IUowRepository
    {
        public UowRepository() : base(new TestContext())
        {

        }

        public TestContext Context => (TestContext)this.Context;
    }

    public interface IUowRepository : IBaseUowRepository
    {
        TestContext Context { get; }
    }
}
