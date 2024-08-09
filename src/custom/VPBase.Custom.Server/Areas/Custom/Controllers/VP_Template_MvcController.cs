using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VPBase.Custom.Core.Definitions;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_Mvc;
using VPBase.Custom.Server.Areas.Custom.WebAppServices;
using VPBase.Shared.Core.Types;
using VPBase.Shared.Server.Configuration;
using VPBase.Shared.Server.Helpers;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{
    [Area("Custom")]
    [Authorize(Policy = CustomCoreAppConfigDefinition.PolicyName_VP_Template_Mvc)]
    public class VP_Template_MvcController : Controller
    {
        private readonly VP_Template_MvcWebAppService _vp_Template_MvcWebAppService;
        private readonly AppSettings _appSettings;

        public VP_Template_MvcController(VP_Template_MvcWebAppService vp_Template_MvcWebAppService, AppSettings appSettings)
        {
            _vp_Template_MvcWebAppService = vp_Template_MvcWebAppService;
            _appSettings = appSettings;
        }

        public ActionResult List()
        {
            return View();
        }

        public async Task<ActionResult> Add()
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var model = await _vp_Template_MvcWebAppService.GetAddModelAsync(activeTenantId, HttpContext.RequestAborted);

            return View(model);
        }

        public async Task<ActionResult> Edit(string id)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var model = await _vp_Template_MvcWebAppService.GetEditModelAsync(id, activeTenantId, HttpContext.RequestAborted);

            if (model == null)
            {
                return LocalRedirect(ViewAdminHelper.BasePageError404Path);
            }

            return View("Edit", model);
        }

        public async Task<ActionResult> Show(string id)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var model = await _vp_Template_MvcWebAppService.GetShowModelAsync(id, activeTenantId, HttpContext.RequestAborted);
            if (model == null)
            {
                return LocalRedirect(ViewAdminHelper.BasePageError404Path);
            }

            return View("Show", model);
        }

        public async Task<ActionResult> GetVP_Template_MvcListItems()
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var items = await _vp_Template_MvcWebAppService.GetListAsync(0, 0, SortType.None, activeTenantId, HttpContext.RequestAborted);

            return Json(items);
        }

        [HttpPost]
        public async Task<ActionResult> SaveAdd(VP_Template_MvcAddOrEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activeTenantId = HttpContext.GetActiveTenantIdCookie();
                var serverResponse = await _vp_Template_MvcWebAppService.AddAsync(model, activeTenantId, HttpContext.RequestAborted);

                if (serverResponse.IsValid)
                {
                    return RedirectToAction("List");
                }

                model.Errors = serverResponse.Errors;
            }

            return View("Add", model);
        }

        [HttpPost]
        public async Task<ActionResult> SaveEdit(VP_Template_MvcAddOrEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var activeTenantId = HttpContext.GetActiveTenantIdCookie();
                var serverResponse = await _vp_Template_MvcWebAppService.EditAsync(model, activeTenantId, HttpContext.RequestAborted);

                if (serverResponse.IsValid)
                {
                    return RedirectToAction("List");
                }

                model.Errors = serverResponse.Errors;
            }
            
            return View("Edit", model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var activeTenantId = HttpContext.GetActiveTenantIdCookie();
            var serverResponse = await _vp_Template_MvcWebAppService.DeleteAsync(id, activeTenantId, HttpContext.RequestAborted);

            return Json(serverResponse);
        }

        [HttpPost]
        public async Task<ActionResult> ValidateTitle(string id, string title)
        {
            var serverResponse = await _vp_Template_MvcWebAppService.ValidateTitleAsync(id, title, HttpContext.GetActiveTenantIdCookie(), HttpContext.RequestAborted);

            return Json(new { isValid = serverResponse.IsValid, errors = serverResponse.Errors.ToArray() });
        }

        [HttpPost]
        public async Task<ActionResult> ValidateCategory(string id, string category)
        {
            var serverResponse = await _vp_Template_MvcWebAppService.ValidateCategoryAsync(id, category, HttpContext.GetActiveTenantIdCookie(), HttpContext.RequestAborted);

            return Json(new { isValid = serverResponse.IsValid, errors = serverResponse.Errors.ToArray() });
        }
    }
}
