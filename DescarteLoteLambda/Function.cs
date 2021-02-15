using Agropop.AwsServices.Helper;
using Agropop.Database.Interfaces;
using Agropop.Database.Saga;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Saga.Dependency.DI;
using System;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: Amazon.Lambda.Core.LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DescarteLoteLambda
{
    public class Function
    {
        private readonly ISagaDynamoRepository _sagaDynamoRepository;
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IEmailService _emailService;
        private readonly string _topicArn;

        public Function()
        {
            var resolver = new DependencyResolver();
            _sagaDynamoRepository = resolver.GetService<ISagaDynamoRepository>();
            _estoqueRepository = resolver.GetService<IEstoqueRepository>();
            _emailService = resolver.GetService<IEmailService>();
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:descarte-saga-topic-sns";
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

        public async Task HandleSagaMessage(TriagemRealizadaEvent msg, string body)
        {
            var request = JsonConvert.DeserializeObject<TriagemRealizadaEvent>(body);

            await _emailService.Enviar(request.Email, $"Seu lote {request.Lote} já pode ser retirado. Em caso de Cancelamento", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));


            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var evento = JsonConvert.DeserializeObject<DescartarLoteEstoqueCommand>(requestString);
            evento.TypeMsg = evento.GetType().AssemblyQualifiedName;

            await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(evento), evento.TypeMsg, _topicArn);
        }

        public async Task HandleSagaMessage(LoteDescartadoEvent msg, string body)
        {
            var request = JsonConvert.DeserializeObject<LoteDescartadoEvent>(body);

            //marcar como descartado no banco de dados sql do estoque
            string requestString = JsonConvert.SerializeObject(request);

            await _emailService.Enviar(request.Email, $"Seu lote {request.Lote} foi entregue com sucesso. Obrigada por contribuir para a natureza", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));

            //salvar no dynamo que está finalizado
        }
    }
}
