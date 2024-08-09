using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Core.Definitions;

namespace VPBase.Custom.Core.Definitions
{
    public class CustomSampleSettingsDefinition
    {
        public class CustomSample
        {
            public const string CategoryName = "CustomSampleCategory";
            public const string SubCategoryName = "CustomSampleSubCategory";

            public static SharedSetting ExampleSetting = new SharedSetting()
            {
                Key = "CustomSample_Example",
                Description = "Custom sample description",
                ExampleValue = "This is an example setting text value",
                CategoryName = CategoryName,
                SubCategoryName = SubCategoryName,
                ModuleName = ConfigModuleConstants.Custom,
            };
        }
    }
}
