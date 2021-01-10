using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EstoqueLambda.DI
{
    public interface IConfigurationService
    {
        AppSettings GetConfiguration(IServiceCollection services);
    }
}
