using Agropop.AwsServices.Helper;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.HttpMessages;
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
        /// 
        private readonly string _topicArn;
        public Function()
        {
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:DescarteSagaTopic";
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
           
            VerificarLotesVencidosCommand message = new VerificarLotesVencidosCommand();
            message.TypeMsg = message.GetType().AssemblyQualifiedName;

            await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(message), message.GetType().AssemblyQualifiedName, _topicArn);
                     

            return $"mensagem publicada { JsonConvert.SerializeObject(message)}";
        }

        /////// <summary>
        /////// A simple function that takes a string and does a ToUpper
        /////// </summary>
        /////// <param name="input"></param>
        /////// <param name="context"></param>
        /////// <returns></returns>
        //////public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        //public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        //{
        //    BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
        //    Type tipo = Type.GetType(baseMsg.TypeMsg);
        //    dynamic instance = Activator.CreateInstance(tipo, false);
        //    HandleSagaMessage(instance);
        //}
        //public static string HandleSagaMessage(SolicitarAgendamentoMessageRequest request)
        //{
        //    return "AgendarRetiradaCommand ok";
        //}
    }
}
