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

namespace TriagemLambda
{
    public class Function
    {
        private readonly IEmailService _emailService;
        private readonly ISagaDynamoRepository _sagaDynamoRepository;
        private readonly string _topicArn;
        public Function()
        {
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:descarte-saga-topic-sns";
            var resolver = new DependencyResolver();
            _emailService = resolver.GetService<IEmailService>();
            _sagaDynamoRepository = resolver.GetService<ISagaDynamoRepository>();
        }
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var message in sqsEvent.Records)
            {
                BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
                Type tipo = Type.GetType(baseMsg.TypeMsg);
                dynamic instance = Activator.CreateInstance(tipo, false);
                await HandleSagaMessage(instance, message.Body);
            }           
        }


        public async Task HandleSagaMessage(AgendamentoRetiradaConfirmadoEvent msg, string body)
        {
            var request = JsonConvert.DeserializeObject<AgendamentoRetiradaConfirmadoEvent>(body); 
           
            await _emailService.Enviar(request.Email, $"Seu lote {request.Lote} foi enviado para a Triagem. Em caso de Cancelamento", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));
            
            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var command = JsonConvert.DeserializeObject<RealizarTriagemCommand>(requestString);
            command.TypeMsg = command.GetType().AssemblyQualifiedName;

            await _sagaDynamoRepository.IncluirMensagemSaga<SagaMessageTable>(command.IdMsr.ToString(), JsonConvert.SerializeObject(command), command.GetType().AssemblyQualifiedName);

            await SNSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(command), command.TypeMsg, _topicArn);
        }

        public async Task HandleSagaMessage(RealizarTriagemCommand msg, string body)
        {
            var request = JsonConvert.DeserializeObject<RealizarTriagemCommand>(body);

            await _emailService.Enviar(request.Email, $"Seu lote {request.Lote} está sendo preparado para descarte. Em caso de Cancelamento", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));
            
            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var evento = JsonConvert.DeserializeObject<TriagemRealizadaEvent>(requestString);
            evento.TypeMsg = evento.GetType().AssemblyQualifiedName;

            await _sagaDynamoRepository.IncluirMensagemSaga<SagaMessageTable>(evento.IdMsr.ToString(), JsonConvert.SerializeObject(evento), evento.GetType().AssemblyQualifiedName);

            await SNSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(evento), evento.TypeMsg, _topicArn);
        }
        
    }
}
