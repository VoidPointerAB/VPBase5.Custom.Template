using IdentityModel;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.Models;
using VPBase.Auth.Contract.Models.Registers;
using VPBase.Custom.Core.Definitions;
using VPBase.Shared.Core.Definitions;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Models.Auth;
using VPBase.Shared.Core.SharedImplementations.AuthContract;
using VPBase.Shared.Server.Services;

namespace VPBase.Custom.Server.Areas.Custom.WebAppServices
{
    public class CustomAccountLocalWebAppService : IAccountWebAppService
    {
        private List<CustomAuthUser> CustomAuthUsers { get; set; }

        private const string CustomFieldId_VP_TimeReportId = "AuthVoidPointer_Auth_CustomField_User00001";
        private const string CustomFieldId_Test_TestId = "AuthTest_Auth_CustomField_TESTUser00001";

        public CustomAccountLocalWebAppService()
        {
            CustomAuthUsers = GetAuthUsers();
        }

        public Tuple<ServiceStatus, AuthSharedUserModel> ValidateUserCredentials(string username, string password)
        {
            var customAuthUser = CustomAuthUsers.FirstOrDefault(x => x.UserName == username && x.Password == password);

            if (customAuthUser != null)
            {
                return Tuple.Create(new ServiceStatus(username, true), customAuthUser.AuthUser);
            }

            return Tuple.Create(new ServiceStatus(username, false), new AuthSharedUserModel());
        }

        public Tuple<ServiceStatus, AuthSharedUserModel> GetUserCredentials(string username, bool throwException = false)
        {
            try
            {
                var customAuthUser = CustomAuthUsers.FirstOrDefault(x => x.UserName == username);

                if (customAuthUser != null)
                {
                    return Tuple.Create(new ServiceStatus(username, true), customAuthUser.AuthUser);
                }
            }
            catch (Exception)
            {
                if (throwException)
                {
                    throw;
                }
            }

            return Tuple.Create(new ServiceStatus(username, false), new AuthSharedUserModel());
        }

        public Tuple<ServiceStatus, AuthSharedUserAccountsModel> GetUserAccounts(string userId)
        {
            var customAuthUser = CustomAuthUsers.FirstOrDefault(x => x.AuthUser.UserId == userId);

            if (customAuthUser != null)
            {
                var authAccountModel = new AuthSharedUserAccountsModel
                {
                    UserAccounts = new List<AuthSharedUserAccountModel>()
                    {
                        new AuthSharedUserAccountModel()
                        {
                            AccountId = "1",
                            UserName = customAuthUser.UserName
                        }
                    }
                };

                return Tuple.Create(new ServiceStatus(userId, true), authAccountModel);
            }

            return Tuple.Create(new ServiceStatus(userId, false), new AuthSharedUserAccountsModel());
        }

        public List<AuthSharedUserBaseModel> GetAllActiveUsers(string tenantId)
        {
            return GetAllActiveUsersInternal(tenantId);
        }

        public int GetAllActiveUsersCount(string tenantId = null)
        {
            return GetAllActiveUsersInternal(tenantId).Count();
        }

        public List<AuthCustomFieldModel> GetUserCustomFields(string tenantId)
        {
            if (tenantId == TenantDefinitions.GetVoidPointerTenant().TenantId)
            {
                var userCustomFields = new List<AuthCustomFieldModel>()
                {
                    new AuthCustomFieldModel()
                    {
                        CreatedUtc = new DateTime(2022, 03, 29, 15, 40, 33),
                        ModifiedUtc = new DateTime(2022, 03, 29, 15, 40, 33),
                        CustomFieldId = CustomFieldId_VP_TimeReportId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        Title = "VP Time Report Identifier",
                        TabName = "TimeReport",
                        FieldNeedToBeAnonymized = false,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        OptionFieldsJson = null,
                        OptionValuesJson = null,
                        Encrypted = false,
                    }
                };

                return userCustomFields;
            }

            if (tenantId == TenantDefinitions.GetTestTenant().TenantId)
            {
                var userCustomFields = new List<AuthCustomFieldModel>()
                {
                    new AuthCustomFieldModel()
                    {
                        CreatedUtc = new DateTime(2021, 02, 2, 15, 2, 1),
                        ModifiedUtc = new DateTime(2021, 05, 11, 17, 10, 5),
                        CustomFieldId = CustomFieldId_Test_TestId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        Title = "Test Identifier",
                        TabName = "TestTab",
                        FieldNeedToBeAnonymized = false,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        OptionFieldsJson = null,
                        OptionValuesJson = null,
                        Encrypted = false,
                    }
                };

                return userCustomFields;
            }

            return new List<AuthCustomFieldModel>();
        }

        // New 2023-08-06
        public RegisterUserReturnModel RegisterUser(RegisterUserModel registerUser)
        {
            var registerUserReturnModel = new RegisterUserReturnModel();

            registerUserReturnModel.Errors.Add("Function not implemented!");

            return registerUserReturnModel;
        }

        private List<CustomAuthUser> GetAuthUsers()
        {
            var listOfUsers = new List<CustomAuthUser>();

            var now = DateTime.UtcNow;

            var tenantTestClaimValueJson = GetTenantClaimValueJson(TenantDefinitions.GetTestTenant().TenantId, TenantDefinitions.GetTestTenant().Name);
            var tenantVPClaimValueJson = GetTenantClaimValueJson(TenantDefinitions.GetVoidPointerTenant().TenantId, TenantDefinitions.GetVoidPointerTenant().Name);

            var authUser1 = new AuthSharedUserModel
            {
                DisplayName = "Tjorvar Olofsson",
                FirstName = "Tjorvar",
                LastName = "Oloffson",
                UserId = "USR0001",

                Claims =
                {
                    // User Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantId, TenantDefinitions.TenantTestId),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantInfo, tenantTestClaimValueJson),

                    // Role Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_Mvc.ClaimValue),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_SimpleMvc.ClaimValue),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, SharedAppConfigDefinition.Activity_Impersonate.ClaimValue),

                    new AuthClaimModel(JwtClaimTypes.Role, SharedAppConfigDefinition.RoleSystemAdmin.Name),

                    // Extra
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.AuthMode, "local_custom"),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.RefreshTime.LocalUtc, now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                },

                CustomFieldWithStringValueModels = new List<AuthSharedUserCustomFieldWithStringValueModel>()
                {
                    new AuthSharedUserCustomFieldWithStringValueModel()
                    {
                        TenantId = TenantDefinitions.TenantTestId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        CustomFieldId = CustomFieldId_Test_TestId,
                        CustomFieldValueId = "831dd594-48cb-47ab-898f-eb38a8c6b3d3",
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        Value = "765"
                    }
                },

                Roles = new List<AuthSharedUserRoleModel>() 
                { 
                    new AuthSharedUserRoleModel()
                    { 
                        RoleId = SharedAppConfigDefinition.RoleSystemAdmin.RoleId,
                        RoleName = SharedAppConfigDefinition.RoleSystemAdmin.Name
                    }
                },

                IsMultiTenant = false
            };

            listOfUsers.Add(new CustomAuthUser() { UserName = "user", Password = "pass", AuthUser = authUser1 });

            var authUser2 = new AuthSharedUserModel
            {
                DisplayName = "Gunhild Svensson",
                FirstName = "Gunhild",
                LastName = "Svensson",
                UserId = "USR0002",
                Claims =
                {
                    // User Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantId, TenantDefinitions.TenantTestId),          // Test
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantInfo, tenantTestClaimValueJson),

                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantId, TenantDefinitions.TenantVoidPointerId),   // VP
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantInfo, tenantVPClaimValueJson),

                    // Role Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_Mvc.ClaimValue),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_SimpleMvc.ClaimValue),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, SharedAppConfigDefinition.Activity_Impersonate.ClaimValue),

                    new AuthClaimModel(JwtClaimTypes.Role, SharedAppConfigDefinition.RoleSystemAdmin.Name),

                    // Extra
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.AuthMode, "local_custom"),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.RefreshTime.LocalUtc, now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                },

                CustomFieldWithStringValueModels = new List<AuthSharedUserCustomFieldWithStringValueModel>()
                {
                    new AuthSharedUserCustomFieldWithStringValueModel()
                    {
                        TenantId = TenantDefinitions.TenantTestId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        CustomFieldId = CustomFieldId_Test_TestId,
                        CustomFieldValueId = "831dd594-48cb-47ab-898f-eb38a8c6b3d3",
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        Value = "18"
                    },
                    new AuthSharedUserCustomFieldWithStringValueModel()
                    {
                        TenantId = TenantDefinitions.TenantVoidPointerId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        CustomFieldId = CustomFieldId_VP_TimeReportId,
                        CustomFieldValueId = "931dd594-48cb-47ab-898f-eb38a8c6b3d2",
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        Value = "18"
                    }
                },

                Roles = new List<AuthSharedUserRoleModel>()
                {
                    new AuthSharedUserRoleModel()
                    {
                        RoleId = SharedAppConfigDefinition.RoleSystemAdmin.RoleId,
                        RoleName = SharedAppConfigDefinition.RoleSystemAdmin.Name
                    }
                },

                IsMultiTenant = true
            };

            listOfUsers.Add(new CustomAuthUser() { UserName = "user2", Password = "pass2", AuthUser = authUser2 });

            var authUser3 = new AuthSharedUserModel
            {
                DisplayName = "Bertil Skanåker",
                FirstName = "Bertil",
                LastName = "Skanåker",
                UserId = "USR0003",
                Claims =
                {
                    // User Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantId, TenantDefinitions.TenantVoidPointerId),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.TenantInfo, tenantVPClaimValueJson),

                    // Role Claims
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_Mvc.ClaimValue),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.Activity, CustomCoreAppConfigDefinition.Activity_VP_Template_SimpleMvc.ClaimValue),

                    // Extra
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.AuthMode, "local_custom"),
                    new AuthClaimModel(AuthorizationConstants.JwtClaimTypes.RefreshTime.LocalUtc, now.ToString("yyyy-MM-dd HH:mm:ss.fff")),
                },

                CustomFieldWithStringValueModels = new List<AuthSharedUserCustomFieldWithStringValueModel>()
                {
                    new AuthSharedUserCustomFieldWithStringValueModel()
                    {
                        TenantId = TenantDefinitions.TenantVoidPointerId,
                        CustomFieldEntityId = AuthDefinitions.GetCustomFieldUserDefinitionId(),
                        CustomFieldId = CustomFieldId_VP_TimeReportId,
                        CustomFieldValueId = "231dd594-48cb-47ab-898f-eb38a8c6b3d1",
                        DataType = Auth.Contract.Types.CustomFieldDataType.Int,
                        FieldType = Auth.Contract.Types.CustomFieldType.Input,
                        Value = "21"
                    }
                },

                Roles = new List<AuthSharedUserRoleModel>(),

                IsMultiTenant = false
            };

            listOfUsers.Add(new CustomAuthUser() { UserName = "user3", Password = "pass3", AuthUser = authUser3 });

            return listOfUsers;
        }

        private string GetTenantClaimValueJson(string tenantId, string tenantName)
        {
            var configJsonConverter = new AuthContractJsonConverter();
            var tenantInfoHelper = new TenantInfoHelper(configJsonConverter);
            var tenantClaimValueJson = tenantInfoHelper.CreateTenantInfoClaimValueJsonFromObject(new TenantInfo()
            {
                Id = tenantId,
                Name = tenantName
            });

            return tenantClaimValueJson;
        }

        private List<AuthSharedUserBaseModel> GetAllActiveUsersInternal(string tenantId = null)
        {
            var listOfUsers = new List<AuthSharedUserBaseModel>();

            foreach (var user in CustomAuthUsers)
            {
                if (!string.IsNullOrEmpty(tenantId))
                {
                    if (user.AuthUser.Claims.Any(x => x.Type == AuthorizationConstants.JwtClaimTypes.TenantId && x.Value == tenantId))
                    {
                        listOfUsers.Add(user.AuthUser);
                    }
                }
                else
                {
                    listOfUsers.Add(user.AuthUser);
                }
            }

            return listOfUsers;
        }
    }

    public class CustomAuthUser
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public AuthSharedUserModel AuthUser { get; set; }
    }
}

