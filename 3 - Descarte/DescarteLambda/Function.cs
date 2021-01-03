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
        public string FunctionHandler(string input, ILambdaContext context)
        {
            BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(input);
            
            Type tipo = Type.GetType(baseMsg.TypeMsg);
            
            ConstructorInfo constructor = tipo.GetConstructor(Type.EmptyTypes);
            
            object msgObject = constructor.Invoke(new object[] { });
                        
            MethodInfo msgMethod = this.GetType().GetMethod("HandleSagaMessage");
            
            var instance = Activator.CreateInstance(tipo, false);

            instance = JsonConvert.DeserializeObject(input);

            object msgValue = msgMethod.Invoke(msgObject, new object[] { instance });

            return (string) msgValue;
        }

        private string HandleSagaMessage(LoteVencidoParaDescartarCommand lote)
        {
            return "bind ok";
        }

        //public virtual T Cast<T>(object entity) where T : class
        //{
        //    return entity as T;
        //}
    }
}
