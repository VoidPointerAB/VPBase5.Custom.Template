using System.Collections.Generic;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_SimpleMvcService.ValidationRules
{
    public class RequiredFields : IValidationRule
    {
        private readonly IVP_Template_SimpleMvcServiceValidationModel _data;

        public RequiredFields(IVP_Template_SimpleMvcServiceValidationModel data)
        {
            _data = data;
        }

        public void Run(ICollection<string> foundErrors)
        {
            if (string.IsNullOrEmpty(_data.Title))
            {
                foundErrors.Add("Title is required");
            }
        }
    }
}
