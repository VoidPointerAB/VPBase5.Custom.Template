using VPBase.Shared.Core.Configuration;

namespace VPBase.Custom.Server
{
#pragma warning disable CS1591
    public class Program
    {
        private const string moduleName = ConfigModuleConstants.Custom;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args)
               .Build()
               .Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel();
                    webBuilder.UseContentRoot(Directory.GetCurrentDirectory());
                    webBuilder.UseIISIntegration();
                    webBuilder.UseStartup<Startup>();
                    webBuilder.CaptureStartupErrors(true);
                    webBuilder.UseSetting("detailedErrors", "true");
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        var path = AppDomain.CurrentDomain.BaseDirectory;
                        IWebHostEnvironment env = hostingContext.HostingEnvironment;
                        SettingsHelper.Init(moduleName, env.EnvironmentName, path);
                        SettingsHelper.AddJsonFiles(config);
                    });
                }).ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddDebug();
                    logging.AddConsole();
                    logging.AddLog4Net(log4NetConfigFile: "log4net.config");
                });
    }
#pragma warning restore CS1591
}

