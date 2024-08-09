/*═══════════════════════════════════════════════════════════════════════════════════════════╗
║ MODIFY CAREFULLY: CustomDatabaseManager
╟────────────────────────────────────────────────────────────────────────────────────────────╢
║ This file is provided from base.
║ If you do not like painful updates in the future, be careful with the changes you make
║ to this file. Try to keep your changes to the highlighted areas below.
╚═══════════════════════════════════════════════════════════════════════════════════════════*/
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Core.Data;

namespace VPBase.Custom.Core.Data
{
    public class CustomDatabaseManager : DbContext, ICustomStorage
    {
        public CustomDatabaseManager(DbContextOptions<CustomDatabaseManager> options)
            : base(options)
        { }

        /* VP_Template_Mvc BEGIN */
        
        public DbSet<VP_Template_Mvc> VP_Template_Mvcs { get; set; }
        IQueryable<VP_Template_Mvc> ICustomStorage.VP_Template_Mvcs => VP_Template_Mvcs;
        
        /* VP_Template_Mvc END */

        /* VP_Template_SimpleMvcs BEGIN */

        public DbSet<VP_Template_SimpleMvc> VP_Template_SimpleMvcs { get; set; }
        IQueryable<VP_Template_SimpleMvc> ICustomStorage.VP_Template_SimpleMvcs => VP_Template_SimpleMvcs;

        /* VP_Template_SimpleMvcs END */


        /*═══════════════════════════════════════════════════════════════════════════════════════════
        ║ Custom modifications - START 
        ║ */



        /* Custom modifications - END                                                               ║
        ════════════════════════════════════════════════════════════════════════════════════════════*/


        public IDbContextTransaction BeginTransaction()
        {
            return !IsInMemory ? Database.BeginTransaction() : new InMemoryContextTransaction();
        }
        public Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return Database.BeginTransactionAsync();
        }

        public bool IsInMemory => Database.ProviderName.Contains("InMemory");

        int ICustomStorage.SaveChanges()
        {
            return SaveChanges();
        }

        T ICustomStorage.Add<T>(T entity)
        {
            return Set<T>().Add(entity).Entity;
        }

        T ICustomStorage.Delete<T>(T entity)
        {
            return Set<T>().Remove(entity).Entity;
        }

        T ICustomStorage.Attach<T>(T entity)
        {
            var entry = Entry(entity);
            entry.State = EntityState.Modified;
            return entity;
        }

        #region Async

        Task<int> ICustomStorage.SaveChangesAsync(CancellationToken cancellationToken)
        {
            return SaveChangesAsync(cancellationToken);
        }

        async Task<T> ICustomStorage.AddAsync<T>(T entity, CancellationToken cancellationToken) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            var entityEntry = await Set<T>().AddAsync(entity, cancellationToken);
            return entityEntry.Entity;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (!IsInMemory)
            {
                return await Database.BeginTransactionAsync(cancellationToken);
            }
            else
            {
                var inMemoryTransaction = new InMemoryContextTransaction();
                return await Task.FromResult(inMemoryTransaction);
            }
        }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseLazyLoadingProxies();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(ConfigModuleConstants.Custom);
            /*═══════════════════════════════════════════════════════════════════════════════════════════
            ║ Custom modifications - START 
            ║ */



            /* Custom modifications - END                                                               ║
            ════════════════════════════════════════════════════════════════════════════════════════════*/
        }
    }
}
