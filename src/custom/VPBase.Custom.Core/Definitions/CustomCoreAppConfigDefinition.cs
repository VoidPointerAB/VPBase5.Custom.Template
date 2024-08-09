using System.Collections.Generic;
using System.Reflection;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.ConfigEntities.Applications;
using VPBase.Auth.Contract.ConfigEntities.CustomFields;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.Definitions.AppConfigs;
using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Core.Definitions
{
    public class CustomCoreAppConfigDefinition : IAppConfigDefinition
    {
        public const string CustomerId = "CustomCustomerId";

        public const string ModuleName = ConfigModuleConstants.Custom;

        #region IConfigSortable

        public string GetModuleName()
        {
            return ModuleName;
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

        #region application

        public ApplicationConfig GetApplication()
        {
            return new ApplicationConfig()
            {
                ApplicationId = ConfigIdHelper.GetId(CustomerId, ModuleName, ConfigEntityType.Application, ModuleName + "App"),
                PrefixName = ModuleName,
                Name = ModuleName + " Application",
                Scope = GlobalDefinitions.DefaultScope,
                ScopeDescription = " Your " + ModuleName.ToLower() + " information with roles, policies etc. needed for this application",
                CustomFieldEntityDefinitions = GetAllCustomFieldEntityDefinitions(),
                ApplicationClients = GetAllClients(),
                Activities = GetAllActivities(),
                ActivityIdsToRemove = GetActivityIdsToRemove(),
                Roles = GetAllRoles(),
                Policies = GetAllPolicies(),
            };
        }

        #endregion
        
        #region customFieldEntityDefintions

        /* VP_Template_Mvc BEGIN */

        public static CustomFieldEntityConfigDefinition CustomFieldEntityVP_Template_MvcDefinition = new CustomFieldEntityConfigDefinition()
        {
            CustomFieldEntityDefinitionId = ConfigIdHelper.GetId(CustomerId, ModuleName, ConfigEntityType.CustomFieldDefinition, "VP_Template_Mvc"),
            Name = "VP_Template_Mvc"
        };

        /* VP_Template_Mvc END */


        public static List<CustomFieldEntityConfigDefinition> GetAllCustomFieldEntityDefinitions()
        {
            return new List<CustomFieldEntityConfigDefinition>()
            {
                CustomFieldEntityVP_Template_MvcDefinition,
            };
        }

        #endregion

        #region clients

        public static List<ApplicationClientConfig> GetAllClients()
        {
            return new List<ApplicationClientConfig>()
            {

            };
        }

        #endregion

        #region activities

        // Note: No need to prefix the last part of the ActivityId (the name) in the CustomAppConfigDefinition. Example: nameOrUniqueId: "IssueWorkPermit. 
        // This since the id already contains the prefix using the CustomerId and the ModuleName combined.

        // ClaimType must be prefixed as hole string, but by using ModuleName as prefix it will be unique.

        /* VP_Template_Mvc BEGIN */

        public static ActivityConfig Activity_VP_Template_Mvc = new ActivityConfig()
        {
            ActivityId = ConfigIdHelper.GetId(CustomerId, ModuleName, ConfigEntityType.Activity, "VP_Template_Mvc"),   
            Name = "VP_Template_Mvc",
            ClaimValue = ModuleName.ToLower() + "_VP_Template_Mvc",
            SortOrder = 10001,
            Description = ModuleName + " VP_Template_Mvc",
            Active = true,
        };

        /* VP_Template_Mvc END */
        
        /* VP_Template_SimpleMvc BEGIN */

        public static ActivityConfig Activity_VP_Template_SimpleMvc = new ActivityConfig()
        {
            ActivityId = ConfigIdHelper.GetId(CustomerId, ModuleName, ConfigEntityType.Activity, "VP_Template_SimpleMvc"),
            Name = "VP_Template_SimpleMvc",
            ClaimValue = ModuleName.ToLower() + "_VP_Template_SimpleMvc",
            SortOrder = 10002,
            Description = ModuleName + " VP_Template_SimpleMvc",
            Active = true,
        };

        /* VP_Template_SimpleMvc END */

        public static List<ActivityConfig> GetAllActivities()
        {
            return new List<ActivityConfig>()
            {
                Activity_VP_Template_Mvc,
                Activity_VP_Template_SimpleMvc
            };
        }

        public static List<string> GetActivityIdsToRemove()
        {
            return new List<string>()
            {
                // The ignores should be in the sample as commented out lines
            };
        }

        #endregion

        #region roles

        public static List<RoleConfig> GetAllRoles()
        {
            return new List<RoleConfig>()
            {
                // Don't use any roles here! Put them in the sample if neeeded!
            };
        }

        #endregion

        #region policies

        /* VP_Template_Mvc BEGIN */

        public const string PolicyName_VP_Template_Mvc = "VPBase_Policy_VP_Template_Mvc";
        public static PolicyConfig Policy_VP_Template_Mvc = new PolicyConfig()
        {
            Name = PolicyName_VP_Template_Mvc,
            //RequiredClaim = new PolicyClaimConfig() { ClaimType = Activity_VP_Template_Mvc.ClaimType, RequiredValues = null }
            RequiredClaim = new PolicyClaimConfig()
            {
                ClaimType = AuthorizationConstants.JwtClaimTypes.Activity,
                RequiredValues = new List<string>() { Activity_VP_Template_Mvc.ClaimValue }
            }
        };

        /* VP_Template_Mvc END */

        /* VP_Template_SimpleMvc BEGIN */

        public const string PolicyName_VP_Template_SimpleMvc = "VPBase_Policy_VP_Template_SimpleMvc";
        public static PolicyConfig Policy_VP_Template_SimpleMvc = new PolicyConfig()
        {
            Name = PolicyName_VP_Template_SimpleMvc,
            //RequiredClaim = new PolicyClaimConfig() { ClaimType = Activity_VP_Template_SimpleMvc.ClaimType, RequiredValues = null }
            RequiredClaim = new PolicyClaimConfig()
            {
                ClaimType = AuthorizationConstants.JwtClaimTypes.Activity,
                RequiredValues = new List<string>() { Activity_VP_Template_SimpleMvc.ClaimValue }
            }
        };

        /* VP_Template_SimpleMvc END */

        public static List<PolicyConfig> GetAllPolicies()
        {
            return new List<PolicyConfig>()
            {
                Policy_VP_Template_Mvc,
                Policy_VP_Template_SimpleMvc
            };
        }

        #endregion
    }
}
