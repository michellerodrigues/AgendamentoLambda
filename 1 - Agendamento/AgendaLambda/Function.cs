using Agropop.Database.DataContext;
using Agropop.Database.Saga;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Saga.Dependency.DI;
using System;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AgendaLambda
{
    public class Function
    {
        //public AppSettings AppSettings { get; }
        private readonly IEmailService _emailService;
        private readonly ISagaDynamoRepository _sagaDynamoRepository;
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            var resolver = new DependencyResolver();
            _emailService = resolver.GetService<IEmailService>();
            _sagaDynamoRepository = resolver.GetService<ISagaDynamoRepository>();
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

        ///// <summary>
        ///// A simple function that takes a string and does a ToUpper
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        ////public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
            Type tipo = Type.GetType(baseMsg.TypeMsg);
            dynamic instance = Activator.CreateInstance(tipo, false);
            HandleSagaMessage(instance);
        }
        public string HandleSagaMessage(AgendarRetiradaCommand request)
        {
            //salvar mensagem no dynamo
            _sagaDynamoRepository.IncluirMensagemAgendamento(request, request.IdMsr.ToString());

            _emailService.Enviar(request.Email, $"Retirada Pendendente Lote {request.IdMsr}", String.Format("Http://api-saga-gateway/api/agendarRetiradaCommand?idMsr={0}",request.IdMsr));
            //enviar mensagem para o tópico e evento
            //quando receber o evento, enviar o email e mandar para o topico outro evento para a triagem
            //enviar o email para o cidadão
            return "AgendarRetiradaCommand ok";
        }

        public async Task<string> HandleSagaMessage(RetiradaAgendadaEvent request)
        {
            var obj = await _sagaDynamoRepository.BuscarMensagemAgendamento( request.IdMsr.ToString()).ConfigureAwait(false);

            string agendamentoString = JsonConvert.SerializeObject(obj);

            AgendarRetiradaCommand agendarRetiradaCommand = JsonConvert.DeserializeObject<AgendarRetiradaCommand>(agendamentoString);
            //quando  o cidadão clicar no e-mail, uma mensagem será colocada na fila RetiradaAgendadaEvent e irá cair aqui
            //enviar mensagem para o tópico e evento
            //quando receber o evento, enviar o email e mandar para o topico outro evento para a triagem

            return "AgendarRetiradaCommand ok";
        }

    }
}
