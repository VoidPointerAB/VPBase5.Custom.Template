using System.Collections.Generic;
using VPBase.Custom.Core.Models.VP_Template_SimpleMvc;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService.ValidationRules;
using VPBase.Shared.Core.Data;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_SimpleMvcService
{
    public class VP_Template_SimpleMvcServiceUpdateValidator : RuleValidator
    {
        public VP_Template_SimpleMvcServiceUpdateValidator(VP_Template_SimpleMvcUpdateModel data, BaseEntity storedEntity)
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
