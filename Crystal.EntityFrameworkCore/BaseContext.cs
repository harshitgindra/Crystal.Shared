using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Crystal.EntityFrameworkCore
{
    /// <summary>
    /// Base context derived from DbContext
    /// </summary>
    public class BaseContext : DbContext
    {
        /// <summary>
        /// Default base context constructor 
        /// </summary>
        public BaseContext()
        {
        }

        /// <summary>
        /// Default base context constructor with DbContextOptions
        /// </summary>
        /// <param name="options"></param>
        public BaseContext(DbContextOptions options)
            : base(options)
        {
        }
        /// <summary>
        /// Context builder
        /// </summary>
        protected virtual DbContextOptionsBuilder ContextBuilder { get; set; }
        /// <summary>
        /// Database transaction
        /// </summary>
        public virtual IDbContextTransaction Transaction { get; set; }
        /// <summary>
        /// Starts a new transaction
        /// </summary>
        public virtual void BeginTransaction()
        {
            this.Transaction = this.Database.BeginTransaction();
        }
        /// <summary>
        /// Saves uncommitted changes to the database
        /// </summary>
        /// <returns>The number of records added/updated</returns>
        public override int SaveChanges()
        {
            //***
            //*** Commits the changes on the transaction
            //***
            this.Transaction?.Commit();
            //***
            //*** Save changes
            //***
            var returnValue = base.SaveChanges();
            //***
            //*** Clean all trackers
            //***
            this.ChangeTracker.Clear();
            //***
            //*** Returns the number of records added/updated
            //***
            return returnValue;
        }
        /// <summary>
        /// Commits pending changes to the database
        /// </summary>
        public virtual void Commit()
        {
            //***
            //*** Save pending changes
            //***
            _ = this.SaveChanges();
        }
        /// <summary>
        /// Commits pending changes to the database
        /// </summary>
        public virtual Task CommitAsync()
        {
            //***
            //*** Save pending changes
            //***
            this.Commit();
            return Task.CompletedTask;
        }
        /// <summary>
        /// Commits pending bulk changes to the database
        /// </summary>
        public virtual void CommitBulkChanges()
        {
            //***
            //*** Save pending bulk changes
            //***
            this.BulkSaveChanges();
            //***
            //*** Commit changes on the transaction
            //***
            this.Transaction?.Commit();
            //***
            //*** Clear tracker
            //***
            this.ChangeTracker.Clear();
        }
        /// <summary>
        /// Commits pending bulk changes to the database
        /// </summary>
        public virtual async Task CommitBulkChangesAsync()
        {
            //***
            //*** Save pending bulk changes
            //***
            await this.BulkSaveChangesAsync();
            //***
            //*** Commit changes on the transaction
            //***
            await this.Transaction?.CommitAsync();
            //***
            //*** Clear tracker
            //***
            this.ChangeTracker.Clear();
        }
        /// <summary>
        /// Rollback any pending changes
        /// </summary>
        public virtual void Rollback()
        {
            //***
            //*** Rollback any pending changes on the transaction
            //***
            this.Transaction?.Rollback();
            //***
            //*** Clear tracker
            //***
            this.ChangeTracker.Clear();
        }
        /// <summary>
        /// Rollback any pending changes
        /// </summary>
        public virtual async Task RollbackAsync()
        {
            //***
            //*** Rollback any pending changes on the transaction
            //***
            await this.Transaction?.RollbackAsync();
            //***
            //*** Clear tracker
            //***
            this.ChangeTracker.Clear();
        }
        /// <summary>
        /// Dispose transaction and dBContext
        /// </summary>
        public override void Dispose()
        {
            //***
            //*** Dispose the transaction
            //***
            this.Transaction?.Dispose();
            base.Dispose();
        }
    }
}