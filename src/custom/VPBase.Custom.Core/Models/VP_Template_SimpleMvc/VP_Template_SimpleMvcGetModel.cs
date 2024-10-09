using VPBase.Custom.Core.Data.Entities;
using VPBase.Shared.Core.Models;

namespace VPBase.Custom.Core.Models.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcGetModel : ReturnBaseModel
    {
        public string VP_Template_SimpleMvcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public VP_Template_SimpleMvcStatus Status { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUtcString => CreatedUtc.ToString("yyyy-MM-dd HH:mm:ss");
    }
}
