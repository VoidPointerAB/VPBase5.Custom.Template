using VPBase.Auth.Contract.ConfigEntities.Importers;
using VPBase.Shared.Core.Services;
using System.Reflection;
using VPBase.Shared.Core.Configuration;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.SharedInterfaces;

namespace VPBase.Custom.Core.CustomConfigImportExporter
{
    public class CustomTestConfigImportExporter : ICustomConfigImportExporter
    {
        private IAuthContractJsonConverter _jsonConverter;
        private IEnvironmentHandler _environmentHandler;

        // Possible to add more dependencies below, for example a setting class controlling the enabled-flags
        public CustomTestConfigImportExporter(IAuthContractJsonConverter jsonConverter,     
            IEnvironmentHandler environmentHandler)
        {
            _jsonConverter = jsonConverter;
            _environmentHandler = environmentHandler;
        }

        public string FileNameWithoutExtension => "customconfig.customtestdata";        // The name of the json file exported and imported in the "configs" folder.

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
            return 10;       // Sortorder should be 0 - (< 10) in Base implementations.
        }

        #endregion

        public bool ImportEnabled
        {
            get
            {
                return _environmentHandler.EnvironmentMode == EnvironmentMode.Development ||        // You can also use a setting object (using a dependency) to the class to 
                       _environmentHandler.EnvironmentMode == EnvironmentMode.Demo;                 // control if the import should be enabled or not.
            }
        }

        public bool ExportEnabled
        {
            get
            {
                return _environmentHandler.EnvironmentMode == EnvironmentMode.Development ||        // You can also use a setting object (using a dependency) to the class to 
                       _environmentHandler.EnvironmentMode == EnvironmentMode.Demo;                 // control if the export should be enabled or not.
            }
        }

        /// <summary>
        /// The import method will be runned if ImportEnabled is true. 
        /// The import is runned when the application is starting up and after the export method.
        /// </summary>
        /// <returns>DataImportResult containing info about the imported result.</returns>
        public DataImportResult Import(string jsonData)
        {
            var dataImportResult = new DataImportResult(this);

            try
            {
                if (!string.IsNullOrEmpty(jsonData))
                {
                    dataImportResult.Logg.Add("found json-data!");

                    var testCustomData = _jsonConverter.DeserializeObject<CustomTestData>(jsonData);

                    if (testCustomData != null)
                    {
                        // Here we could insert or even update data in the custom application.

                        dataImportResult.Logg.Add($"Id: {testCustomData.Id}, TenantId: {testCustomData.TenantId}");

                        // Entity #1

                        if (testCustomData.TestCustomDataItems != null)
                        {
                            foreach (var item in testCustomData.TestCustomDataItems)
                            {
                                // Import data to db or something
                                dataImportResult.AddedItems++;

                                // dataImportResult.ModifiedItems++;

                                // dataImportResult.DeletedItems++;
                            }
                        }

                        // Entity #2

                        if (testCustomData.TestCustomDataItems2 != null)
                        {
                            foreach (var item in testCustomData.TestCustomDataItems2)
                            {
                                // Import data to db or something
                                dataImportResult.AddedItems++;

                                // dataImportResult.ModifiedItems++;

                                // dataImportResult.DeletedItems++;
                            }
                        }
                    }
                }
                else
                {
                    dataImportResult.Logg.Add("found no json-data!");
                }

            }
            catch (Exception ex)
            {
                dataImportResult.Exceptions.Add(ex);
                return dataImportResult;
            }

            return dataImportResult;
        }

        /// <summary>
        /// The export method will be runned if ExportEnabled is true and validation is true
        /// The export is runned when the application is starting up and before the import method.
        /// The exported file will end up in the "configs" folder with the FileNameWithoutExtension name as 
        /// json file.
        /// </summary>
        /// <returns>True or false depending on the export is successful or not</returns>
        public DataExportResult Export()
        {
            var dataExportResult = new DataExportResult(this);

            try
            {
                // Below is an example of an object to be serialized to the json-file.
                // Use the class as a container to hold all data to be serialized.
                // Only use objects with possible empty contructors otherwise serialization
                // could fail.

                var customBaseTestData = new CustomTestData
                {
                    Id = "Custom_TestId",

                    TenantId = TenantDefinitions.GetTestTenant().TenantId,      // Here just a example that the data in the container belong to a special tenant

                    // Read data from db or something to a list of entities

                    // Entity #1 (example of entities)

                    TestCustomDataItems =
                    [
                        new CustomTestDataItem()
                        {
                            Name = "Custom_Test1_1",
                            Description = "Custom_TestDescription1_1",
                        },
                        new CustomTestDataItem()
                        {
                            Name = "Custom_Test1_2",
                            Description = "Custom_TestDescription1_2",
                        }
                    ],

                    // Entity #2

                    TestCustomDataItems2 =
                    [
                        new CustomTestDataItem()
                        {
                            Name = "Custom_Test2_1",
                            Description = "Custom_TestDescription2_1",
                        },
                        new CustomTestDataItem()
                        {
                            Name = "Custom_Test2_2",
                            Description = "Custom_TestDescription2_2",
                        }
                    ]
                };

                dataExportResult.Logg.Add("trying to serialize object to json-data!");

                var jsonData = _jsonConverter.SerializeObject(customBaseTestData);

                dataExportResult.Data = jsonData;
            }
            catch (Exception ex)
            {
                dataExportResult.Exceptions.Add(ex);
                dataExportResult.Logg.Add($"Error: {ex.Message}");
                return dataExportResult;
            }

            return dataExportResult;
        }
    }
}
