using Agropop.Database.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace Saga.Dependency.DI
{
    public class DescarteDataContextFactory : IDesignTimeDbContextFactory<DescarteDataContext>
    {
        //public DescarteDataContext CreateDbContext(string[] args)
        //{
        //    IConfigurationRoot configuration = new ConfigurationBuilder()
        //        .SetBasePath(Directory.GetCurrentDirectory())
        //        .AddJsonFile("appsettings.json")
        //        .Build();
        //    var builder = new DbContextOptionsBuilder<DescarteDataContext>();
        //    var connectionString = configuration.GetConnectionString("Default");
        //    builder.UseMySql(connectionString);
        //    return new DescarteDataContext(builder.Options);
        //}
        //public DescarteDataContext CreateDbContext(IServiceCollection services, IConfiguration config)
        //{
        //    Get DbContext from DI system

        //   var resolver = new DependencyResolver
        //   {
        //       CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../NetCoreLambda")
        //   };

        //    var builder = new DbContextOptionsBuilder<DescarteDataContext>();

        //    builder.UseMySql(config.GetConnectionString("Default"));

        //    return new DescarteDataContext(builder.Options);

        //    services.AddDbContext<DescarteDataContext>(ServiceLifetime.Scoped); ;//(typeof(DescarteDataContext)) as DescarteDataContext;
        //}

        public DescarteDataContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            var builder = new DbContextOptionsBuilder<DescarteDataContext>();
            builder.UseMySql(args[0].ToString());
            return new DescarteDataContext(builder.Options);
        }
    }
}