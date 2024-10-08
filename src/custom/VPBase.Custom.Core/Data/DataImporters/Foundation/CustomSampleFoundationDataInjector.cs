﻿/*═══════════════════════════════════════════════════════════════════════════════════════════╗
║ READONLY SAMPLE: CustomSampleFoundationDataInjector
╟────────────────────────────────────────────────────────────────────────────────────────────╢
║ Do NOT edit this file in your custom project.
║ When starting a new custom project, copy this file and name the copy
║ "CustomFoundationDataInjector". Insert your custom project´s foundation data into that copy.
║ Update CustomStartupInstruction.cs so that it refers to your copy instead of this sample.
╚═══════════════════════════════════════════════════════════════════════════════════════════*/

using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.Definitions;
using VPBase.Custom.Core.Definitions;
using VPBase.Shared.Core.Configuration;
using VPBase.Shared.Core.Data.Foundation;
using VPBase.Shared.Core.Definitions;
using VPBase.Shared.Core.Services;
using VPBase.Shared.Core.Types;
using static VPBase.Shared.Core.Definitions.Settings.SettingDataTypeDefinition;

namespace VPBase.Custom.Core.Data.DataImporters.Foundation
{
    public class CustomSampleFoundationDataInjector : IFoundationDataInjector
    {
        #region IConfigSortable

        public string GetModuleName()
        {
            return ConfigModuleConstants.Custom;
        }

        public string GetName()
        {
            return MethodBase.GetCurrentMethod().DeclaringType.Name;
        }

        public double GetSortOrder()
        {
            return 10;      // Sortorder should be 10 - 49 in Base Custom implementations.
        }

        #endregion

        public void Inject(IServiceProvider serviceProvider, IConfigEntities configEntities, ILogger logger, DateTime baselineDate)
        {
            // Use a foundation data injector for populating registries etc. that are identical for all environments.

            var fileTransformationService = serviceProvider.GetService<IFileTransformationService>();

            var settingEntitySettings = serviceProvider.GetService<SettingEntitySettings>();
            var entitySettingService = serviceProvider.GetService<ISettingEntityService>();

            InjectFileTransformationExample(fileTransformationService);

            InjectExampleSettingValuesAsync(settingEntitySettings, entitySettingService).ConfigureAwait(false).GetAwaiter().GetResult();
            
        }

        public void InjectFileTransformationExample(IFileTransformationService fileTransformationService)
        {
            var fileTransformationNameSampleProfileImage = CustomSampleFileStorageDefinition.Transformations.Name_UserSampleProfileImage;

            var fileTransformationSampleProfileImage = fileTransformationService.GetByName(fileTransformationNameSampleProfileImage);
            if (fileTransformationSampleProfileImage == null)
            {
                fileTransformationService.AddAsync(new Shared.Core.Models.FileTransformation.FileTransformationAddModel()
                {
                    Name = fileTransformationNameSampleProfileImage,
                    FileCategoryType = FileCategoryType.StandardImage,
                    ImageSizes = new List<Shared.Core.Models.Image.ImageSize>()
                    {
                        new Shared.Core.Models.Image.ImageSize(50, 50),
                        new Shared.Core.Models.Image.ImageSize(100, 100),
                        new Shared.Core.Models.Image.ImageSize(200, 100),   // Thumb preview
                        new Shared.Core.Models.Image.ImageSize(300, 300),
                    },
                    ExtraRelativePath = "usersampleprofileimages",
                    ImageResizeModeType = ImageResizeModeType.Crop
                });
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task InjectExampleSettingValuesAsync(SettingEntitySettings settingEntitySettings, ISettingEntityService settingEntityService, CancellationToken cancellationToken = default)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!settingEntitySettings.InjectExampleValues)
            {
                return;
            }

            // *** USER EXAMPLES *** //

            // Sample Users settings
            //await settingEntityService.AddValueAsync<SettingUserDataType>(CustomSampleSettingsDefinition.CustomSample.ExampleSetting.Key, CustomSampleAuthInMemoryTestData.UserId_SampleSuperAdmin, "Sample example setting. Sample Super Admin", null, cancellationToken);
            //await settingEntityService.AddValueAsync<SettingUserDataType>(CustomSampleSettingsDefinition.CustomSample.ExampleSetting.Key, CustomSampleAuthInMemoryTestData.UserId_SampleUser, "Sample example setting. Sample User", null, cancellationToken);
            //await settingEntityService.AddValueAsync<SettingUserDataType>(CustomSampleSettingsDefinition.CustomSample.ExampleSetting.Key, CustomSampleAuthInMemoryTestData.UserId_SampleApplicationAdmin, "Sample example setting. Sample Application Admin", null, cancellationToken);

            //// *** TENANT EXAMPLES *** //

            //// Test Tenant settings
            //await settingEntityService.AddValueAsync<SettingTenantDataType>(CustomSampleSettingsDefinition.CustomSample.ExampleSetting.Key, TenantDefinitions.GetTestTenant().TenantId, "Sample example setting. Test Tenant", null, cancellationToken);

        }

    }
}