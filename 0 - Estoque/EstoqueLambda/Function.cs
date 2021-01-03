using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Amazon.SQS;
using Descarte.Messages.Command;
using EstoqueLambda.Database.DataContext;
using EstoqueLambda.Database.Interfaces;
using EstoqueLambda.Database.Models;
using EstoqueLambda.DI;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task<string> FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            var lotesVencidos = (List<Estoque>)EstoqueRepository.FindItensVencidosEstoque();
            var lotes = new List<LoteVencidoParaDescartarCommand>();
            //var snsClient = new AmazonSimpleNotificationServiceClient()
            
            var client = new AmazonSQSClient();
            string queue = "https://sqs.sa-east-1.amazonaws.com/428672449531/lote-pendentes-queue";

            if (lotesVencidos != null)
            {              
                foreach (Estoque estoque in lotesVencidos)
                {
                    LoteVencidoParaDescartarCommand lote = new LoteVencidoParaDescartarCommand()
                    {
                        DateMsg = System.DateTime.Now,
                        Email = estoque.Fabricante.Email,
                        IdMsr = Guid.NewGuid(),
                        Lote = estoque.EstoqueId
                    };
                    lote.TypeMsg = lote.GetType().AssemblyQualifiedName;

                    lotes.Add(lote);
                }
                foreach (LoteVencidoParaDescartarCommand lote in lotes)
                {
                    await client.SendMessageAsync(queue, JsonConvert.SerializeObject(lote)).ConfigureAwait(false);
                }
                return $"Lotes enviados para a fila : {lotes.Count}";
            }
            return $"Não existem lotes vencidos para descarte";      
        }

        public string PublicarNoTopico(string topicArn, string message)
        {
            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.EUSouth1);

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = topicArn
            };

            client.PublishAsync(request);

            return "Mensagem Publicada com sucesso";
        }

        //public string VerificarFile(string queue)
        //{
        //    var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.EUSouth1);

        //    var request = new PublishRequest
        //    {
        //        Message = message,
        //        TopicArn = topicArn
        //    };

        //    client.PublishAsync(request);

        //    return "Mensagem Publicada com sucesso";
        //}

        //public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        //{
        //    foreach (var message in evnt.Records)
        //    {
        //        await ProcessMessageAsync(message, context);
        //    }
        //}

        //private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        //{
        //    context.Logger.LogLine($"Processed message '{message.Body}'");

        //    // TODO: Do interesting work based on the new message

        //    var client = new AmazonSQSClient();
        //    string topic = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento_events_sqs";

        //    RetiradaAgendadaEvent evento = new RetiradaAgendadaEvent()
        //    {
        //        DateMsg = DateTime.Now,
        //        ReceivedMessage = message.Body
        //    };

        //    await client.SendMessageAsync(topic, JsonConvert.SerializeObject(evento)).ConfigureAwait(false);

        //    await Task.CompletedTask;
        //}


    }
}
