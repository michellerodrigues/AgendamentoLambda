using Amazon.Lambda.Core;
using EstoqueLambda.Database.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using EstoqueLambda.DI;
using EstoqueLambda.Database.DataContext;
using Amazon.SQS;
using Descarte.Messages.Event;
using Newtonsoft.Json;
using System.Threading.Tasks;
using EstoqueLambda.Database.Models;
using System.Collections.Generic;
using Descarte.Messages.Command;
using System;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EstoqueLambda
{
    public class Function
    {
        public Microsoft.Extensions.Configuration.IConfiguration ConfigService { get; }
        public IEstoqueRepository EstoqueRepository { get; }
        
        public Function()
        {
            var resolver = new DependencyResolver();
            EstoqueRepository = resolver.ServiceProvider.GetService<IEstoqueRepository>();
            ConfigService = resolver.ServiceProvider.GetService<IConfigurationService>().GetConfiguration();
            var initializeDbContext = new InitializeDbContext();
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(string input, ILambdaContext context)
        {
            var lotesVencidos = (List<Estoque>)EstoqueRepository.FindItensVencidosEstoque();
            var lotes = new List<LoteVencidoParaDescartarCommand>();
            var client = new AmazonSQSClient();
            string topic = "https://sqs.sa-east-1.amazonaws.com/428672449531/DescarteSagaTopic";

            if (lotesVencidos != null)
            {              
                foreach (Estoque estoque in lotesVencidos)
                {
                    LoteVencidoParaDescartarCommand lote = new LoteVencidoParaDescartarCommand()
                    {
                        DateMsg = System.DateTime.Now,
                        TypeMsg = typeof(LoteVencidoParaDescartarCommand).ToString(),
                        Email = estoque.Fabricante.Email,
                        IdMsr = Guid.NewGuid(),
                        Lote = Guid.Parse(estoque.Lote)
                    };
                    lotes.Add(lote);
                }
                foreach (LoteVencidoParaDescartarCommand lote in lotes)
                {
                    await client.SendMessageAsync(topic, JsonConvert.SerializeObject(lote)).ConfigureAwait(false);
                }
                return $"Lotes enviados para a fila : {lotes.Count}";
            }
            return $"Não existem lotes vencidos para descarte";      
        }              
    }
}
