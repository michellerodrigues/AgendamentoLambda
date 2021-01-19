using Agropop.Database.DataContext;
using EmailHelper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace Saga.Dependency.DI
{
    public class DependencyResolver
    {
        public IServiceProvider ServiceProvider { get; }
        public string CurrentDirectory { get; set; }
        public Action<IServiceCollection> RegisterServices { get; }

        public static IOptionsMonitor<EmailConfigOptions> emailConfigOptions { get; set; }

        public DependencyResolver(Action<IServiceCollection> registerServices = null)
        {
            // Set up Dependency Injection
            var serviceCollection = new ServiceCollection();
            RegisterServices = registerServices;
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

        }

        private void ConfigureServices(IServiceCollection services)
        {
            //building configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var appSettings = configuration.Get<AppSettings>();
            services.AddSingleton(appSettings);
            ConfigureDescarteDataContext(services);
            AddEmailService(services, configuration);
            services.AddTransient<IEnvironmentService, EnvironmentService>();

            // Register other services
            RegisterServices?.Invoke(services);
        }

        private static void ConfigureDescarteDataContext(IServiceCollection services)
        {
            services.AddDbContext<DescarteDataContext>();
        }

        private static void AddEmailService(IServiceCollection services, IConfiguration Configuration)
        {
            var config = new EmailConfigOptions();
            Configuration.Bind(EmailConfigOptions.EmailConfig, config);

            services.AddSingleton<EmailConfigOptions>(config);

            services.AddTransient<IEmailService, EmailService>();
        }
    

        public T GetService<T>() where T: class
        {
            return ServiceProvider.GetService<T>();
        }

        //var config = new EmailConfigOptions();
        //Configuration.Bind(EmailConfigOptions.EmailConfig, config);

        //    services.AddSingleton<EmailConfigOptions>(config);

        //    services.AddTransient<IEmailService, EmailService>();

    }
}
