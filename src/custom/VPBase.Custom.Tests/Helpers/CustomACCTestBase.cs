using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System.IO;
using System.Linq;
using VPBase.Auth.Client.Code.ApiClients;
using VPBase.Auth.Client.Code.ApiClients.AccessToken;
using VPBase.Auth.Client.Code.ApiClients.InMemory.TestData;
using VPBase.Auth.Client.Code.InMemory;
using VPBase.Auth.Client.SharedInterfaces;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.ConfigEntities.Tenants;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.Definitions.AppConfigs;
using VPBase.Auth.Contract.Definitions.TenantConfigs;
using VPBase.Auth.Contract.Files;
using VPBase.Auth.Contract.SharedInterfaces;
using VPBase.Shared.Core.Definitions;
using VPBase.Shared.Core.SharedImplementations.AuthContract;
using VPBase.Shared.Core.Types;
using VPBase.Shared.Server.SharedImplementations.AuthClient;

namespace VPBase.Custom.Tests.Helpers
{
    [TestFixture]
    public abstract class CustomACCTestBase
    {
        public const string AuthBaseUrl = "https://localhost:44332";

        public CustomACCTestBase()
        {
            ACC_Base = new ACC_Base_Test();
            ACC_SharedContract = new ACC_SharedContract_Test();
            ACC_CommunicationService = new ACC_CommunicationService_Test();
            ACC_Other = new ACC_Other_Test();
        }

        public ACC_Base_Test ACC_Base { get; set; }

        public ACC_SharedContract_Test ACC_SharedContract { get; set; }

        public ACC_CommunicationService_Test ACC_CommunicationService { get; set; }

        public ACC_Other_Test ACC_Other { get; set; }

        [SetUp]
        public void SetUp()
        {
            Setup_Base();
            Setup_SharedContracts();
            Setup_CommunicationServices();
            Setup_Other();
        }

        public void SetupACCDependencies(IServiceCollection services)
        {
            // *** SharedContract - Auth.Contract ***//

            services.AddSingleton(x => ACC_SharedContract.AuthContractAssemblyHandler);
            services.AddSingleton(x => ACC_SharedContract.AuthContractCryptionHandler);

            services.AddSingleton(x => ACC_SharedContract.AuthContractDateTimeProvider);
            services.AddSingleton(x => ACC_SharedContract.TestAuthContractDateTimeProvider);

            services.AddSingleton(x => ACC_SharedContract.AuthContractFileHandler);
            services.AddSingleton(x => ACC_SharedContract.TestAuthContractFileHandler);

            services.AddSingleton(x => ACC_SharedContract.AuthContractJsonConverter);

            services.AddSingleton(x => ACC_SharedContract.AuthContractLogger);
            services.AddSingleton(x => ACC_SharedContract.TestAuthContractLogger);

            services.AddSingleton(x => ACC_SharedContract.AuthContractThreading);

            // *** SharedContract - Auth.Client ***//

            services.AddSingleton(x => ACC_SharedContract.AccessTokenSettings);

            services.AddSingleton(x => ACC_SharedContract.AuthClientAccessTokenManager);

            services.AddSingleton(x => ACC_SharedContract.AuthClientHelper);

            // *** CommunicationServices ***//

            services.AddSingleton(x => ACC_CommunicationService.InMemoryClientAuthService);
            services.AddSingleton(x => ACC_CommunicationService.ClientAuthService);

            // *** Other ***//

            services.AddSingleton(x => ACC_Other.ConfigFolderFileHandlerSettings);
            services.AddSingleton(x => ACC_Other.ConfigConverter);
            services.AddSingleton(x => ACC_Other.ConfigHandler);
            services.AddSingleton(x => ACC_Other.TenantInfoHelper);
        }

        private void Setup_Base()
        {
            ACC_Base.AppConfigDefinition = new SharedAppConfigDefinition();

            var sharedVPTenantConfigDefinition = new SharedVPTenantConfigDefinition();

            ACC_Base.TenantConfigDefinition = sharedVPTenantConfigDefinition;
            ACC_Base.TenantConfig = sharedVPTenantConfigDefinition.GetTenant();

            ACC_Base.ClientCommunicationType = ACCCommunicationType.Test;
        }

        private void Setup_SharedContracts()
        {
            // *** SharedContract - Auth.Contract ***//

            ACC_SharedContract.AuthContractAssemblyHandler = new AuthContractAssemblyHandler();

            var cryptionHandler = new AuthContractCryptionHandler();
            cryptionHandler.AddUsingKey(CryptionTestsSharedInternal.UsingKey);
            ACC_SharedContract.AuthContractCryptionHandler = cryptionHandler;

            // AuthContractDateTimeProvider - avoid setting current DateTime
            ACC_SharedContract.TestAuthContractDateTimeProvider = new TestAuthContractDateTimeProvider();
            ACC_SharedContract.AuthContractDateTimeProvider = ACC_SharedContract.TestAuthContractDateTimeProvider;

            // TestAuthContractFileHandler - avoid file access
            ACC_SharedContract.TestAuthContractFileHandler = new TestAuthContractFileHandler();
            ACC_SharedContract.AuthContractFileHandler = ACC_SharedContract.TestAuthContractFileHandler;
            ACC_SharedContract.TestAuthContractFileHandler.TestableFiles.Clear();

            ACC_SharedContract.AuthContractJsonConverter = new AuthContractJsonConverter();

            // AuthContractLogger - avoid file access
            ACC_SharedContract.TestAuthContractLogger = new TestAuthContractLogger();
            ACC_SharedContract.AuthContractLogger = ACC_SharedContract.TestAuthContractLogger;

            ACC_SharedContract.AuthContractThreading = new AuthContractThreading();

            // *** SharedContract - Auth.Client ***//

            var applicationConfig = ACC_Base.AppConfigDefinition.GetApplication();
            var applicationClient = applicationConfig.GetApplicationSyncApiClient();
            var scope = applicationClient.AllowedScopes.First();

            ACC_SharedContract.AccessTokenSettings = new AccessTokenSettings
            {
                AccessTokenEndPoint = AuthBaseUrl,
                ClientId = applicationClient.ClientId,
                Scope = scope,
                Secret = applicationClient.Secret
            };

            ACC_SharedContract.AuthClientAccessTokenManager = new AuthClientAccessTokenManager(ACC_SharedContract.AccessTokenSettings);
            ACC_SharedContract.AuthClientHelper = new AuthClientHelper(ACC_SharedContract.AuthContractJsonConverter);
        }

        private void Setup_CommunicationServices()
        {
            var inMemoryClientAuthService = new InMemoryClientAuthService();

            var applicationConfig = ACC_Base.AppConfigDefinition.GetApplication();
            var tenantConfig = ACC_Base.TenantConfig;

            var testDataService = new InMemoryBaseTestData(applicationConfig, tenantConfig);
            var testDataParams = testDataService.CreateBaseTestDataParams();

            inMemoryClientAuthService.Applications.Add(testDataParams.Application);
            inMemoryClientAuthService.Users.AddRange(testDataParams.Users);
            inMemoryClientAuthService.Groups.AddRange(testDataParams.Groups);

            ACC_CommunicationService.InMemoryClientAuthService = inMemoryClientAuthService;
            ACC_CommunicationService.ClientAuthService = ACC_CommunicationService.InMemoryClientAuthService;
        }

        private void Setup_Other()
        {
            ACC_Other.ConfigFolderFileHandlerSettings = new ConfigFolderFileHandlerSettings
            {
                PhysicalPath = GetPhysicalFolderPath(),
                FileExtension = ".json"
            };

            ACC_Other.ConfigConverter = new ConfigConverter(ACC_SharedContract.AuthContractJsonConverter, ACC_SharedContract.AuthContractAssemblyHandler);

            ACC_Other.ConfigHandler = new ConfigHandler(ACC_Other.ConfigConverter,
                ACC_SharedContract.AuthContractFileHandler,
                ACC_SharedContract.AuthContractLogger,
                ACC_SharedContract.AuthContractCryptionHandler);

            ACC_Other.TenantInfoHelper = new TenantInfoHelper(ACC_SharedContract.AuthContractJsonConverter);
        }

        public static string GetPhysicalFolderPath()
        {
            string assemblyLocation = System.Reflection.Assembly.GetAssembly(typeof(CustomACCTestBase)).Location;
            string currentDirectory = Path.GetDirectoryName(assemblyLocation);
            var configsFolderPath = currentDirectory + "\\configs";
            return configsFolderPath;
        }
    }

    public class ACC_Base_Test
    {
        public IAppConfigDefinition AppConfigDefinition { get; set; }

        public TenantConfig TenantConfig { get; set; }

        public INetCoreTenantConfigDefinition TenantConfigDefinition { get; set; }

        public ACCCommunicationType ClientCommunicationType { get; set; }
    }

    public class ACC_SharedContract_Test
    {
        // AuthContract

        public IAuthContractAssemblyHandler AuthContractAssemblyHandler { get; set; }

        public IAuthContractCryptionHandler AuthContractCryptionHandler { get; set; }

        public IAuthContractDateTimeProvider AuthContractDateTimeProvider { get; set; }
        public TestAuthContractDateTimeProvider TestAuthContractDateTimeProvider { get; set; }

        public IAuthContractFileHandler AuthContractFileHandler { get; set; }
        public TestAuthContractFileHandler TestAuthContractFileHandler { get; set; }

        public IAuthContractJsonConverter AuthContractJsonConverter { get; set; }

        public IAuthContractLogger AuthContractLogger { get; set; }
        public TestAuthContractLogger TestAuthContractLogger { get; set; }

        public IAuthContractThreading AuthContractThreading { get; set; }

        // AuthClient

        public AccessTokenSettings AccessTokenSettings { get; set; }

        public IAuthClientAccessTokenManager AuthClientAccessTokenManager { get; set; }

        public IAuthClientHelper AuthClientHelper { get; set; }

        public AccessTokenService AccessTokenService { get; set; }
    }

    public class ACC_CommunicationService_Test
    {
        public IClientAuthService ClientAuthService { get; set; }

        public InMemoryClientAuthService InMemoryClientAuthService { get; set; }
    }

    public class ACC_Other_Test
    {
        public ConfigFolderFileHandlerSettings ConfigFolderFileHandlerSettings { get; set; }

        public ConfigConverter ConfigConverter { get; set; }

        public ConfigHandler ConfigHandler { get; set; }

        public TenantInfoHelper TenantInfoHelper { get; set; }
    }

    // Important: internal class
    internal class CryptionTestsSharedInternal
    {
        // Note: Only one place in the tests that the using key exists!
        public const string UsingKey = "VPBase rockar fett!";
    }
}
