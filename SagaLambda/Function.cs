using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;


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
        public async Task<string> FunctionHandler(int input, ILambdaContext context)
        {
            string topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";

            VerificarLotesVencidosCommand message = new VerificarLotesVencidosCommand();
            message.TypeMsg = message.GetType().AssemblyQualifiedName;

            Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

            byte[] byteArray = Encoding.UTF8.GetBytes(message.GetType().AssemblyQualifiedName);

            var stream = new MemoryStream(byteArray);
            stream.Position = 0;

            MessageAttributeValue attrib = new MessageAttributeValue()
            {
                StringValue = message.GetType().AssemblyQualifiedName,
                DataType = "String"
            };

            attributos.Add("typeMsg", attrib);

            return await PublicarNoTopico(topicArn, JsonConvert.SerializeObject(message), attributos);
        }

        public async Task<string> PublicarNoTopico(string topicArn, string message, Dictionary<string, MessageAttributeValue> attributos)
        {
            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.SAEast1);

            var request = new PublishRequest()
            {
                Message = message,
                MessageAttributes= attributos,
                TopicArn = topicArn
            };

            string teste = JsonConvert.SerializeObject(request);
            await client.PublishAsync(request);

            return $"mensagem publicada { teste}";
        }
    }
}
