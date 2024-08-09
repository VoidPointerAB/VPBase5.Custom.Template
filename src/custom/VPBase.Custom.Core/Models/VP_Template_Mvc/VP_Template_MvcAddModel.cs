using System.Collections.Generic;
using VPBase.Custom.Core.Services.VP_Template_Mvc;
using VPBase.Shared.Core.Models.CustomField;

namespace VPBase.Custom.Core.Models.VP_Template_Mvc
{
    public class VP_Template_MvcAddModel : IVP_Template_MvcValidationModel
    {
        public VP_Template_MvcAddModel()
        {
            CustomFieldValues = new List<AddCustomFieldValueModel>();
        }

        public string VP_Template_MvcId { get; set; }
        public string TenantId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<AddCustomFieldValueModel> CustomFieldValues { get; set; }
        public string Category { get; set; }
    }
}
