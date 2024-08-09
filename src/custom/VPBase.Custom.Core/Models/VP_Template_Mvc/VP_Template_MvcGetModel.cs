using System;
using System.Collections.Generic;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Models.CustomField;

namespace VPBase.Custom.Core.Models.VP_Template_Mvc
{
    public class VP_Template_MvcGetModel : CustomFieldInDataTableBaseModel
    {
        public string VP_Template_MvcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedUtc { get; set; }
        public DateTime ModifiedUtc { get; set; }
        public string Category { get; set; }
    }
}
