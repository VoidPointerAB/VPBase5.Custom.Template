using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.ConfigEntities.Applications;
using VPBase.Base.Server;
using VPBase.Base.Server.Configuration;
using VPBase.Custom.Server.Configuration;
using VPBase.Shared.Server.Configuration;

namespace VPBase.Custom.Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public IWebHostEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var startupInstructions = new List<IStartupInstruction>
            {
                new BaseStartupInstruction(),
                new CustomStartupInstruction()
            };

            var configurationItemsContainer = new ConfigurationItemsContainer();
            AssemblyBootstrap.ExecuteStartupInstructions(configurationItemsContainer, startupInstructions);

            BaseStartup.ConfigureServices(services, Configuration, Environment, configurationItemsContainer);
        }

        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment environment,
            ConfigHandler configHandler,
            ConfigurationItemsContainer configurationItemsContainer,
            ApplicationConfig authApplication)
        {
            BaseStartup.Configure(app, environment, configHandler, Configuration, configurationItemsContainer, authApplication);
        }
    }
}
