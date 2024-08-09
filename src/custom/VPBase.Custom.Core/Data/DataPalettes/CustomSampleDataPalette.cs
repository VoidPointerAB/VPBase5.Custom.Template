using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.Definitions;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Data.DataPalettes
{
    public static class CustomSampleDataPalette
    {
        public static DateTime ReferenceDate = DateTime.MinValue;
        public const string TenantId = TenantDefinitions.TenantTestId; 
        public const string TestDescription = "Skapad av demo/test- miljö";

        public static class Ids
        {
            public const string EntityId = "EntityId";
            public const string TestIdPrefix = "TEST-";

            public static int VP_Template_SimpleMvcId = 0;
            public static int VP_Template_MvcId = 0;

            #region GetId

            public static string GetVP_Template_SimpleMvcId(string tenantId)
            {
                VP_Template_SimpleMvcId++;
                return ConfigIdHelper.GetId(GetTenantIdPrefix(tenantId), ConfigModuleConstants.Custom, "TSM-VPTemplateSimpleMvc", $"{TestIdPrefix}{VP_Template_SimpleMvcId.ToString("D6")}");
            }

            public static string GetVP_Template_MvcId(string tenantId)
            {
                VP_Template_MvcId++;
                return ConfigIdHelper.GetId(GetTenantIdPrefix(tenantId), ConfigModuleConstants.Custom, "TM-VPVPTemplateMvc", $"{TestIdPrefix}{VP_Template_MvcId.ToString("D6")}");
            }

            #endregion

            public static string GetTenantIdPrefix(string tenantId)
            {
                if (IsValidTenantId(tenantId))
                {
                    return ConfigIdHelper.GetTenantIdPrefixFromTenantId(tenantId);
                }
                return "VP";
            }

            public static bool IsValidTenantId(string tenantId)
            {
                if (string.IsNullOrEmpty(tenantId))
                {
                    return false;
                }
                string[] array = tenantId.Split(new char[1] { '_' });
                return array.Length == 4;
            }

            public static void Clear()
            {
                VP_Template_SimpleMvcId = 0;
                VP_Template_MvcId = 0;
            }
        }

        public static class Entities
        {
            public static VP_Template_SimpleMvc VP_Template_SimpleMvc(
                string tenantId = null,
                string id = null,
                string name = null,
                string description = null,
                DateTime? createdUtc = null,
                DateTime? modifiedUtc = null,
                DateTime? deletedUtc = null,
                DateTime? anonymizedUtc = null
                )
            {

                if (tenantId == null)
                {
                    tenantId = TenantId;
                }

                var uniqueId = Ids.GetVP_Template_SimpleMvcId(tenantId);
                //friendlyId = friendlyId ?? "TSM" + Ids.VP_Template_SimpleMvcId.ToString("D4");    // Todo: Should be implemented!

                return new VP_Template_SimpleMvc(tenantId)
                {
                    VP_Template_SimpleMvcId = id ?? uniqueId,                  
                    Title = name ?? "Djuret X",
                    Description = description ?? TestDescription,
                    CreatedUtc = createdUtc ?? ReferenceDate,
                    ModifiedUtc = modifiedUtc ?? ReferenceDate,
                    DeletedUtc = deletedUtc,
                    AnonymizedUtc = anonymizedUtc
                };
            }

            public static VP_Template_Mvc VP_Template_Mvc(
                string tenantId = null,
                string id = null,
                string name = null,
                string description = null,
                DateTime? createdUtc = null,
                DateTime? modifiedUtc = null,
                DateTime? deletedUtc = null,
                DateTime? anonymizedUtc = null
            )
            {
                if (tenantId == null)
                {
                    tenantId = TenantId;
                }

                var uniqueId = Ids.GetVP_Template_MvcId(tenantId);
                //friendlyId = friendlyId ?? "TMV" + Ids.VP_Template_MvcId.ToString("D4");    // Todo: Should be implemented!

                return new VP_Template_Mvc(tenantId)
                {
                    VP_Template_MvcId = id ?? uniqueId,
                    Title = name ?? "Frukt X",
                    Description = description ?? TestDescription,
                    CreatedUtc = createdUtc ?? ReferenceDate,
                    ModifiedUtc = modifiedUtc ?? ReferenceDate,
                    DeletedUtc = deletedUtc,
                    AnonymizedUtc = anonymizedUtc
                };
            }
        }

        public static class Models
        {
        }
    }
}

   

