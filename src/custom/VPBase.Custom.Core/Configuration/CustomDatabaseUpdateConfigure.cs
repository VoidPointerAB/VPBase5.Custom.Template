// MOVED to the "VPBase.Custom.Server" project to not reference the full application with Microsoft.NETCore.App, since
// IApplicationBuilder is moved / deprecated.

//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using System.Reflection;
//using VPBase.Auth.Contract.ConfigEntities;
//using VPBase.Custom.Core.Data;
//using VPBase.Shared.Core.Configuration;

//namespace VPBase.Custom.Core.Configuration
//{
//    public class CustomDatabaseUpdateConfigure : IDatabaseUpdateConfigure
//    {
//        #region IConfigSortable

//        public string GetModuleName()
//        {
//            return ConfigModuleConstants.Custom;
//        }

//        public string GetName()
//        {
//            return MethodBase.GetCurrentMethod().DeclaringType.Name;
//        }

//        public double GetSortOrder()
//        {
//            return 10;
//        }

//        #endregion

//        public void Update(IApplicationBuilder app, EnvironmentMode environmentMode, ILogger logger, string testTenantId, IConfigEntities configEntities)
//        {
//            using (var serviceScope = app.ApplicationServices
//                .GetRequiredService<IServiceScopeFactory>()
//                .CreateScope())
//            {
//                using (var customContext = serviceScope.ServiceProvider.GetService<CustomDatabaseManager>())
//                {
//                    customContext.Database.Migrate();
//                }
//            }
//        }
//    }
//}
