using System.Reflection;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Services;
using VPBase.Shared.Core.SharedImplementations.AuthContract;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using VPBase.Shared.Core.Models.TenantMigration;
using VPBase.Shared.Core.Helpers;
using VPBase.Custom.Core.Definitions;

namespace VPBase.Custom.Core.Services.TenantMigrationServices
{
    public class CustomSampleTenantMigrationService : ITenantMigrationService
    {
        private ICustomStorage _customStorage;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEnumerable<ITenantMigrationEntityService> _tenantMigrationServices;
        private readonly SharedIdHelper _idHelper;
        private readonly ICustomFieldValueTenantMigrationService _customFieldValueTenantMigrationService;

        public CustomSampleTenantMigrationService(ICustomStorage customStorage,
            IDateTimeProvider dateTimeProvider,
            IEnumerable<ITenantMigrationEntityService> tenantMigrationEntityServices, 
            SharedIdHelper idHelper,
            ICustomFieldValueTenantMigrationService customFieldValueTenantMigrationService)
        {
            _customStorage = customStorage;
            _dateTimeProvider = dateTimeProvider;
            _tenantMigrationServices = tenantMigrationEntityServices;
            _idHelper = idHelper;
            _customFieldValueTenantMigrationService = customFieldValueTenantMigrationService;
        }

        #region IConfigSortable

        public string GetModuleName()
        {
            return ConfigModuleConstants.Custom;
        }

        public string GetName()
        {
            return MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public double GetSortOrder()
        {
            return 10;
        }

        #endregion

        #region ITenantMigrationService

        public string Export(string tenantId, TenantMigrationExportFilter tenantMigrationExportFilter)
        {
            var specification = new CustomTenantMigrationSpecification
            {
                Version = "0.1",
                GeneratedTimeStamp = _dateTimeProvider.Now()
            };

            // Export: VP_Template_Mvc
            if (tenantMigrationExportFilter.CheckExport(_idHelper.GetName<CustomIdDefinition.VP_Template_Mvc>()))
            {
                specification.Mvcs = _customStorage.VP_Template_Mvcs
                    .Where(x => x.TenantId == tenantId && x.DeletedUtc == null)
                    .Select(x => new VP_Template_MvcMigrationModel
                    {
                        Id = x.VP_Template_MvcId,
                        Title = x.Title,
                        Description = x.Description,
                        Category = x.Category,
                    }).ToList();

                // Export: Custom field values VP_Template_Mvc
                if (tenantMigrationExportFilter.CheckExport(_customFieldValueTenantMigrationService.EntityName))
                {                   
                    _customFieldValueTenantMigrationService.Apply(specification.Mvcs, CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId, tenantId);
                }         
            }

            // Export: VP_Template_SimpleMvc
            if (tenantMigrationExportFilter.CheckExport(_idHelper.GetName<CustomIdDefinition.VP_Template_SimpleMvc>()))
            {
                specification.SimpleMvcs = _customStorage.VP_Template_SimpleMvcs
                    .Where(x => x.TenantId == tenantId && x.DeletedUtc == null)
                    .Select(x => new VP_Template_SimpleMvcMigrationModel
                    {
                        Id = x.VP_Template_SimpleMvcId,
                        Title = x.Title,
                        Description = x.Description,
                    }).ToList();
            }

            // Export: Registered base services
            foreach (var tenantMigrationService in _tenantMigrationServices)
            {
                if (tenantMigrationExportFilter.CheckExport(tenantMigrationService.EntityName))
                {
                    tenantMigrationService.Export(tenantId, specification);
                }
            }

            var json = SerializeSpecificationToJson(specification);

            json = SharedTenantMigrationHelper.GetExportJsonReplacement(json, tenantId);

            return json;
        }

        public TenantMigrationImportResult Import(string tenantId, string jsonData)
        {
            var result = new TenantMigrationImportResult();

            var currentTime = _dateTimeProvider.Now();

            jsonData = SharedTenantMigrationHelper.GetImportJsonReplacement(jsonData, tenantId);

            var specification = DeserializeJsonToSpecification(jsonData);

            // Import: Mvcs
            var importMvcResult = ImportMvcs(specification.Mvcs, tenantId, currentTime);
            result.AddLogItems(importMvcResult.LogItems);

            // Import: Simple Mvcs
            var importSimpleMvcResult = ImportSimpleMvcs(specification.SimpleMvcs, tenantId, currentTime);
            result.AddLogItems(importMvcResult.LogItems);

            if (result.Success)
            {
                _customStorage.SaveChanges();
            }

            // Import: Registered base services
            foreach (var tenantMigrationService in _tenantMigrationServices)
            {
                var importEntityResult = tenantMigrationService.Import(tenantId, jsonData);
                result.AddLogItems(importEntityResult.LogItems);
            }

            // Import: Custom Field Values - Mvcs
            _customFieldValueTenantMigrationService.Import(tenantId, specification.Mvcs, CustomCoreAppConfigDefinition.CustomFieldEntityVP_Template_MvcDefinition.CustomFieldEntityDefinitionId);

            return result;
        }

        public void Exterminate(string tenantId)
        {
            var db = ((DbContext)_customStorage).Database;

            // Exterminate: VP_Template_Mvc and VP_Template_SimpleMvc
            db.ExecuteSql(FormattableStringFactory.Create(
                $"DELETE FROM {"[Custom.Sample].VP_Template_SimpleMvcs"} WHERE TenantId='{tenantId}'"));

            db.ExecuteSql(FormattableStringFactory.Create(
                $"DELETE FROM {"[Custom.Sample].VP_Template_Mvcs"} WHERE TenantId='{tenantId}'"));

            // Exterminate: Registered base services
            foreach (var tenantMigrationService in _tenantMigrationServices)
            {
                tenantMigrationService.Delete(tenantId);
            }

            // Exterminate: Custom field values
            _customFieldValueTenantMigrationService.Delete(tenantId);
        }

        public TenantMigrationInfo GetInfo(string tenantId)
        {
            var info = new TenantMigrationInfo(tenantId);

            // Info: VP_Template_Mvc
            info.AvailableEntities.Add(new TenantMigrationEntityInfo()
            {
                Name = _idHelper.GetName<CustomIdDefinition.VP_Template_Mvc>(),
                Description = "VP Template Mvcs",
                Count = _customStorage.VP_Template_Mvcs.Count(c => c.TenantId == tenantId && c.DeletedUtc == null)
            });

            // Info: VP_Template_SimpleMvc
            info.AvailableEntities.Add(new TenantMigrationEntityInfo()
            {
                Name = _idHelper.GetName<CustomIdDefinition.VP_Template_SimpleMvc>(),
                Description = "VP Template Simple Mvcs",
                Count = _customStorage.VP_Template_SimpleMvcs.Count(c => c.TenantId == tenantId && c.DeletedUtc == null)
            });

            // Info: Registered base services
            foreach (var tenantMigrationService in _tenantMigrationServices)
            {
                info.AvailableEntities.Add(tenantMigrationService.GetMigrationEntity(tenantId));
            }

            // Info: Custom field values
            info.AvailableEntities.Add(_customFieldValueTenantMigrationService.GetMigrationEntity(tenantId));

            return info;
        }

        #endregion

        public string SerializeSpecificationToJson(CustomTenantMigrationSpecification specification)
        {
            var authContractJsonConverter = AuthContractJsonConverterFactory.Create();
            var json = authContractJsonConverter.SerializeObject(specification);
            return json;
        }

        public CustomTenantMigrationSpecification DeserializeJsonToSpecification(string jsonData)
        {
            var authContractJsonConverter = AuthContractJsonConverterFactory.Create();
            var specification = authContractJsonConverter.DeserializeObject<CustomTenantMigrationSpecification>(jsonData);
            return specification;
        }

        private TenantMigrationImportResult ImportMvcs(IEnumerable<VP_Template_MvcMigrationModel> models, string tenantId, DateTime currentTime)
        {
            var result = new TenantMigrationImportResult();
            var entityName = _idHelper.GetName<CustomIdDefinition.VP_Template_Mvc>();

            try
            {
                result.AddFriendlyMessage($"Started importing", entityName);

                foreach (var model in models)
                {
                    var existingMvc = _customStorage.VP_Template_Mvcs.FirstOrDefault(c => c.VP_Template_MvcId == model.Id && c.TenantId == tenantId);

                    if (existingMvc != null)
                    {
                        existingMvc.Title = model.Title;
                        existingMvc.Description = model.Description;
                        existingMvc.Category = model.Category;
                        existingMvc.ModifiedUtc = currentTime;
                    }
                    else
                    {
                        var newMvc = new Data.Entities.VP_Template_Mvc(tenantId)
                        {
                            VP_Template_MvcId = model.Id,
                            Title = model.Title,
                            Description = model.Description,
                            Category = model.Category,
                            CreatedUtc = currentTime,
                            ModifiedUtc = currentTime
                        };
                        _customStorage.Add(newMvc);
                    }
                }

                result.AddFriendlyMessage($"Finished importing", entityName);
            }
            catch (Exception ex)
            {
                result.AddException(ex, ex.Message, entityName);
            }

            return result;
        }

        private TenantMigrationImportResult ImportSimpleMvcs(IEnumerable<VP_Template_SimpleMvcMigrationModel> models, string tenantId, DateTime currentTime)
        {
            var result = new TenantMigrationImportResult();
            var entityName = _idHelper.GetName<CustomIdDefinition.VP_Template_SimpleMvc>();

            try
            {
                result.AddFriendlyMessage($"Started importing", entityName);

                foreach (var model in models)
                {
                    var existingSimpleMvc = _customStorage.VP_Template_SimpleMvcs.FirstOrDefault(c => c.VP_Template_SimpleMvcId == model.Id && c.TenantId == tenantId);

                    if (existingSimpleMvc != null)
                    {
                        existingSimpleMvc.Title = model.Title;
                        existingSimpleMvc.Description = model.Description;
                        existingSimpleMvc.ModifiedUtc = currentTime;
                    }
                    else
                    {
                        var newSimpleMvc = new VP_Template_SimpleMvc(tenantId)
                        {
                            VP_Template_SimpleMvcId = model.Id,
                            Title = model.Title,
                            Description = model.Description,
                            CreatedUtc = currentTime,
                            ModifiedUtc = currentTime
                        };
                        _customStorage.Add(newSimpleMvc);
                    }
                }

                result.AddFriendlyMessage($"Finished importing", entityName);
            }
            catch (Exception ex)
            {
                result.AddException(ex, ex.Message, entityName);
            }

            return result;
        }
    }
}
