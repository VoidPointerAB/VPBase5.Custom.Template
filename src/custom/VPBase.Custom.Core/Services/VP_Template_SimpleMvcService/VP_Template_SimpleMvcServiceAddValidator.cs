using System.Collections.Generic;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService.ValidationRules;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_SimpleMvcService
{
    public class VP_Template_SimpleMvcServiceAddValidator : RuleValidator
    {
        public VP_Template_SimpleMvcServiceAddValidator(IVP_Template_SimpleMvcServiceValidationModel data)
        {
            _rules = new List<IValidationRule>
            {
                new RequiredFields(data),
                new TitleMaxLength(data),
            };
        }
    }
}
