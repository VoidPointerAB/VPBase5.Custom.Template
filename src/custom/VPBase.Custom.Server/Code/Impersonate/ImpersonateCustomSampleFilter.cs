using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Reflection;
using VPBase.Auth.Contract.Models;
using VPBase.Base.Server.Code.Impersonate;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Server.Services;

namespace VPBase.Custom.Server.Code.Impersonate
{
    public class ImpersonateCustomSampleFilter : ImpersonateBaseFilter, IImpersonateFilter
    {
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
            return 10;      // Sortorder should be 10 - 49 in Base Custom implementations.
        }

        #endregion

        public List<AuthSharedUserBaseModel> Filter(ImpersonateFilterParams impersonateFilterParams)
        {
            // Example: hide multitenant users
            // var users = impersonateFilterParams.ActiveUsers.Where(x => x.IsMultiTenant == false).ToList();

            // var roles = impersonateFilterParams.User.Roles();  

            // Check all params to make the filter

            var users = impersonateFilterParams.ActiveUsers;
            return users;
        }

        public new string GetImpersonateDisplayOutputString(AuthSharedUserBaseModel user, IEnumerable<AuthCustomFieldModel> availableUserCustomFields)
        {
            // Example: (more simpler)
            // return $"{user.DisplayName} ({user.FirstName} {user.LastName})";  

            return base.GetImpersonateDisplayOutputString(user, availableUserCustomFields);
        }

        public void AfterSuccessLogin(string userId, HttpContext httpContext)
        {
            // Do nothing, never throw an exception here!
        }
    }
}