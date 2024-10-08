﻿
/*═══════════════════════════════════════════════════════════════════════════════════════════╗
║ READONLY SAMPLE: CustomSample_DeveloperMenuLayer
╟────────────────────────────────────────────────────────────────────────────────────────────╢
║ Do NOT edit this file in your custom project.
║ When starting a new custom project, copy this file and modify your copy.
║ Update CustomStartupInstruction.cs so that it refers to your copy instead of this sample.
╚═══════════════════════════════════════════════════════════════════════════════════════════*/

using Microsoft.AspNetCore.Mvc;
using VPBase.Custom.Core.Definitions;
using VPBase.Shared.Server.Services.Menu;

namespace VPBase.Custom.Server.Configuration.MenuLayers
{
    public class CustomSample_DeveloperMenuLayer : MenuLayerBase
    {
        public override string LayerHandle => "CUSTOM_DEVELOPER";
        public override double LayerOrder => 10;

        public override bool IsApplicableForMenuIdentifier(string menuIdentifier)
        {
            return menuIdentifier == "DEVELOPER";
        }

        public override void Apply(SubMenu rootNode, IUrlHelper urlHelper)
        {
            rootNode.AddLink("Custom Mvc Template",
                urlHelper.Action("List", "VP_Template_Mvc", new { Area = "Custom" }),
                null,
                "fa-star").Policy(CustomCoreAppConfigDefinition.PolicyName_VP_Template_Mvc);

            rootNode.AddLink("Custom S.Mvc Template",
                urlHelper.Action("List", "VP_Template_SimpleMvc", new { Area = "Custom" }),
                null,
                "fa-star").Policy(CustomCoreAppConfigDefinition.PolicyName_VP_Template_SimpleMvc);
        }
    }
}