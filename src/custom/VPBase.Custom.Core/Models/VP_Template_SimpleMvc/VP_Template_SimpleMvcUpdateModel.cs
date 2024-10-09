using System;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Core.Models.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcUpdateModel : IVP_Template_SimpleMvcServiceValidationModel
    {
        public string VP_Template_SimpleMvcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VP_Template_SimpleMvcStatus Status { get; set; }

        public CrudMode CrudMode { get; set; }
        public DateTime ModifiedUtcDate { get; set; }
    }
}
