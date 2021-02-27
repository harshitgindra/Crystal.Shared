using Crystal.Abstraction;

namespace Samples.EfCore.Web
{
    public interface IUowRepository : IBaseUowRepository
    {
        IBaseRepository<Book> Books { get; }
        IBaseRepository<Author> Authors { get; }
    }
}
