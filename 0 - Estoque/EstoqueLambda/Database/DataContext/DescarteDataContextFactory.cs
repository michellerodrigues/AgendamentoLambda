using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace EstoqueLambda.Database.DataContext
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
        public DescarteDataContext CreateDbContext(string[] args)
        {
            // Get DbContext from DI system
            var resolver = new DependencyResolver
            {
                CurrentDirectory = Path.Combine(Directory.GetCurrentDirectory(), "../NetCoreLambda")
            };
            return resolver.ServiceProvider.GetService(typeof(DescarteDataContext)) as DescarteDataContext;
        }
    }
}