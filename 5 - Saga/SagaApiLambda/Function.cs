using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Saga.Dependency.DI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SagaApiLambda
{
    public class Function
    {
        private readonly IEmailService _emailService;
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {
            var resolver = new DependencyResolver();
            _emailService = resolver.GetService<IEmailService>();
        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SNS event object and can be used 
        /// to respond to SNS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(Object mensagem, ILambdaContext context)
        {
            string topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";
            string idMensagemString = JsonConvert.SerializeObject(mensagem);

            BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(idMensagemString);

            RetiradaAgendadaEvent message = new RetiradaAgendadaEvent();
            message.TypeMsg = message.GetType().AssemblyQualifiedName;
            message.IdMsr = baseMsg.IdMensagem;
            message.Email = "mica.msr@gmail.com";

            Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

            MessageAttributeValue values = new MessageAttributeValue()
            {
                StringValue = message.GetType().AssemblyQualifiedName,
                DataType="String"
            };
            attributos.Add("typeMsg", values);

            await _emailService.Enviar(message.Email, $"Retirada Agendada Lote {message.IdMsr}", String.Format("Http://api-saga-gateway/api/agendarRetiradaCommand?idMsr={0}", message.IdMsr));

            // await ProcessRecordAsync(topicArn, JsonConvert.SerializeObject(message), attributos);

        }

        private async Task ProcessRecordAsync(string topicArn, string message, Dictionary<string, MessageAttributeValue> attributos)
        {
            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.SAEast1);

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = topicArn,
               /* TargetArn = topicArn,*/
                MessageAttributes = attributos
            };

            string teste = JsonConvert.SerializeObject(request);
            await client.PublishAsync(request);         
        }
    }
}
