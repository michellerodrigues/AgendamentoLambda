using Amazon.Lambda.Core;
using EstoqueLambda.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EstoqueLambda.DI;
using EstoqueLambda.Database.DataContext;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EstoqueLambda
{
    public class Function
    {
        public Microsoft.Extensions.Configuration.IConfiguration ConfigService { get; }
        public IProdutoRepository ProdutoRepository { get; }
        
        public Function()
        {
            var resolver = new DependencyResolver();
            ProdutoRepository = resolver.ServiceProvider.GetService<IProdutoRepository>();
            ConfigService = resolver.ServiceProvider.GetService<IConfigurationService>().GetConfiguration();
            var initializeDbContext = new InitializeDbContext();
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {            
            return ConfigService.GetSection(input).Value;
        }              
    }
}
