using EstoqueLambda.Database.DataContext;
using EstoqueLambda.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using EstoqueLambda.Database.Repository;
using EstoqueLambda.Database.Interfaces;

namespace EstoqueLambda
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
            // Register env and config services
            services.AddTransient<IEnvironmentService, EnvironmentService>();
            services.AddTransient<IConfigurationService, ConfigurationService>();
            // Register DbContext class
            services.AddTransient(provider =>
            {
                var configService = provider.GetService<IConfigurationService>();
                var connectionString = configService.GetConfiguration().GetSection(nameof(DescarteDataContext)).Value;
                var optionsBuilder = new DbContextOptionsBuilder<DescarteDataContext>();
                optionsBuilder.UseMySql(connectionString, builder => builder.MigrationsAssembly("NetCoreLambda.EF.Design"));
                return new DescarteDataContext(optionsBuilder.Options);
            });

            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            // Register other services
            RegisterServices?.Invoke(services);
        }
    }
}
