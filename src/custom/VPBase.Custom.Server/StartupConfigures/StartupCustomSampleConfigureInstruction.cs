using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Generic;
using VPBase.Custom.Server.StartupConfigures.Applications;
using VPBase.Shared.Server.Code.Api;
using VPBase.Shared.Server.StartupConfigures;

namespace VPBase.Custom.Server.StartupConfigures
{
    public class StartupCustomSampleConfigureInstruction : IStartupConfigure
    {
        private const string ModuleName = "Custom";

        public void ApplySwaggerGenOptions(SwaggerGenOptions options)
        {
            // Apply hide in docs filter for the paths and schemas from base
            //options.DocumentFilter<CustomHideInDocsBaseApiFilter>();

            // Example: An extra "Custom Api Definition":
            options.SwaggerDoc(ModuleName.ToLower(), SwaggerHelper.GetOpenApiModuleInfo(ModuleName));
        }

        public void ApplySwaggerUiOptions(SwaggerUIOptions options)
        {
            // Example: An extra "Custom Api Definition Endpoint":
            options.SwaggerEndpoint("/swagger/" + ModuleName.ToLower() + "/swagger.json", "VPBase5 " + ModuleName + " API");
        }

        public IEnumerable<IStartupConfigureApplication> GetConfigureApplications()
        {
            var startupApplications = new List<IStartupConfigureApplication>
            {
                new DefaultSettingSampleStartupConfigureApplication()
            };

            return startupApplications;
        }

        public IEnumerable<IStartupConfigureService> GetConfigureServices()
        {
            return new List<IStartupConfigureService>();
        }
    }

    // This filter hides the BaseApi paths and schemas (the methods/models)
    public class CustomHideInDocsBaseApiFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            // Api Paths:
            foreach (var contextApiDescription in context.ApiDescriptions)
            {
                var actionDescriptor = (ControllerActionDescriptor)contextApiDescription.ActionDescriptor;

                if (actionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(SwaggerTagAttribute), true).Length == 0 &&
                    actionDescriptor.MethodInfo.GetCustomAttributes(typeof(SwaggerTagAttribute), true).Length == 0)
                {
                    var key = "/" + contextApiDescription.RelativePath.TrimEnd('/');
                    if (key.Contains(ApiResultHelper.BaseSampleApiName) ||          // "BaseSampleApi"
                        key.Contains(ApiResultHelper.BaseSampleAuthApiName))        // "BaseSampleAuthApi"
                    {
                        swaggerDoc.Paths.Remove(key);
                    }
                }
            }

            // Schemas/Models:
            var schemaKeysToRemove = new List<string>();
            IDictionary<string, OpenApiSchema> schemas = swaggerDoc.Components.Schemas;
            foreach (var schema in schemas)
            {
                if (schema.Key.Contains(ApiResultHelper.BaseApiSampleItemName)) // "BaseSampleItem"
                {
                    schemaKeysToRemove.Add(schema.Key);
                }
            }
            foreach(var schemaKeyToRemove in schemaKeysToRemove)
            {
                swaggerDoc.Components.Schemas.Remove(schemaKeyToRemove);
            }
        }
    }
}
