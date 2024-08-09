using System.Collections.Generic;
using System.Reflection;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.ConfigEntities.CustomFields;
using VPBase.Auth.Contract.ConfigEntities.Tenants;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.Definitions.TenantConfigs;
using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Definitions
{
    public class CustomSampleTenantConfigDefinition : INetCoreTenantConfigDefinition
    {
        private const string TenantIdPrefix = "TEST";
        public static string TenantId = TenantDefinitions.GetTestTenant().TenantId;
        public static string TenantName = TenantDefinitions.GetTestTenant().Name;

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

        #region tenant

        public TenantConfig GetTenant()
        {
            return new TenantConfig()
            {
                TenantId = TenantId,
                Name = TenantName,
                CustomFields = GetAllCustomFields(),
                Groups = GetAllGroups(),
                ApplicationName = "CustomSample Application"
            };
        }

        #endregion

        #region customFields

        public static CustomFieldConfig GetSampleAuthUserCustomField()
        {
            var customField = new CustomFieldConfig(TenantId)
            {
                // AuthCustomerId_Auth_CustomField_SampleUser00001
                CustomFieldId = ConfigIdHelper.GetId(TenantIdPrefix, AuthDefinitions.ModuleType, ConfigEntityType.CustomField, "SampleUser00001"),

                // AuthCustomerId_Auth_CustomFieldDefinition_User
                CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),

                Title = "Sample Employee Identifier",
                TabName = "Sample",
                DataType = CustomFieldDataConfigType.String,
                FieldNeedToBeAnonymized = true
            };

            return customField;
        }

        public static CustomFieldConfig GetSampleAuthGroupCustomField()
        {
            var customField = new CustomFieldConfig(TenantId)
            {
                // AuthCustomerId_Auth_CustomField_SampleGroup00001
                CustomFieldId = ConfigIdHelper.GetId(TenantIdPrefix, AuthDefinitions.ModuleType, ConfigEntityType.CustomField, "SampleGroup00001"),

                // AuthCustomerId_Auth_CustomFieldDefinition_Group
                CustomFieldEntityId = AuthDefinitions.GetCustomFieldGroupDefinitionId(),

                Title = "Sample Group identifier",
                TabName = "Sample",
                DataType = CustomFieldDataConfigType.String,
                FieldNeedToBeAnonymized = false
            };

            return customField;
        }

        // Add specific custom fields below

        public static List<CustomFieldConfig> GetAllCustomFields()
        {
            return new List<CustomFieldConfig>()
            {
                // Add specific custom fields below:

                GetSampleAuthUserCustomField(),
                GetSampleAuthGroupCustomField(),
            };
        }

        #endregion

        #region groups

        public static GroupConfig GetSampleGroup()
        {
            var group = new GroupConfig()
            {
                // TenantPrefix_Auth_Group_Sample00001
                GroupId = ConfigIdHelper.GetId(TenantIdPrefix, AuthDefinitions.ModuleType, ConfigEntityType.Group, "Sample00001"),

                Name = "Sample Group"
            };

            return group;
        }

        // Add specific groups below

        public static List<GroupConfig> GetAllGroups()
        {
            return new List<GroupConfig>()
            {
                // Add specific groups below:

                GetSampleGroup(),
            };
        }

        #endregion
    }
}
