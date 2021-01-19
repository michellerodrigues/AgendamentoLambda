using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace Agropop.Database.DataContext
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
            return DependencyResolver.
                
                
                resolver.ServiceProvider.GetService(typeof(DescarteDataContext)) as DescarteDataContext;
        }
    }
}