using Crystal.Core.Shared.Abstraction;
using System.Threading.Tasks;

namespace Crystal.Core.Shared.Db
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseContext DbContext { get;set;}

        public BaseUowRepository(BaseContext context)
        {
            DbContext = context;
        }

        public async Task<bool> Commit()
        {
            bool returnValue = true;
            if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                returnValue = (await DbContext.SaveChangesAsync()) > 0;
            }
            return returnValue;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}
