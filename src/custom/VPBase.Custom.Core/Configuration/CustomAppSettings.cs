using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Configuration
{
    public class CustomAppSettings
    {
        public CustomAppSettings()
        {

        }

        public string Name { get; set; }

        public string ExampleSetting { get; set; }
    }

    public class CustomModuleAppSettings : IModuleAppSettings
    {
        public string Name { get; set; }

        public object Settings { get; set; }
    }
}
