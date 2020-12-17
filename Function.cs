using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Descarte.Messages.Event;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AgendamentoLambda
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
        public async Task FunctionHandler(SQSEvent evnt, ILambdaContext context)
        {
            foreach(var message in evnt.Records)
            {
                await ProcessMessageAsync(message, context);
            }
        }

        private async Task ProcessMessageAsync(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            context.Logger.LogLine($"Processed message '{message.Body}'");

            // TODO: Do interesting work based on the new message

            var client = new AmazonSQSClient();
            string topic = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento_events_sqs";

            RetiradaAgendadaEvent evento = new RetiradaAgendadaEvent()
            {
                DateMsg = DateTime.Now,
                ReceivedMessage = message.Body
            };

            await client.SendMessageAsync(topic, JsonConvert.SerializeObject(evento)).ConfigureAwait(false);

            await Task.CompletedTask;
        }
    }
}
