/*═══════════════════════════════════════════════════════════════════════════════════════════╗
║ READONLY: MemoryCustomDbTestBase
╟────────────────────────────────────────────────────────────────────────────────────────────╢
║ This file is provided from base - DO NOT MODIFY IT.
║ Instead, add your own layer by subclassing it and put your custom code in that sub class.
║ Then, let your tests inherit from your sub class.
╚═══════════════════════════════════════════════════════════════════════════════════════════*/
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using VPBase.Base.Core.Helpers.DateTimeProvider;
using VPBase.Custom.Core.Configuration;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Server.Configuration;
using VPBase.Custom.Tests.Settings;
using VPBase.Shared.Core.Fakes;
using VPBase.Shared.Core.Helpers;
using VPBase.Shared.Core.Helpers.DateTimeProvider;
using VPBase.Shared.Core.Services;
using VPBase.Shared.Server.Configuration;
using VPBase.Shared.Server.Dependencies;
using VPBase.Shared.Server.Dependencies.Fakes;
using VPBase.Shared.Server.Helpers;

namespace VPBase.Custom.Tests.Helpers
{
    public abstract class MemoryCustomDbTestBase : CustomACCTestBase
    {
        public ICustomStorage Storage { get; set; }
        public DbContext DbContext { get; set; }

        public IDateTimeProvider DateTimeProvider { get; set; }

        public ILogger Logger { get; set; }

        private IServiceCollection Services { get; set; }
        public ServiceProvider ServiceProvider { get; set; }

        public const string TenantTestId = "TEST";

        public FakeCustomAppSettings CustomAppSettings { get; set; }
        public FakeAppSettings AppSettings { get; set; }

        public SharedFakeFriendlyIdCounterService SharedFakeFriendlyIdCounterService { get; set; }

        [SetUp]
        public new void SetUp()
        {
            Services = new ServiceCollection()
                .AddLogging(config => config.AddDebug());

            var databaseManagerOptions = new DbContextOptionsBuilder<CustomDatabaseManager>()
                .UseInMemoryDatabase("In memory test database for CustomDatabaseManager")
                .Options;

            var dbManager = new CustomDatabaseManager(databaseManagerOptions);
            dbManager.Database.EnsureDeleted(); 
            Storage = dbManager;
            DbContext = dbManager;

            Services.AddTransient<ICustomStorage>(c => dbManager);

            // Fakes from Contract-assembly to be used in other modules also.
            Services.AddTransient<ICustomFieldValueService, FakeCustomFieldValueService>();
            Services.AddTransient<IInstanceContractBaseCreator, FakeInstanceContractBaseCreator>();

            Services.AddSingleton(databaseManagerOptions);
            Services.AddSingleton<CustomDbTransactionHandler>();
            Services.AddTransient<ICustomDbFactory, CustomDbFactory>();

            var startupInstructionContainer = new ConfigurationItemsContainer();
            var customStartupInstructions = new CustomStartupInstruction();
            customStartupInstructions.Execute(startupInstructionContainer);

            // AppSettings:
            AppSettings = new FakeAppSettings();
            Services.AddSingleton<AppSettings>(AppSettings);
            AppSettingsHelper.ApplyAppSettings(AppSettings);

            CustomAppSettings = new FakeCustomAppSettings();
            Services.AddSingleton<CustomAppSettings>(CustomAppSettings);
            CustomAppSettingsHelper.ApplyCustomAppSettings(CustomAppSettings);

            SetupACCDependencies(Services);

            // Now safe to add dependencies
            foreach (var dependencyResolver in startupInstructionContainer.DependencyResolvers)
            {
                if (dependencyResolver.GetDependencyType() != DependencyType.PhysicalFiles)
                {
                    dependencyResolver.Register(Services, Shared.Core.Configuration.EnvironmentMode.Development);
                }
            }

            var dateTimeProvider = new FakeDateTimeProvider(new DateTime(2020, 1, 1));
            DateTimeProvider = dateTimeProvider;
            Services.AddSingleton<IDateTimeProvider>(x => dateTimeProvider);

            AddSharedIdDependencies();

            ServiceProvider = Services.BuildServiceProvider();

            Logger = ServiceProvider.GetService<ILoggerFactory>().CreateLogger("TestLogger");
        }

        private void AddSharedIdDependencies()
        {
            Services.AddTransient<SharedIdHelper>();
            SharedFakeFriendlyIdCounterService = new SharedFakeFriendlyIdCounterService();
            SharedFakeFriendlyIdCounterService.Fake_Clear();
            Services.AddTransient<IFriendlyIdCounterService, SharedFakeFriendlyIdCounterService>();
        }

        [TearDown]
        public void TearDown()
        {
            Storage?.Dispose();
            DbContext?.Dispose();
        }
    }
}
