using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Custom.Core.Data;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Server.Configuration;

namespace VPBase.Custom.Server.Configuration
{
    public class CustomDatabaseUpdateConfigure : IDatabaseUpdateConfigure
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
            return 10;       // Sortorder should be 10 - 49 in Base Custom implementations.
        }

        #endregion

        public void Update(IApplicationBuilder app, EnvironmentMode environmentMode, ILogger logger, string testTenantId, IConfigEntities configEntities)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var customContext = serviceScope.ServiceProvider.GetService<CustomDatabaseManager>())
                {
                    customContext.Database.Migrate();
                }
            }
        }
    }
}
