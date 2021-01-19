using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AgendaLambda
{
    public class Function
    {
        //public AppSettings AppSettings { get; }
        //private readonly IEmailService _emailService;
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            //var resolver = new DependencyResolver();
            //_emailService = resolver.ServiceProvider.GetService<IEmailService>();

            //var initializeDbContext = new InitializeDbContext();


        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach (var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed message '{message.Body}'");

            // TODO: Do interesting work based on the new message

            var client = new AmazonSQSClient();
            string topic = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento_events_sqs";

            RetiradaAgendadaEvent evento = new RetiradaAgendadaEvent()
            {
                DateMsg = DateTime.Now,
                ReceivedMessage = message.Body
            };

            await client.SendMessageAsync(topic, JsonConvert.SerializeObject(evento)).ConfigureAwait(false);

            await Task.CompletedTask;
        }
    }

//        var lotesVencidos = (List<Estoque>)_estoqueRepository.FindItensVencidosEstoque();
//        var lotes = new List<LotesVencidosVerificadosEvent>();

//        var client = new AmazonSQSClient();
//        string queue = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento";

//            if (lotesVencidos != null)
//            {
//                foreach (Estoque estoque in lotesVencidos)
//                {
//                    LotesVencidosVerificadosEvent lote = new LotesVencidosVerificadosEvent()
//                    {
//                        DateMsg = System.DateTime.Now,
//                        Email = estoque.Fabricante.Email,
//                        IdMsr = Guid.NewGuid(),
//                        Lote = estoque.EstoqueId
//                    };
//        lote.TypeMsg = lote.GetType().AssemblyQualifiedName;

//                    lotes.Add(lote);
//                }
//                foreach (LotesVencidosVerificadosEvent lote in lotes)
//                {
//                    await client.SendMessageAsync(queue, JsonConvert.SerializeObject(lote)).ConfigureAwait(false);
//}
//                return $"Lotes enviados para a fila : {lotes.Count}";
//            }
//            return $"Não existem lotes vencidos para descarte";
//        }

//        public async Task<string> HandleSagaMessage(AgendarRetiradaCommand msg)
//        {
            

//            var client = new AmazonSQSClient();
//            string queue = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento";

//            if (lotesVencidos != null)
//            {
//                return $"Lotes Descartados Com Sucesso";
//            }
//            return $"Não existem lotes vencidos para descarte";
//        }
//    }


}
