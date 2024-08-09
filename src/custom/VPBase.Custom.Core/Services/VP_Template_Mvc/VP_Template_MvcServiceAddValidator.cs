using System.Collections.Generic;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Services.VP_Template_Mvc.ValidationRules;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_Mvc
{
    public class VP_Template_MvcServiceAddValidator : RuleValidator
    {
        public VP_Template_MvcServiceAddValidator(IVP_Template_MvcValidationModel data, ICustomStorage storage)
        {
            _rules = new List<IValidationRule>
            {
                new RequiredFields(data),
                new TitleMaxLength(data),
                new TitleUnique(data, storage),
            };
        }
    }
}
