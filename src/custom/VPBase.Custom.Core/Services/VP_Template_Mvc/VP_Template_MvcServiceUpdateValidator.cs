using System.Collections.Generic;
using VPBase.Custom.Core.Models.VP_Template_Mvc;
using VPBase.Custom.Core.Services.VP_Template_Mvc.ValidationRules;
using VPBase.Shared.Core.Data;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_Mvc
{
    public class VP_Template_MvcServiceUpdateValidator : RuleValidator
    {
        public VP_Template_MvcServiceUpdateValidator(VP_Template_MvcUpdateModel data, BaseEntity storedEntity)
        {
            _rules = new List<IValidationRule>
            {
                new BaseEntityEditRule(storedEntity, data.ModifiedUtcDate),
                new RequiredFields(data),
                new TitleMaxLength(data),
            };
        }
    }
}
