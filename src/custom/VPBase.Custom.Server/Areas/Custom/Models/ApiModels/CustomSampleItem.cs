using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace VPBase.Custom.Server.Areas.Custom.Models.ApiModels
{
    public class CustomSampleItem
    {
        [Required]
        [DefaultValue(1)]
        public long Id { get; set; }

        [Required]
        [DefaultValue("Name")]
        public string Name { get; set; }

        [Required]
        [DefaultValue(true)]
        public bool IsComplete { get; set; }

        [DefaultValue("")]
        public string Description { get; set; }

        [DefaultValue("CustomProperty")]
        public string CustomProperty { get; set; }
    }
}
