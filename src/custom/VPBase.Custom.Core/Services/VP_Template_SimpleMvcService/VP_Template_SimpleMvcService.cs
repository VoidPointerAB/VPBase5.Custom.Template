using Microsoft.EntityFrameworkCore;
using System.Linq;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Core.Models.VP_Template_SimpleMvc;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Models;
using VPBase.Shared.Core.Services;

namespace VPBase.Custom.Core.Services.VP_Template_SimpleMvcService
{
    public class VP_Template_SimpleMvcService : IAnonymizingService
    {
        private readonly ICustomStorage _storage;
        private readonly IDateTimeProvider _dateTimeProvider;

        public VP_Template_SimpleMvcService(ICustomStorage storage, IDateTimeProvider dateTimeProvider)
        {
            _storage = storage;
            _dateTimeProvider = dateTimeProvider;
        }

        public ServerResponse Add(VP_Template_SimpleMvcAddModel model, string tenantId)
        {
            var validator = new VP_Template_SimpleMvcServiceAddValidator(model);
            var errors = validator.Run();
            if (errors.Any())
            {
                return new ServerResponse { Errors = errors };
            }

            var newModel = new VP_Template_SimpleMvc(tenantId)
            {
                VP_Template_SimpleMvcId = model.VP_Template_SimpleMvcId,
                Title = model.Title,
                Description = model.Description,
                CreatedUtc = _dateTimeProvider.Now(),
                ModifiedUtc = _dateTimeProvider.Now()
            };

            _storage.Add(newModel);

            _storage.SaveChanges();

            return new ServerResponse();
        }

        public ServerResponse Update(VP_Template_SimpleMvcUpdateModel model, string tenantId)
        {
            var dbModel = _storage.VP_Template_SimpleMvcs.FirstOrDefault(x => !x.DeletedUtc.HasValue && x.VP_Template_SimpleMvcId == model.VP_Template_SimpleMvcId && x.TenantId == tenantId);

            var validator = new VP_Template_SimpleMvcServiceUpdateValidator(model, dbModel);
            var errors = validator.Run();
            if (errors.Any())
            {
                return new ServerResponse { Errors = errors };
            }

            dbModel.Title = model.Title;

            dbModel.Description = model.Description;
            dbModel.ModifiedUtc = _dateTimeProvider.Now();

            _storage.SaveChanges();

            return new ServerResponse();
        }

        public ServerResponse Delete(string id, string tenantId)
        {
            var response = new ServerResponse();

            var dbModel = _storage.VP_Template_SimpleMvcs.FirstOrDefault(x => x.VP_Template_SimpleMvcId == id && x.TenantId == tenantId);
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

            _storage.SaveChanges();

            return response;
        }

        public async Task AnonymizeItemsAsync(int itemShouldBeAnonymousAfterTheseDays, CancellationToken cancellationToken = default)
        {
            var anonymousOlderThanThis = _dateTimeProvider.Now().AddDays(-itemShouldBeAnonymousAfterTheseDays);
            var items = await _storage.VP_Template_SimpleMvcs.Where(x => x.DeletedUtc != null && x.DeletedUtc < anonymousOlderThanThis
                                                               && x.AnonymizedUtc == null).ToListAsync(cancellationToken);

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
