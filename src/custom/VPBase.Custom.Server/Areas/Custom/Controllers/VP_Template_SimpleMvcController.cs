using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VPBase.Base.Server.Helpers;
using VPBase.Custom.Core.Definitions;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_SimpleMvc;
using VPBase.Custom.Server.Areas.Custom.WebAppServices;
using VPBase.Shared.Core.Types;
using VPBase.Shared.Server.Configuration;
using VPBase.Shared.Server.Helpers;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{
    [Area("Custom")]
    [Authorize(Policy = CustomCoreAppConfigDefinition.PolicyName_VP_Template_SimpleMvc)]
    public class VP_Template_SimpleMvcController : Controller
    {
        private readonly VP_Template_SimpleMvcWebAppService _vp_Template_SimpleMvcWebAppService;
        private readonly AppSettings _appSettings;

        public VP_Template_SimpleMvcController(VP_Template_SimpleMvcWebAppService vp_Template_SimpleMvcWebAppService, AppSettings appSettings)
        {
            _vp_Template_SimpleMvcWebAppService = vp_Template_SimpleMvcWebAppService;
            _appSettings = appSettings;
        }

        public ActionResult List(string title, string status, bool isActive, DateTime? startDate, DateTime? endDate)
        {
            return View(_vp_Template_SimpleMvcWebAppService.GetListModel(title, status, isActive, startDate, endDate));
        }

        public ActionResult Add()
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var model = _vp_Template_SimpleMvcWebAppService.GetAddModel(activeTenantId);

            model.ReturnUrl = ReturnUrlHelper.GetUrl(null, "/Custom/VP_Template_SimpleMvc/List", Request);

            return View(model);
        }

        public ActionResult Edit(string id)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var model = _vp_Template_SimpleMvcWebAppService.GetEditModel(id, activeTenantId);

            if (model == null)
            {
                return LocalRedirect(ViewAdminHelper.BasePageError404Path);
            }

            model.ReturnUrl = ReturnUrlHelper.GetUrl(null, "/Custom/VP_Template_SimpleMvc/List", Request);

            return View("Edit", model);
        }

        public ActionResult GetVP_Template_SimpleMvcListItems(string title, string status, bool isActive, DateTime? startDate, DateTime? endDate)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var items = _vp_Template_SimpleMvcWebAppService.GetList(title, status, isActive, startDate, endDate, 0, 0, SortType.None, activeTenantId);

            return Json(items);
        }

        [HttpPost]
        public ActionResult SaveAdd(VP_Template_SimpleMvcAddOrEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activeTenantId = HttpContext.GetActiveTenantIdCookie();
                var serverResponse = _vp_Template_SimpleMvcWebAppService.Add(model, activeTenantId);

                if (!serverResponse.Errors.Any())
                {
                    return Redirect(model.ReturnUrl);
                }

                model.Errors = serverResponse.Errors;
            }

            return View("Add", model);
        }

        [HttpPost]
        public ActionResult SaveEdit(VP_Template_SimpleMvcAddOrEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activeTenantId = HttpContext.GetActiveTenantIdCookie();
                var serverResponse = _vp_Template_SimpleMvcWebAppService.Edit(model, activeTenantId);

                if (!serverResponse.Errors.Any())
                {
                    return Redirect(model.ReturnUrl);
                }

                model.Errors = serverResponse.Errors;
            }

            return View("Edit", model);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var serverResponse = _vp_Template_SimpleMvcWebAppService.Delete(id, activeTenantId);

            return Json(serverResponse);
        }
    }
}
