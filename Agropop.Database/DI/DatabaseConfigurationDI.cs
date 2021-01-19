using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Agropop.Database.DataContext;
using Agropop.Database.Interfaces;
using Agropop.Database.Repository;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseConfigurationDI
    {
        public static IOptionsMonitor<DatabaseConfigOptions> databaseConfigOptions { get; set; }

        public static void AddDatabase(IServiceCollection services, IConfiguration Configuration)
        {
            var config = new DatabaseConfigOptions();
            Configuration.Bind(DatabaseConfigOptions.DatabaseOptions, config);

            services.AddSingleton<DatabaseConfigOptions>(config);

            var appSettings = Configuration.Get<DatabaseConfigOptions>();

            services.AddTransient(provider =>
            {
                var optionsBuilder = new EntityFrameworkCore.DbContextOptionsBuilder<DescarteDataContext>();
                optionsBuilder.UseMySql(appSettings.ConnectionString, builder => builder.MigrationsAssembly("NetCoreLambda.EF.Design"));
                return new Agropop.Database.DataContext.DescarteDataContext(optionsBuilder.Options);
            });

            services.AddTransient<IProdutoRepository, ProdutoRepository>();
            services.AddTransient<IEstoqueRepository, EstoqueRepository>();
        }

    }
}
