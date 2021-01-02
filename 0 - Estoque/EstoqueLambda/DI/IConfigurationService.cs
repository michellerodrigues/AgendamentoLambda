using Microsoft.Extensions.Configuration;

namespace EstoqueLambda.DI
{
    public interface IConfigurationService
    {
        IConfiguration GetConfiguration();
    }
}
