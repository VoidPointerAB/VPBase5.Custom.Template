using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Models.VP_Template_SimpleMvc;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_SimpleMvc;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Server.Areas.Custom.WebAppServices
{
    public class VP_Template_SimpleMvcWebAppService
    {
        private readonly ILogger _logger;
        private readonly ICustomStorage _storage;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly VP_Template_SimpleMvcService _vp_Template_SimpleMvcService;

        public VP_Template_SimpleMvcWebAppService(
            ICustomStorage storage,
            ILoggerFactory loggerFactory,
            IDateTimeProvider dateTimeProvider,
            VP_Template_SimpleMvcService vp_Template_SimpleMvcService)
        {
            _storage = storage;
            _dateTimeProvider = dateTimeProvider;
            _logger = loggerFactory.CreateLogger(GetType().Name);
            _vp_Template_SimpleMvcService = vp_Template_SimpleMvcService;
        }

        public VP_Template_SimpleMvcAddOrEditViewModel GetAddModel(string tenantId)
        {
            return new VP_Template_SimpleMvcAddOrEditViewModel
            {
                VP_Template_SimpleMvcId = ConfigIdHelper.GenerateUniqueId(),
                ModifiedUtcDate = _dateTimeProvider.Now()
            };
        }

        public VP_Template_SimpleMvcAddOrEditViewModel GetEditModel(string id, string tenantId)
        {
            var item = _storage.VP_Template_SimpleMvcs.FirstOrDefault(x =>
                x.DeletedUtc == null &&
                x.VP_Template_SimpleMvcId == id &&
                x.TenantId == tenantId);

            if (item == null)
            {
                return null;
            }

            return new VP_Template_SimpleMvcAddOrEditViewModel
            {
                VP_Template_SimpleMvcId = item.VP_Template_SimpleMvcId,
                Title = item.Title,
                Description = item.Description,
                ModifiedUtcDate = item.ModifiedUtc
            };
        }

        public ServerResponse<List<VP_Template_SimpleMvcGetModel>> GetList(int skip, int take, SortType sort, string tenantId)
        {
            var query = _storage.VP_Template_SimpleMvcs.Where(x => x.DeletedUtc == null && x.TenantId == tenantId)
                .Select(x => new VP_Template_SimpleMvcGetModel
                {
                    VP_Template_SimpleMvcId = x.VP_Template_SimpleMvcId,
                    Title = x.Title,
                    Description = x.Description,
                    CreatedUtc = x.CreatedUtc,
                    ModifiedUtc = x.ModifiedUtc,
                });

            if (sort != SortType.None)
            {
                switch (sort)
                {
                    case SortType.CreatedUtc_Asc: query = query.OrderBy(x => x.CreatedUtc); break;
                    case SortType.CreatedUtc_Desc: query = query.OrderByDescending(x => x.CreatedUtc); break;
                    default: query = query.OrderBy(x => x.Title); break;
                }
            }

            if (skip > 0)
            {
                query = query.Skip(skip);
            }
            if (take > 0)
            {
                query = query.Take(take);
            }

            return new ServerResponse<List<VP_Template_SimpleMvcGetModel>>
            {
                Data = query.ToList()
            };
        }

        public ServerResponse Add(VP_Template_SimpleMvcAddOrEditViewModel data, string tenantId)
        {
            var response = new ServerResponse();

            try
            {
                response = _vp_Template_SimpleMvcService.Add(new VP_Template_SimpleMvcAddModel
                {
                    VP_Template_SimpleMvcId = data.VP_Template_SimpleMvcId,
                    Description = data.Description,
                    Title = data.Title
                }, tenantId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }

        public ServerResponse Edit(VP_Template_SimpleMvcAddOrEditViewModel data, string tenantId)
        {
            var response = new ServerResponse();

            try
            {
                response = _vp_Template_SimpleMvcService.Update(new VP_Template_SimpleMvcUpdateModel
                {
                    VP_Template_SimpleMvcId = data.VP_Template_SimpleMvcId,
                    Description = data.Description,
                    Title = data.Title,
                    CrudMode = CrudMode.AddUpdateDelete,
                    ModifiedUtcDate = data.ModifiedUtcDate
                }, tenantId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }

        public ServerResponse Delete(string id, string tenantId)
        {
            var response = new ServerResponse();

            try
            {
                response = _vp_Template_SimpleMvcService.Delete(id, tenantId);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }
    }
}
