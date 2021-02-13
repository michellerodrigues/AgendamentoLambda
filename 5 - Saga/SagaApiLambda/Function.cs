using Agropop.AwsServices.Helper;
using Agropop.Database.Saga;
using Agropop.Database.Saga.Tables;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Saga.Dependency.DI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SagaApiLambda
{
    public class Function
    {
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
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(Stream inputStream, ILambdaContext context)
        {
            await HandleSagaMessage(inputStream);
        }

     
        public async Task HandleSagaMessage(Stream inputStream)
        {
            LambdaRequestMessage idMensagemString;

            inputStream.Position = 0;
            using (StreamReader reader = new StreamReader(inputStream, Encoding.UTF8))
            {
                string mensagemRequest = reader.ReadToEnd();

                idMensagemString = JsonConvert.DeserializeObject<LambdaRequestMessage>(mensagemRequest);
            }

            var mensagem = await _sagaDynamoRepository.BuscarMensagemAgendamento<SagaMessageTable>(idMensagemString.Parametros.querystring.msgid);

            if (mensagem != null)
            {
                //acrescentar desserialize dinamico
                AgendarRetiradaCommand message = JsonConvert.DeserializeObject<AgendarRetiradaCommand>(mensagem.Msg);

                ConfirmarAgendamentoRetiradaCommand confirmar = new ConfirmarAgendamentoRetiradaCommand()
                {
                    DataRetirada = message.DataRetirada,
                    DateMsg = DateTime.Now,
                    Email = message.Email,
                    IdMsr = message.IdMsr,
                    Lote = message.Lote,
                    TypeMsg = message.TypeMsg
                };

                confirmar.TypeMsg = confirmar.GetType().AssemblyQualifiedName;

                await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(confirmar), message.GetType().AssemblyQualifiedName, _topicArn);
             }
        }

        //public async Task<Object> FunctionHandler(LambdaRequestMessage idMensagemString, ILambdaContext context)
        //{
        //    string topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";

        //    RetiradaAgendadaEvent requestx = new RetiradaAgendadaEvent();
        //    requestx.TypeMsg = requestx.GetType().AssemblyQualifiedName;
        //    requestx.IdMsr = Guid.Parse(idMensagemString.Parametros.querystring.msgid);
        //    requestx.Email = "mica.msr@gmail.com";

        //    SagaMessageTable request = new SagaMessageTable()
        //    {
        //        IdMsg = idMensagemString.Parametros.querystring.msgid,
        //        Msg = JsonConvert.SerializeObject(requestx),
        //        TypeMsg = requestx.GetType().AssemblyQualifiedName
        //    };

        //    await _sagaDynamoRepository.IncluirMensagemAgendamento(request);

        //    var mensagem = await _sagaDynamoRepository.BuscarMensagemAgendamento<SagaMessageTable>(idMensagemString.Parametros.querystring.msgid);

        //    if (mensagem != null)
        //    {
        //        //acrescentar desserialize dinamico
        //        RetiradaAgendadaEvent message = JsonConvert.DeserializeObject<RetiradaAgendadaEvent>(mensagem.Msg);

        //        Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

        //        MessageAttributeValue values = new MessageAttributeValue()
        //        {
        //            StringValue = message.GetType().AssemblyQualifiedName,
        //            DataType = "String"
        //        };
        //        attributos.Add("typeMsg", values);

        //        await _emailService.Enviar(message.Email, $"Retirada Agendada Lote {message.IdMsr}", String.Format("Http://api-saga-gateway/api/agendarRetiradaCommand?idMsr={0}", idMensagemString.Parametros.querystring.msgid));

        //        ProcessRecordAsync(topicArn, JsonConvert.SerializeObject(message), attributos).ConfigureAwait(false).GetAwaiter().GetResult();
        //    }
        //    return mensagem;

        //}

        //private async Task ProcessRecordAsync(string topicArn, string message, Dictionary<string, MessageAttributeValue> attributos)
        //{
        //    var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.SAEast1);

        //    var request = new PublishRequest
        //    {
        //        Message = message,
        //        TopicArn = topicArn,
        //       /* TargetArn = topicArn,*/
        //        MessageAttributes = attributos
        //    };

        //    string teste = JsonConvert.SerializeObject(request);
        //    await client.PublishAsync(request);         
        //}

    }
}
