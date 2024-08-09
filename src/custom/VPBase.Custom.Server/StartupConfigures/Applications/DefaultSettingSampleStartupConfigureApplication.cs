using System.Reflection;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Server.StartupConfigures;
using VPBase.Shared.Server.Services.ContextSettings;
using VPBase.Custom.Core.Configuration;
using VPBase.Custom.Core.Definitions;

namespace VPBase.Custom.Server.StartupConfigures.Applications
{
    /// <summary>
    /// Check base implementation in: DefaultSettingStartupConfigureApplication.cs
    /// </summary>
    public class DefaultSettingSampleStartupConfigureApplication : IStartupConfigureApplication
    {
        #region IConfigSortable

        public string GetModuleName()
        {
            return ConfigModuleConstants.Base;
        }

        public string GetName()
        {
            return MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public double GetSortOrder()
        {
            return 0.2;        // IMPORTANT! Normally sortorder should be > 10 in custom implementations but here we want the implementation to be
                               // exectuted directly in the beginning chain of configure applications no matter custom or module implementations and before other
                               // implementations in the base.
        }

        #endregion

        public void Configure(StartupConfigureApplicationParams startupConfigureApplicationParams)
        {
            using (var serviceScope = startupConfigureApplicationParams.ApplicationBuilder.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var serviceProvider = serviceScope.ServiceProvider;
                var contextSettingService = serviceProvider.GetService<IContextSettingService>();

                // Custom
                var customAppSetting = serviceProvider.GetService<CustomAppSettings>();

                InjectDefaultSettingToStaticMemory(contextSettingService, customAppSetting);
            }
        }

        /// <summary>
        /// This methods adds default appsettings directly to the cached context setting list in the memory. 
        /// Since the appsettings can be changed all the time is of no use to store the values to the database first
        /// </summary>
        /// <param name="contextSettingService">Context Service</param>
        /// <param name="customAppSettings">Custom AppSettings</param>
        public void InjectDefaultSettingToStaticMemory(IContextSettingService contextSettingService, CustomAppSettings customAppSettings)
        {
            // Example
            var customSampleExampleSettingValue = CustomSampleSettingsDefinition.CustomSample.ExampleSetting.ExampleValue as string;

            if (!string.IsNullOrEmpty(customAppSettings.ExampleSetting))
            {
                customSampleExampleSettingValue = customAppSettings.ExampleSetting;
            }

            contextSettingService.AddNewSharedSettingWithDefaultObjectValue(CustomSampleSettingsDefinition.CustomSample.ExampleSetting, customSampleExampleSettingValue, true);
        }
    }
}
