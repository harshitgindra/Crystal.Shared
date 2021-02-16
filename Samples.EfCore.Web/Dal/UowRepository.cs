using Crystal.Abstraction;
using Crystal.EntityFrameworkCore;

namespace Samples.EfCore.Web
{
    public class UowRepository : BaseUowRepository, IUowRepository
    {
        private IBaseRepository<Book> _bookRepository;
        private IBaseRepository<Author> _authorRepository;

        public UowRepository(SampleContext ctx) : base(ctx)
        {
        }

        public IBaseRepository<Book> Books
        {
            get
            {
                if (_bookRepository == null)
                {
                    _bookRepository = new BaseRepository<Book>(this.DbContext);
                }
                return _bookRepository;
            }
        }

        public IBaseRepository<Author> Authors
        {
            get
            {
                if (_authorRepository == null)
                {
                    _authorRepository = new BaseRepository<Author>(this.DbContext);
                }
                return _authorRepository;
            }
        }
    }
}
