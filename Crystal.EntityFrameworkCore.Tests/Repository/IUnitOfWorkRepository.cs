using Crystal.Patterns.Abstraction;

namespace Crystal.EntityFrameworkCore.Tests
{
    public interface IUnitOfWorkRepository
    {
        IBaseRepository<Order> Orders { get; }
    }

    public class UnitOfWorkRepository : BaseUowRepository, IUnitOfWorkRepository
    {
        private IBaseRepository<Order> _orders;

        public UnitOfWorkRepository() : base(new TestContext())
        {
        }

        public UnitOfWorkRepository(BaseContext context) : base(context)
        {
        }

        public IBaseRepository<Order> Orders => GetInstance<Order>(_orders);
    }
}
