using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VPBase.Custom.Core.Configuration;
using VPBase.Custom.Tests.Helpers;


namespace VPBase.Custom.Tests.Settings
{
    [TestFixture]
    public class CustomAppSettingsTests : MemoryCustomDbTestBase
    {
        [Test]
        public void When_getting_custom_app_settings()
        {
            Assert.That(CustomAppSettings, Is.Not.Null);

            CustomAppSettings.Name = "Test";

            var customAppSettings = ServiceProvider.GetService<CustomAppSettings>();
            Assert.That(customAppSettings, Is.Not.Null);
            Assert.That(customAppSettings.Name, Is.EqualTo("Test"));
        }
    }
}
