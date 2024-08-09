using Microsoft.Extensions.Configuration;
using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Configuration
{
    public class CustomAppSettingsHelper
    {
        private static CustomAppSettings _customAppSettings = null;

        public static CustomAppSettings GetCustomAppSettings()
        {
            if (_customAppSettings == null)
            {
                var settingsSection = SettingsHelper.GetConfigurationSection("CustomAppSettings");
                _customAppSettings = settingsSection.Get<CustomAppSettings>();
            }
            return _customAppSettings;
        }

        #region Testing 

        public static void Clear()
        {
            ApplyCustomAppSettings(null);
        }

        public static void ApplyCustomAppSettings(CustomAppSettings customAppSettings)
        {
            _customAppSettings = customAppSettings;
        }

        #endregion
    }
}
