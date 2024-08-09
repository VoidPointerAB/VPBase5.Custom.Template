using System.Reflection;
using VPBase.Custom.Core.Configuration;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Data.DataImporters.DemoTest;
using VPBase.Custom.Core.Data.DataImporters.Foundation;
using VPBase.Custom.Core.Definitions;
using VPBase.Custom.Server.Configuration.MenuLayers;
using VPBase.Custom.Server.Dependencies;
using VPBase.Custom.Server.Jobs;
using VPBase.Custom.Server.StartupConfigures;
using VPBase.Shared.Server.Configuration;

namespace VPBase.Custom.Server.Configuration
{
    public class CustomStartupInstruction : IStartupInstruction
    {
        public void Execute(ConfigurationItemsContainer container)
        {
            container.ServerAssemblies.Add(Assembly.GetAssembly(typeof(CustomStartupInstruction)));
            container.GdprAssemblies.Add(Assembly.GetAssembly(typeof(CustomDatabaseManager)));

            container.DatabaseResolvers.Add(new CustomDatabaseResolver());

            container.DatabaseUpdateConfigure.Add(new CustomDatabaseUpdateConfigure());

            container.AppConfigDefinitions.Add(new CustomCoreAppConfigDefinition());

            container.DependencyResolvers.Add(new CustomDependencyPhysicalFileResolver());

            ExecuteSampleInstructions(container);
        }

        private void ExecuteCustomInstructions(ConfigurationItemsContainer container)
        {
            /*════════════════════════════════════════════════════════════════════════════════════════╗
            ║ Plug in your custom instructions below.
            ║ To prevent future conflicts, do NOT modify the sample classes. Instead, make copies
            ║ of them and modify the copies. Then refer to the copies from this method.
            ║ Update the Execute method to call custom implementation instead of sample implementation.
            ╚════════════════════════════════════════════════════════════════════════════════════════*/
        }

        private void ExecuteSampleInstructions(ConfigurationItemsContainer container)
        {
            container.DependencyResolvers.Add(new CustomSampleDependencyResolver());

            container.FoundationDataInjectors.Add(new CustomSampleFoundationDataInjector());

            container.MenuLayers.Add(new CustomSample_MainMenuLayer());   // A sample showing how to add your own menu with different types of menu items
            container.MenuLayers.Add(new CustomSample_ExtendedMainMenuLayer());   // A sample showing how to add an additional layer + custom access check
            container.MenuLayers.Add(new CustomSample_DeveloperMenuLayer());  // A sample showing how to add your own layer to the developer menu
            
            container.AppConfigDefinitions.Add(new CustomSampleAppConfigDefinition());

            container.AuthInMemoryTestDataList.Add(new CustomSampleAuthInMemoryTestData());

            container.JobSchedulers.Add(new CustomSampleJobScheduler());

            container.DemoTestDataSetups.Add(new CustomSampleGenericCleanupDataSetup());
            container.DemoTestDataSetups.Add(new CustomSampleDemoDataSetup());
            container.DemoTestDataSetups.Add(new CustomSampleTestDataSetup());

            container.RoutingDefinitions.Add(new CustomSampleRoutingDefinition());

            container.TenantConfigDefinitions.Add(new CustomSampleTenantConfigDefinition());

            container.StartupConfigure.Add(new StartupCustomSampleConfigureInstruction());
        }
    }
}
