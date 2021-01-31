using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Agropop.Database.Saga;
using Amazon.Lambda.Core;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using Newtonsoft.Json;
using Saga.Dependency.DI;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DescarteLambda
{


    public class Function
    {
        private readonly ISagaDynamoRepository _sagaDynamoRepository;

        public Function()
        {
            var resolver = new DependencyResolver();
            _sagaDynamoRepository = resolver.GetService<ISagaDynamoRepository>();
        }
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandler(string idMsg, ILambdaContext context)
        {

            //buscar id da mensagem no dynamo

            string topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";

            RetiradaAgendadaEvent retiradaAgendadaEvent = new RetiradaAgendadaEvent()
            {
                IdMsr = Guid.Parse(idMsg),
                Email = "mica.msr@gmail.com"
            };
            retiradaAgendadaEvent.TypeMsg = retiradaAgendadaEvent.GetType().AssemblyQualifiedName;


            Dictionary<string, MessageAttributeValue> attributos = new Dictionary<string, MessageAttributeValue>();

            byte[] byteArray = Encoding.UTF8.GetBytes(retiradaAgendadaEvent.GetType().AssemblyQualifiedName);

            var stream = new MemoryStream(byteArray);
            stream.Position = 0;

            MessageAttributeValue attrib = new MessageAttributeValue()
            {
                StringValue = retiradaAgendadaEvent.GetType().AssemblyQualifiedName,
                DataType = "String"
            };

            attributos.Add("typeMsg", attrib);

            return await PublicarNoTopico(topicArn, JsonConvert.SerializeObject(retiradaAgendadaEvent), attributos);
        }

        public async Task<string> PublicarNoTopico(string topicArn, string message, Dictionary<string, MessageAttributeValue> attributos)
        {
            var client = new AmazonSimpleNotificationServiceClient(region: Amazon.RegionEndpoint.SAEast1);

            var request = new PublishRequest()
            {
                Message = message,
                MessageAttributes = attributos,
                TopicArn = topicArn
            };

            string teste = JsonConvert.SerializeObject(request);
            await client.PublishAsync(request);

            return $"mensagem publicada { teste}";
        }
    }
}
