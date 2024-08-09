using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VPBase.Shared.Core.Models.CustomField;

namespace VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_Mvc
{
    public class VP_Template_MvcAddOrEditViewModel : CustomFieldModel
    {
        public VP_Template_MvcAddOrEditViewModel()
        {
            Errors = new List<string>();
        }

        public string VP_Template_MvcId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime ModifiedUtcDate { get; set; }

        public ICollection<string> Errors { get; set; }

        public string Category { get; set; }
    }
}
