using Microsoft.Extensions.Configuration;

namespace EstoqueLambda.DI
{
    public class ConfigurationService : IConfigurationService
    {
        public IEnvironmentService EnvService { get; }
        private IConfiguration Configuration { get; set; }

        public AppSettings AppSettings { get; private set; }

        public ConfigurationService(IEnvironmentService envService)
        {
            EnvService = envService;
        }

        public AppSettings GetConfiguration()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{EnvService.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables()
                .Build();
            
            AppSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            return AppSettings;
        }
    }
}
