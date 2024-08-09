namespace VPBase.Custom.Core.CustomConfigImportExporter
{
    public class CustomTestData
    {
        public CustomTestData()
        {
            TestCustomDataItems = new List<CustomTestDataItem>();
            TestCustomDataItems2 = new List<CustomTestDataItem>();
        }

        public string Id { get; set; }

        public string TenantId { get; set; }

        public List<CustomTestDataItem> TestCustomDataItems { get; set; }

        public List<CustomTestDataItem> TestCustomDataItems2 { get; set; }
    }
}
