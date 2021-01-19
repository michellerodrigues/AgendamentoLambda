using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Saga.Dependency.DI
{
    public interface IConfigurationService
    {
        AppSettings GetConfiguration(IServiceCollection services);
    }
}
