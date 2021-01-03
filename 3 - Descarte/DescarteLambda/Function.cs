using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Descarte.Messages;
using Descarte.Messages.Command;
using Newtonsoft.Json;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace DescarteLambda
{
    public class Function
    {
        
        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(int input, ILambdaContext context)
        {
            string inputString= "{\"Email\": \"mica-fabricante2@mailinator.com\",\"Lote\": \"9bd89eeb-ee99-4f46-b461-93b9b495eeb9\",\"IdMsr\": \"41b02429-ad8b-4493-8656-31f0fbea51a3\",\"DateMsg\": \"2021-01-03T12:27:40.150953-03:00\", \"TypeMsg\": \"Descarte.Messages.Command.LoteVencidoParaDescartarCommand, Descarte.Messages, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null\"}";
            
            BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(inputString);           
            
            Type tipo = Type.GetType(baseMsg.TypeMsg);

            dynamic instance = Activator.CreateInstance(tipo, false);

            return HandleSagaMessage(instance);
        }

        public static string HandleSagaMessage(LoteVencidoParaDescartarCommand lote)
        {
            return "LoteVencidoParaDescartarCommand ok";
        }

        public static string HandleSagaMessage(AgendarRetiradaCommand lote)
        {
            return "AgendarRetiradaCommand ok";
        }

    }
}
