using System;
using System.IO;

namespace Saga.Dependency.DI
{
    public class DependencyResolver
    {
        public IServiceProvider ServiceProvider { get; }
        public string CurrentDirectory { get; set; }
        public Action<IServiceCollection> RegisterServices { get; }

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

            //Get strongly typed setting from appsettings binding to object graph
            var appSettings = configuration.Get<AppSettings>();
            // adding to service collection so that it can be resolved/injected as needed.
            services.AddSingleton(appSettings);

            ConfigureEmailService(services, configuration);

            services.AddTransient<IEnvironmentService, EnvironmentService>();
            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IEstoqueRepository, EstoqueRepository>();

           // services.AddTransient<IEmailService, EmailService>();
    
            // Register DbContext class
            services.AddTransient(provider =>
            {                     
                var optionsBuilder = new DbContextOptionsBuilder<DescarteDataContext>();
                optionsBuilder.UseMySql(appSettings.DescarteDataContext, builder => builder.MigrationsAssembly("NetCoreLambda.EF.Design"));
                return new DescarteDataContext(optionsBuilder.Options);
            });                      
            
            // Register other services
            RegisterServices?.Invoke(services);
        }

        private void ConfigureEmailService(IServiceCollection services, IConfiguration configuration)
        {
            EmailHelperExtensions.AddEmailService(services, configuration);           
        }

    }
}
