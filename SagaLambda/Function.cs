using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages.Command;
using Newtonsoft.Json;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SagaLambda
{
    public class Function
    {
        /// <summary>
        /// Default constructor. This constructor is used by Lambda to construct the instance. When invoked in a Lambda environment
        /// the AWS credentials will come from the IAM role associated with the function and the AWS region will be set to the
        /// region the Lambda function is executed in.
        /// </summary>
        public Function()
        {

        }


        /// <summary>
        /// This method is called for every Lambda invocation. This method takes in an SQS event object and can be used 
        /// to respond to SQS messages.
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(string input, ILambdaContext context)
        {
            string topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";

            VerificarLotesVencidosParaDescartarCommand message = new VerificarLotesVencidosParaDescartarCommand();
            message.TypeMsg = message.GetType().AssemblyQualifiedName;

            Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

            attributos.Add("typeMsg", new MessageAttributeValue() { StringValue = message.GetType().AssemblyQualifiedName });

            PublicarNoTopico(topicArn, JsonConvert.SerializeObject(message), attributos);
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed message {message.Body}");

            // TODO: Do interesting work based on the new message
            await Task.CompletedTask;
        }

        public string PublicarNoTopico(string topicArn, string message, Dictionary<string, MessageAttributeValue> attributos)
        {
            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.EUSouth1);

            var request = new PublishRequest
            {
                Message = message,
                TopicArn = topicArn,
                MessageAttributes = attributos
            };

            client.PublishAsync(request);

            return "Mensagem Publicada com sucesso";
        }
    }
}
