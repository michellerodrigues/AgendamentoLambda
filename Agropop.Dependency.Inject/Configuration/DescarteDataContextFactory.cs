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
        public DescarteDataContext CreateDbContext(string[] args)
        {

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