using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Definitions;
using VPBase.Custom.Core.Models.VP_Template_Mvc;
using VPBase.Custom.Core.Services.VP_Template_Mvc;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_Mvc;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Services;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Server.Areas.Custom.WebAppServices
{
    public class VP_Template_MvcWebAppService
    {
        private readonly ICustomStorage _storage;
        private readonly ILogger _logger;
        private readonly ICustomFieldValueService _customFieldValueService;
        private readonly VP_Template_MvcService _vp_Template_MvcService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VP_Template_MvcWebAppService(
            ICustomStorage storage,
            ILogger<VP_Template_MvcWebAppService> logger,
            ICustomFieldValueService customFieldValueService,
            VP_Template_MvcService vp_Template_MvcService,
            IDateTimeProvider dateTimeProvider)
        {
            _storage = storage;
            _logger = logger;
            _customFieldValueService = customFieldValueService;
            _vp_Template_MvcService = vp_Template_MvcService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<VP_Template_MvcAddOrEditViewModel> GetAddModelAsync(string tenantId, CancellationToken cancellationToken = default)
        {
            string uniqueId;

            do
            {
                uniqueId = ConfigIdHelper.GenerateUniqueId();
            } while (await _storage.VP_Template_Mvcs.AnyAsync(x => x.VP_Template_MvcId == uniqueId, cancellationToken));

            return new VP_Template_MvcAddOrEditViewModel
            {
                VP_Template_MvcId = uniqueId,
                ModifiedUtcDate = _dateTimeProvider.Now(),
                CustomFieldWithValues = await _customFieldValueService.GetCustomFieldsWithValuesAsync(null,
                    CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId,
                    tenantId, null, cancellationToken),
                CustomFieldEntityId = CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId
            };
        }

        public async Task<VP_Template_MvcAddOrEditViewModel> GetEditModelAsync(string id, string tenantId, CancellationToken cancellationToken = default)
        {
            var item = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x =>
                x.DeletedUtc == null &&
                x.VP_Template_MvcId == id &&
                x.TenantId == tenantId, cancellationToken);

            if (item == null)
            {
                return null;
            }

            return new VP_Template_MvcAddOrEditViewModel
            {
                VP_Template_MvcId = item.VP_Template_MvcId,
                Title = item.Title,
                Description = item.Description,
                Category = item.Category,
                ModifiedUtcDate = item.ModifiedUtc,
                CustomFieldWithValues = await _customFieldValueService.GetCustomFieldsWithValuesAsync(id,
                    CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId,
                    tenantId, null, cancellationToken),
                CustomFieldEntityId = CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId
            };
        }

        public async Task<VP_Template_MvcShowViewModel> GetShowModelAsync(string id, string tenantId, CancellationToken cancellationToken = default)
        {
            var item = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x =>
                x.DeletedUtc == null &&
                x.VP_Template_MvcId == id &&
                x.TenantId == tenantId, cancellationToken);

            if (item == null)
            {
                return null;
            }

            return new VP_Template_MvcShowViewModel
            {
                VP_Template_MvcId = item.VP_Template_MvcId,
                Title = item.Title,
                Description = item.Description,
                Category = item.Category,
                ModifiedUtcDate = item.ModifiedUtc,
                CustomFieldWithValues = await _customFieldValueService.GetCustomFieldsWithValuesAsync(id,
                    CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId,
                    tenantId, null, cancellationToken),
                CustomFieldEntityId = CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId
            };
        }

        public async Task<ServerResponse<List<VP_Template_MvcGetModel>>> GetListAsync(int skip, int take, SortType sort, string tenantId, CancellationToken cancellationToken = default)
        {
            var query = _storage.VP_Template_Mvcs.Where(x => x.DeletedUtc == null && x.TenantId == tenantId)
                .Select(x => new VP_Template_MvcGetModel
                {
                    VP_Template_MvcId = x.VP_Template_MvcId,
                    Title = x.Title,
                    Category = x.Category,
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

            var list = await query.ToListAsync(cancellationToken);


            #region Supports customfields in list

            var customFieldValues = await _customFieldValueService.GetCustomFieldsWithStringValuesAsync(
                CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId, // Change to correct entity type id
                tenantId,
                null,
                cancellationToken);

            Parallel.ForEach(list, item =>
            {
                item.CustomFieldWithValues =
                    customFieldValues.Where(x => x.EntityId == item.VP_Template_MvcId)
                        .ToList(); // Change to correct entity id
            });

            #endregion

            return new ServerResponse<List<VP_Template_MvcGetModel>>
            {
                Data = list
            };
        }

        public async Task<ServerResponse> AddAsync(VP_Template_MvcAddOrEditViewModel data, string tenantId, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            data.CustomFieldWithValues.ForEach(x =>
            {
                x.CustomFieldValueId = Guid.NewGuid().ToString();
            });

            try
            {
                response = await _vp_Template_MvcService.AddAsync(new VP_Template_MvcAddModel
                {
                    VP_Template_MvcId = data.VP_Template_MvcId,
                    Description = data.Description,
                    Title = data.Title,
                    Category = data.Category,
                    TenantId = tenantId,
                    CustomFieldValues = _customFieldValueService.ConvertToSaveList(data.CustomFieldWithValues)  // Async not needed!
                }, cancellationToken);

                if (!response.IsValid)
                {
                    return response;
                }   
            }
            catch (TransactionAbortedException e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.TransactionAbortedError);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }

        public async Task<ServerResponse> EditAsync(VP_Template_MvcAddOrEditViewModel data, string tenantId, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            try
            {
                response = await _vp_Template_MvcService.UpdateAsync(new VP_Template_MvcUpdateModel
                {
                    VP_Template_MvcId = data.VP_Template_MvcId,
                    Description = data.Description,
                    Title = data.Title,
                    Category = data.Category,
                    CustomFieldValues = _customFieldValueService.ConvertToSaveList(data.CustomFieldWithValues),     // Async not needed!
                    ModifiedUtcDate = data.ModifiedUtcDate,
                    TenantId = tenantId
                }, cancellationToken);

                if (!response.IsValid)
                {
                    return response;
                }

                return response;
            }
            catch (TransactionAbortedException e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.TransactionAbortedError);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }

        public async Task<ServerResponse> DeleteAsync(string id, string tenantId, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            try
            {
                response = await _vp_Template_MvcService.DeleteAsync(id, tenantId, cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                response.Errors.Add(StandardErrors.UnexpectedServerError);
            }

            return response;
        }


        public async Task<ServerResponse> ValidateTitleAsync(string id, string title, string tenantId, CancellationToken cancellationToken = default)
        {
            var appWithNameAlready = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x =>
                x.Title.ToLower() == title.ToLower() &&
                (id == null || x.VP_Template_MvcId != id) &&
                x.TenantId == tenantId &&
                x.DeletedUtc == null, cancellationToken);

            if (appWithNameAlready == null)
            {
                return new ServerResponse() { Errors = new List<string>() { } };
            }

            return new ServerResponse() { Errors = new List<string>() { "This name already exists" } };

        }


        public async Task<ServerResponse> ValidateCategoryAsync(string id, string category, string tenantId, CancellationToken cancellationToken = default)
        {
            var appWithNameAlready = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x =>
                x.Category.ToLower() == category.ToLower() &&
                (id == null || x.VP_Template_MvcId != id) &&
                x.TenantId == tenantId &&
                x.DeletedUtc == null, cancellationToken);

            if (appWithNameAlready == null)
            {
                return new ServerResponse() { Errors = new List<string>() { "No existing category by this name" } };
            }

            return new ServerResponse() { Errors = new List<string>() { } };

        }
    }
}


