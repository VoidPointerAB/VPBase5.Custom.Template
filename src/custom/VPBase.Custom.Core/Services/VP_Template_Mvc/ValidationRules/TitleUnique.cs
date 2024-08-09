using System.Collections.Generic;
using VPBase.Custom.Core.Data;
using VPBase.Shared.Core.Helpers.Validation.Rules;
using static VPBase.Auth.Client.Code.ApiClients.GraphQL.Responses.UsersResponse;

namespace VPBase.Custom.Core.Services.VP_Template_Mvc.ValidationRules
{
    public class TitleUnique : IValidationRule
    {
        private readonly IVP_Template_MvcValidationModel _data;
        private readonly ICustomStorage _storage;

        public TitleUnique(IVP_Template_MvcValidationModel data, ICustomStorage storage)
        {
            _data = data;
            _storage = storage;
        }

        public void Run(ICollection<string> foundErrors)
        {
            if (_data.Title == null)
            {
                return;
            }

            var appWithNameAlready = _storage.VP_Template_Mvcs.FirstOrDefault(x =>
                x.Title.ToLower() == _data.Title.ToLower() &&
                x.TenantId == _data.TenantId &&
                x.DeletedUtc == null);

            if (appWithNameAlready != null)
            {
                foundErrors.Add(string.Format("Title must be unique"));
            }
        }
    }
}
