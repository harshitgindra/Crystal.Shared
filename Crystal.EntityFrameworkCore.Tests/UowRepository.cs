using AutoMapper;
using Crystal.Patterns.Abstraction;

namespace Crystal.EntityFrameworkCore.Tests
{
    public class UowRepository : BaseUowRepository, IUowRepository
    {
        public UowRepository() : base(new TestContext())
        {

        }

        public UowRepository(BaseContext context) : base(context)
        {

        }

        public UowRepository(BaseContext context, MapperConfiguration mapperConfiguration)
            : base(context, mapperConfiguration)
        {

        }

        public TestContext Context => (TestContext)this.DbContext;

        private IBaseRepository<Order> _order;
        public IBaseRepository<Order> Order
        {
            get
            {
                if (_order == null)
                {
                    _order = new BaseRepository<Order>(this.DbContext, this.MapperConfiguration);
                }

                return _order;
            }
        }
    }

    public interface IUowRepository : IBaseUowRepository
    {
        IBaseRepository<Order> Order { get; }
    }
}
