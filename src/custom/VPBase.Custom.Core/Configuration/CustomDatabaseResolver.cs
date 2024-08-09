using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using VPBase.Custom.Core.Data;
using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Configuration
{
    public class CustomDatabaseResolver : IDatabaseResolver
    {
        #region IConfigSortable

        public string GetModuleName()
        {
            return ConfigModuleConstants.Custom;
        }

        public string GetName()
        {
            return MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public double GetSortOrder()
        {
            return 10;      // Sortorder should be 10 - 49 in Base Custom implementations.
        }

        #endregion

        public void Register(IServiceCollection services, string defaultConnectionString, int dbCompatibilityLevel)
        {
            var migrationsStorageAssembly = typeof(ICustomStorage).GetTypeInfo().Assembly.GetName().Name;

            services.AddDbContext<CustomDatabaseManager>(options => options.UseSqlServer(defaultConnectionString,
                sql => sql.MigrationsAssembly(migrationsStorageAssembly)));

            var optionsBuilder = new DbContextOptionsBuilder<CustomDatabaseManager>();

            optionsBuilder.UseSqlServer(defaultConnectionString, db => db.UseCompatibilityLevel(dbCompatibilityLevel));

            services.AddSingleton<DbContextOptions<CustomDatabaseManager>>(optionsBuilder.Options);

            services.AddTransient<ICustomStorage, CustomDatabaseManager>();

            services.AddTransient<CustomDbTransactionHandler>();
            services.AddTransient<ICustomDbFactory, CustomDbFactory>();
        }
    }
}
