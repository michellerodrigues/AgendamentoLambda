using EstoqueLambda.DI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection
{
    public class ConfigurationService : IConfigurationService
    {
        public IEnvironmentService EnvService { get; }
        public IServiceCollection Services { get; }
        private IConfiguration Configuration { get; set; }

        public AppSettings AppSettings { get; private set; }

        public ConfigurationService(IEnvironmentService envService, IServiceCollection services)
        {
            EnvService = envService;
            Services = services;
        }

        public AppSettings GetConfiguration(IServiceCollection services)
        {
            services.AddOptions(); //OPTIONAL

            //...add additional services as needed

            //building configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvService.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            //Get strongly typed setting from appsettings binding to object graph
            var settings = configuration.Get<AppSettings>();
            // adding to service collection so that it can be resolved/injected as needed.
            services.AddSingleton(settings);

            AppSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            EmailConfigOptions emailConfigOptions = new EmailConfigOptions();

            emailConfigOptions = settings.EmailConfig;

            services.AddSingleton(emailConfigOptions);

            return AppSettings;
        }
    }
}
