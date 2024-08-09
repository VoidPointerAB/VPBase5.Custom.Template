using System.Collections.Generic;
using VPBase.Shared.Core.Helpers.Validation.Rules;

namespace VPBase.Custom.Core.Services.VP_Template_SimpleMvcService.ValidationRules
{
    public class TitleMaxLength : IValidationRule
    {
        private readonly IVP_Template_SimpleMvcServiceValidationModel _data;

        public TitleMaxLength(IVP_Template_SimpleMvcServiceValidationModel data)
        {
            _data = data;
        }

        public void Run(ICollection<string> foundErrors)
        {
            if (_data.Title == null)
            {
                return;
            }

            var allowedLength = 30;
            if (_data.Title.Length > allowedLength)
            {
                foundErrors.Add(string.Format("Title must be shorter than {0} characters", allowedLength));
            }
        }
    }
}
