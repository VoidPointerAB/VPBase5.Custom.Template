using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VPBase.Auth.Contract.Definitions;
using VPBase.Base.Server.Authorization.FiltersAndAttributes;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{

    [Route("api/custom/[controller]")]
    [ApiExplorerSettings(GroupName = "custom")] // Check group in: StartupCustomSampleConfigureInstruction
    [ApiController]
    public class CustomSampleAuthApiController : ControllerBase
    {
        /// <summary>
        /// Using 'ClaimRequirementAttribute' to ensure that the user/client is authenticated and has only ONE claim of type: 'tid'
        /// </summary>
        /// <returns></returns>
        [ClaimRequirement(AuthorizationConstants.JwtClaimTypes.TenantId, 1)]
        [HttpGet, Route("OnlyOneClaimTypeTenantId")]
        public string OnlyOneClaimTypeTenantId()
        {
            var userClaims = HttpContext.User.Claims.ToList();
            var tenantIdClaim = userClaims.First(x => x.Type == AuthorizationConstants.JwtClaimTypes.TenantId);  // First is now possible
            return "TentantIdValue: " + tenantIdClaim.Value;
        }

        /// <summary>
        /// Using 'SimpleBasicAuthRequirementAttribute' to ensure that the user/client is authenticated with basic auth with the specified username and password
        /// </summary>
        /// <returns></returns>
        [SimpleBasicAuthRequirement("user", "pass")]    // The client must authenticate with the username 'user' and the password 'pass' and basic auth
        [HttpGet, Route("SimplestBasicAuth")]
        public string SimplestBasicAuth()
        {
            return "Woho! Your'e in with basic auth!";
        }
    }
}
