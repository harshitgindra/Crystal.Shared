using System.Threading.Tasks;
using Crystal.Abstraction.Repository;

namespace Crystal.EntityFramework.Repository
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseUowRepository(BaseContext context)
        {
            DbContext = context;
        }

        private BaseContext DbContext { get; }

        public async Task<bool> Commit()
        {
            var returnValue = true;
            if (DbContext != null && DbContext.ChangeTracker.HasChanges())
                returnValue = await DbContext.SaveChangesAsync() > 0;

            return returnValue;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}