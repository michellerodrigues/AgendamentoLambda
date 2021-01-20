using Agropop.Database.DataContext;
using EmailHelper;
using Microsoft.EntityFrameworkCore;
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
            var descarteContext = ConfigureDescarteDataContext(appSettings);

            services.AddSingleton(descarteContext);

           // services.AddDbContext<DbContext>(ServiceLifetime.Scoped);

           AddEmailService(services, configuration);
            services.AddTransient<IEnvironmentService, EnvironmentService>();

            // Register other services
            RegisterServices?.Invoke(services);
        }

        private static DescarteDataContext ConfigureDescarteDataContext(AppSettings settings)
        {
            DescarteDataContextFactory factory = new DescarteDataContextFactory();
            string[] args = new string[1] { settings.DescarteDataContext };

            return factory.CreateDbContext(args);
            
           

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

        public DescarteDataContext CreateDbContext(string[] args)
        {
            this.CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../NetCoreLambda");
            
            //IConfigurationRoot configuration = new ConfigurationBuilder()
            //.SetBasePath(Directory.GetCurrentDirectory())
            //.AddJsonFile("appsettings.json")
            //.Build();
            var builder = new DbContextOptionsBuilder<DescarteDataContext>();
            builder.UseMySql(args[0].ToString());
            return new DescarteDataContext(builder.Options);
        }
    }
}
