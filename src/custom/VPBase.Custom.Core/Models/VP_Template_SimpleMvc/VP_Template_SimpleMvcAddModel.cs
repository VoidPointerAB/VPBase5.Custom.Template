using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService;

namespace VPBase.Custom.Core.Models.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcAddModel : IVP_Template_SimpleMvcServiceValidationModel
    {
        public string VP_Template_SimpleMvcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VP_Template_SimpleMvcStatus Status { get; set; }
    }
}
