using VPBase.Shared.Core.Models;

namespace VPBase.Custom.Core.Models.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcGetModel : ReturnBaseModel
    {
        public string VP_Template_SimpleMvcId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
