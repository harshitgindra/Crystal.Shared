using Crystal.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Samples.EfCore.Web
{
    public interface IUowRepository : IBaseUowRepository
    {
        IBaseRepository<Book> Books { get; }
        IBaseRepository<Author> Authors { get; }
    }
}
