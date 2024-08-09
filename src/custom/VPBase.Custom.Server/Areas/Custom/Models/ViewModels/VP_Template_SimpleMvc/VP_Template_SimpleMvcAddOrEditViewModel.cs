using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcAddOrEditViewModel
    {
        public VP_Template_SimpleMvcAddOrEditViewModel()
        {
            Errors = new List<string>();
        }

        public string VP_Template_SimpleMvcId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ModifiedUtcDate { get; set; }

        public ICollection<string> Errors { get; set; }
    }
}
