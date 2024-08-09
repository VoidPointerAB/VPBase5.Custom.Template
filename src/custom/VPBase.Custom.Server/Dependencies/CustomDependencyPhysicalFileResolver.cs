using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using VPBase.Custom.Core.Configuration;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Server.Dependencies;

namespace VPBase.Custom.Server.Dependencies
{
    // NOTE: This class should NOT be registered in the MemoryCustomDbTestBase class!!!!
    public class CustomDependencyPhysicalFileResolver : IDependencyResolver
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
            return 11;       // Sortorder should be 10 - 49 in Base Custom implementations.
        }

        #endregion

        public void Register(IServiceCollection services, EnvironmentMode environmentMode)
        {
            // Use CustomAppSettingsHelper to get CustomAppSettings since it is not yet registered
            var customAppSettings = CustomAppSettingsHelper.GetCustomAppSettings();     // Here ok to do physical file access
            services.AddSingleton(customAppSettings);

            var customModuleAppSettings = new CustomModuleAppSettings
            {
                Name = typeof(CustomAppSettings).FullName,
                Settings = customAppSettings
            };

            services.AddSingleton<IModuleAppSettings>(customModuleAppSettings);
        }

        public DependencyType GetDependencyType()
        {
            return DependencyType.PhysicalFiles;
        }
    }
}
