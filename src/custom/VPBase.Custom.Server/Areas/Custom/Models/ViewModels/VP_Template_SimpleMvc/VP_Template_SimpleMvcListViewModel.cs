using System.Runtime.InteropServices.JavaScript;
using Microsoft.AspNetCore.Mvc.Rendering;
using VPBase.Custom.Core.Data.Entities;

namespace VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_SimpleMvc
{
    public class VP_Template_SimpleMvcListViewModel
    {
        public VP_Template_SimpleMvcListViewModel()
        {
            StatusSelect = new List<SelectListItem>();
        }

        public string TitleFilter { get; set; }
        public string StatusFilter { get; set; }
        public bool IsActiveFilter { get; set; }
        public DateTime? StartDateFilter { get; set; }
        public DateTime? EndDateFilter { get; set; }

        public IList<SelectListItem> StatusSelect { get; set; }
    }
}
