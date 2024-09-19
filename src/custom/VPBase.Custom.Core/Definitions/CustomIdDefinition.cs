using VPBase.Shared.Core.Definitions;

namespace VPBase.Custom.Core.Definitions
{
    public class CustomIdDefinition
    {
        #region VP_Template

        public class VP_Template_Mvc : ISharedIdDefinition
        {
            public string Name  => "VPTemplateMvc";
            public string CounterId => "VPTM";
            public string FormatString => "VPTM{0:00000}";
        }

        public class VP_Template_SimpleMvc : ISharedIdDefinition
        {
            public string Name => "VPTemplateSimpleMvc";
            public string CounterId => "VPTSM";
            public string FormatString => "VPTSM{0:00000}";
        }

        #endregion

        public class CustomFieldValue : ISharedIdDefinition     // TEMP until BaseId definitions are replaced using ISharedIdDefinition!
        {
            public string Name => "CustomFieldValue";
            public string CounterId => "CFV";
            public string FormatString => "CFV{0:000000}";
        }
    }
}
