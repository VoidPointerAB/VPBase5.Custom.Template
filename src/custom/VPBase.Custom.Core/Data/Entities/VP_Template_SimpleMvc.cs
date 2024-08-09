using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBase.Shared.Core.Data;

namespace VPBase.Custom.Core.Data.Entities
{
    [Table("VP_Template_SimpleMvcs", Schema = "Custom.Sample")]
    public class VP_Template_SimpleMvc : AnonymizeBaseEntity
    {
        public VP_Template_SimpleMvc(string tenantId) : base(tenantId)
        {
        }

        [Key, Column(name: "VP_Template_SimpleMvcId")]
        public string VP_Template_SimpleMvcId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}
