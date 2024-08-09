using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VPBase.Shared.Core.Data;

namespace VPBase.Custom.Core.Data.Entities
{
    [Table("VP_Template_Mvcs", Schema = "Custom.Sample")]
    public class VP_Template_Mvc : AnonymizeBaseEntity
    {
        public VP_Template_Mvc(string tenantId) : base(tenantId)
        {
        }

        [Key, Column(name: "VP_Template_MvcId")]
        public string VP_Template_MvcId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
