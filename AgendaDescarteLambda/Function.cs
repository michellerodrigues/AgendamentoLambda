using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Agropop.AwsServices.Helper;
using Agropop.AwsServices.Helper.SNS;
using Agropop.Database.Saga;
using Agropop.Database.Saga.Tables;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Saga.Dependency.DI;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AgendaDescarteLambda
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
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:descarte-saga-topic-sns";
        }

        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            LambdaLogger.Log("CONTEXT: " + JsonConvert.SerializeObject(context));
            LambdaLogger.Log("EVENT: " + JsonConvert.SerializeObject(sqsEvent));

            foreach (var message in sqsEvent.Records)
            {
                BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
                Type tipo = Type.GetType(baseMsg.TypeMsg);
                dynamic instance = Activator.CreateInstance(tipo, false);
                await HandleSagaMessage(instance, message.Body);
            }
        }

        public async Task HandleSagaMessage(LotesVencidosVerificadosEvent evento, string body)
        {

            LambdaLogger.Log("LotesVencidosVerificadosEvent: " + JsonConvert.SerializeObject(body));

            var request = JsonConvert.DeserializeObject<LotesVencidosVerificadosEvent>(body);

            AgendarRetiradaCommand agendar = new AgendarRetiradaCommand()
            {
                DataRetirada = DateTime.Now.AddDays(7), //RN
                Email = request.Email,
                IdMsr = request.Lote,
                Lote = request.Lote,
            };

            SagaIniciadaComSucessoEvent sagastart = JsonConvert.DeserializeObject<SagaIniciadaComSucessoEvent>(JsonConvert.SerializeObject(agendar));
           
            agendar.TypeMsg = agendar.GetType().AssemblyQualifiedName;
            await _sagaDynamoRepository.IncluirMensagemSaga<SagaMessageTable>(agendar.IdMsr.ToString(), agendar.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(agendar));            
            await SNSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(agendar), agendar.TypeMsg, _topicArn);
            await _sagaDynamoRepository.IncluirMensagemSaga<SagaMessageTable>(sagastart.IdMsr.ToString(), sagastart.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(sagastart));
        }

        public async Task HandleSagaMessage(AgendarRetiradaCommand comando, string body)
        {
            LambdaLogger.Log("AgendarRetiradaCommand: " + JsonConvert.SerializeObject(body));

            var request = JsonConvert.DeserializeObject<AgendarRetiradaCommand>(body);
           
            await _emailService.Enviar(request.Email, $"Favor Confirmar a Retirada Agendada Lote {request.IdMsr}", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/agendar?msgid={0}", request.IdMsr));

            //publicar event
        }
        

        public async Task HandleSagaMessage(ConfirmarAgendamentoRetiradaCommand comando, string body)
        {

            var request = JsonConvert.DeserializeObject<ConfirmarAgendamentoRetiradaCommand>(body);

            await _emailService.Enviar(request.Email, $"Retirada Confirmada Lote {request.IdMsr}. Em caso de Cancelamento", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));


            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var evento = JsonConvert.DeserializeObject<AgendamentoRetiradaConfirmadoEvent>(requestString);
            evento.TypeMsg = evento.GetType().AssemblyQualifiedName;

            await _sagaDynamoRepository.IncluirMensagemSaga<SagaMessageTable>(evento.IdMsr.ToString(),  evento.GetType().AssemblyQualifiedName, JsonConvert.SerializeObject(evento));
            await SNSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(evento), evento.TypeMsg, _topicArn);
        }
    }
}
