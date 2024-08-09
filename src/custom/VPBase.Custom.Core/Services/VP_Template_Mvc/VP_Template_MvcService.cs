using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Models.VP_Template_Mvc;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Services;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Core.Services.VP_Template_Mvc
{
    public class VP_Template_MvcService : IAnonymizingService
    {
        private readonly ICustomStorage _storage;
        private readonly ICustomFieldValueService _customFieldValueService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IInstanceContractBaseCreator _instanceContractBaseCreator;

        public VP_Template_MvcService(ICustomStorage storage,
            ICustomFieldValueService customFieldValueService, 
            IDateTimeProvider dateTimeProvider,
            CustomDbTransactionHandler customDbTransactionHandler,
            IInstanceContractBaseCreator instanceContractBaseCreator)
        {
            _storage = storage;
            _customFieldValueService = customFieldValueService;
            _dateTimeProvider = dateTimeProvider;
            _instanceContractBaseCreator = instanceContractBaseCreator;
        }

        public async Task<ServerResponse> AddAsync(VP_Template_MvcAddModel model, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            var validator = new VP_Template_MvcServiceAddValidator(model, _storage);
            var errors = validator.Run();
            if (errors.Any())
            {
                response.Errors = errors;
                return response;
            }

            var newModel = new Data.Entities.VP_Template_Mvc(model.TenantId)
            {
                VP_Template_MvcId = model.VP_Template_MvcId,
                Title = model.Title,
                Category = model.Category,
                Description = model.Description,
                CreatedUtc = _dateTimeProvider.Now(),
                ModifiedUtc = _dateTimeProvider.Now()
            };

            var baseDbTransactionHandler = _instanceContractBaseCreator.CreateBaseDbTransactionHandler();
            IDbContextTransaction dbContextTransaction = null;
            IDbContextTransaction dbBaseContextTransaction = null;

            try
            {
                using (dbBaseContextTransaction = await baseDbTransactionHandler.BeginTransactionAsync(cancellationToken))
                using (dbContextTransaction = await _storage.BeginTransactionAsync(cancellationToken))
                {
                    await _customFieldValueService.AddValuesForEntityAsync(
                        model.CustomFieldValues,
                        model.VP_Template_MvcId,
                        model.TenantId,
                        baseDbTransactionHandler,
                        cancellationToken);

                    await _storage.AddAsync(newModel);

                    await baseDbTransactionHandler.SaveChangesAsync(cancellationToken);
                    await _storage.SaveChangesAsync(cancellationToken);

                    await dbBaseContextTransaction.CommitAsync(cancellationToken);
                    await dbContextTransaction.CommitAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                dbBaseContextTransaction?.RollbackAsync(cancellationToken);
                dbContextTransaction?.RollbackAsync(cancellationToken);
                throw;
            }

            return response;
        }

        public async Task<ServerResponse> UpdateAsync(VP_Template_MvcUpdateModel model, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            var dbModel = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x => !x.DeletedUtc.HasValue && x.VP_Template_MvcId == model.VP_Template_MvcId && 
                                                                                    x.TenantId == model.TenantId, cancellationToken);

            var validator = new VP_Template_MvcServiceUpdateValidator(model, dbModel);
            var errors = validator.Run();
            if (errors.Any())
            {
                return new ServerResponse { Errors = errors };
            }

            var baseDbTransactionHandler = _instanceContractBaseCreator.CreateBaseDbTransactionHandler();
            IDbContextTransaction dbContextTransaction = null;
            IDbContextTransaction dbBaseContextTransaction = null;

            try
            {
                using (dbBaseContextTransaction = await baseDbTransactionHandler.BeginTransactionAsync(cancellationToken))
                using (dbContextTransaction = await _storage.BeginTransactionAsync(cancellationToken))
                {
                    await _customFieldValueService.UpdateValuesForEntityAsync(
                                model.CustomFieldValues,
                                CrudMode.AddUpdateDelete,
                                model.VP_Template_MvcId,
                                model.TenantId,
                                baseDbTransactionHandler, cancellationToken);

                    dbModel.Title = model.Title;

                    dbModel.Category = model.Category;

                    dbModel.Description = model.Description;
                    dbModel.ModifiedUtc = _dateTimeProvider.Now();

                    await baseDbTransactionHandler.SaveChangesAsync(cancellationToken);
                    await _storage.SaveChangesAsync(cancellationToken);

                    await dbBaseContextTransaction.CommitAsync(cancellationToken);
                    await dbContextTransaction.CommitAsync(cancellationToken);
                }
            }
            catch (Exception)
            {
                dbBaseContextTransaction?.RollbackAsync(cancellationToken);
                dbContextTransaction?.RollbackAsync(cancellationToken);
                throw;
            }

            return response;
        }

        public async Task<ServerResponse> DeleteAsync(string id, string tenantId, CancellationToken cancellationToken = default)
        {
            var response = new ServerResponse();

            var dbModel = await _storage.VP_Template_Mvcs.FirstOrDefaultAsync(x => x.VP_Template_MvcId == id && x.TenantId == tenantId, cancellationToken);
            if (dbModel == null)
            {
                response.Errors.Add(StandardErrors.EntityNotFound);
                return response;
            }

            if (dbModel.DeletedUtc.HasValue)
            {
                return response;
            }

            var deletedDate = _dateTimeProvider.Now();
            dbModel.DeletedUtc = deletedDate;
            dbModel.ModifiedUtc = deletedDate;

            await _storage.SaveChangesAsync(cancellationToken);

            return response;
        }

        public async Task AnonymizeItemsAsync(int itemShouldBeAnonymousAfterTheseDays, CancellationToken cancellationToken = default)
        {
            var anonymousOlderThanThis = _dateTimeProvider.Now().AddDays(-itemShouldBeAnonymousAfterTheseDays);
            var items = await _storage.VP_Template_Mvcs.Where(x => x.DeletedUtc != null && 
                                                                    x.DeletedUtc < anonymousOlderThanThis && 
                                                                    x.AnonymizedUtc == null).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                item.Title = "-";
                item.ModifiedUtc = _dateTimeProvider.Now();
                item.AnonymizedUtc = _dateTimeProvider.Now();
            }

            await _storage.SaveChangesAsync(cancellationToken);
        }
    }
}
