#region USING
using Crystal.Patterns.Abstraction;
using System.Threading.Tasks;

#endregion

namespace Crystal.EntityFrameworkCore
{
    public class BaseUowRepository : IBaseUowRepository
    {
        public BaseUowRepository(BaseContext context)
        {
            DbContext = context;
        }

        public BaseUowRepository()
        {
            if (DbContext == null)
            {
                DbContext = new BaseContext();
            }
        }

        private BaseContext DbContext { get; }

        public async Task<bool> Commit()
        {
            var returnValue = true;
            if (DbContext != null && DbContext.ChangeTracker.HasChanges())
            {
                returnValue = await DbContext.SaveChangesAsync() > 0;

            }

            return returnValue;
        }

        public void Dispose()
        {
            DbContext?.Dispose();
        }
    }
}