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
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:descarte-saga-topic-sns";
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

                await SNSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(confirmar), message.GetType().AssemblyQualifiedName, _topicArn);
             }
        }
    }
}
