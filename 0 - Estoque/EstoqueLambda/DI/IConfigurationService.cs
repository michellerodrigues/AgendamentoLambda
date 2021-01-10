using Microsoft.Extensions.Configuration;

namespace EstoqueLambda.DI
{
    public interface IConfigurationService
    {
        AppSettings GetConfiguration();
    }
}
