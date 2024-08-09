using System.Collections.Generic;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_Mvc.ValidationRules
{
    public class RequiredFields : IValidationRule
    {
        private readonly IVP_Template_MvcValidationModel _data;

        public RequiredFields(IVP_Template_MvcValidationModel data)
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
