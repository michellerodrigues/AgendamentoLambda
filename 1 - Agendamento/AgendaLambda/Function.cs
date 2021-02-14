using Agropop.AwsServices.Helper;
using Agropop.Database.DataContext;
using Agropop.Database.Saga;
using Agropop.Database.Saga.Tables;
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
        private readonly string _topicArn;
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
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:saga-descarte-topic-sns.fifo";
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
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

        ///// <summary>
        ///// A simple function that takes a string and does a ToUpper
        ///// </summary>
        ///// <param name="input"></param>
        ///// <param name="context"></param>
        ///// <returns></returns>
        ////public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var message in sqsEvent.Records)
            {
                BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
                Type tipo = Type.GetType(baseMsg.TypeMsg);
                dynamic instance = Activator.CreateInstance(tipo, false);
                await HandleSagaMessage(instance);
            }
        }

        public async Task HandleSagaMessage(LotesVencidosVerificadosEvent request)
        {
            AgendarRetiradaCommand agendar = new AgendarRetiradaCommand()
            {
                DataRetirada = DateTime.Now.AddDays(7), //RN
                DateMsg = DateTime.Now,
                Email = request.Email,
                IdMsr = request.IdMsr,
                Lote = request.Lote,
            };

            agendar.TypeMsg = agendar.GetType().AssemblyQualifiedName;

            await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(agendar), agendar.TypeMsg, _topicArn);
        }

        public async Task HandleSagaMessage(AgendarRetiradaCommand request)
        {

            SagaMessageTable objDynamo = new SagaMessageTable()
            {
                IdMsg = request.IdMsr.ToString(),
                Msg = JsonConvert.SerializeObject(request),
                TypeMsg = request.GetType().AssemblyQualifiedName
            };

            await _sagaDynamoRepository.IncluirMensagemAgendamento(objDynamo);

            await _emailService.Enviar(request.Email, $"Retirada Agendada Lote {request.IdMsr}", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/agendar?msgid={0}", request.IdMsr));
            
            //publicar event
        }

       

        public async Task HandleSagaMessage(ConfirmarAgendamentoRetiradaCommand request)
        {
            await _emailService.Enviar(request.Email, $"Retirada Confirmada Lote {request.IdMsr}. Em caso de Cancelamento", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));


            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var evento = JsonConvert.DeserializeObject<AgendamentoRetiradaConfirmadoEvent>(requestString);
            evento.TypeMsg = evento.GetType().AssemblyQualifiedName;

            await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(evento), evento.TypeMsg, _topicArn);
        }

    }
}
