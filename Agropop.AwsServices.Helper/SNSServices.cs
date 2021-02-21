using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Agropop.AwsServices.Helper.SNS
{
    public class SNSServices
    {
        public static async Task EnviarMensgemTopico(string message, string assemblyQualifiedName, string topicArn)
        {
            Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

            MessageAttributeValue values = new MessageAttributeValue()
            {
                StringValue = assemblyQualifiedName,
                DataType = "String"
            };
            attributos.Add("typeMsg", values);

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
