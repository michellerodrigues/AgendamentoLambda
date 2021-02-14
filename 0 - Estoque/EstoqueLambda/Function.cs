using Agropop.AwsServices.Helper;
using Agropop.Database.DataContext;
using Agropop.Database.Interfaces;
using Agropop.Database.Models;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Descarte.Messages;
using Descarte.Messages.Command;
using Descarte.Messages.Event;
using EmailHelper;
using Newtonsoft.Json;
using Saga.Dependency.DI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EstoqueLambda
{
    public class Function
    {
        public AppSettings AppSettings { get; }
        private readonly IEstoqueRepository _estoqueRepository;

        private readonly IEmailService _emailService;

        private readonly InitializeDbContext _initialize;

        private readonly string _topicArn;

        public Function()
        {
            var resolver = new DependencyResolver();
            _estoqueRepository = resolver.GetService<IEstoqueRepository>();
            _emailService = resolver.GetService<IEmailService>();
            var context = resolver.GetService<DescarteDataContext>();
            _initialize = new InitializeDbContext(context);
            _topicArn = "arn:aws:sns:sa-east-1:428672449531:descarte-saga-topic-sns";
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var message in sqsEvent.Records)
            {
                BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
                Type tipo = Type.GetType(baseMsg.TypeMsg);
                dynamic instance = Activator.CreateInstance(tipo, false);
                await HandleSagaMessage(instance, baseMsg.TypeMsg);
            }
        }

        public async Task<string> HandleSagaMessage(VerificarLotesVencidosCommand msg, string body)
        {
            var lotesVencidos = (List<Estoque>)_estoqueRepository.FindItensVencidosEstoque();
            var lotes = new List<LotesVencidosVerificadosEvent>();

            var client = new AmazonSQSClient();
            string queue = "https://sqs.sa-east-1.amazonaws.com/428672449531/estoque";

            if (lotesVencidos != null)
            {
                foreach (Estoque estoque in lotesVencidos)
                {
                    LotesVencidosVerificadosEvent lote = new LotesVencidosVerificadosEvent()
                    {
                        DateMsg = System.DateTime.Now,
                        Email = estoque.Fabricante.Email,
                        IdMsr = Guid.NewGuid(),
                        Lote = estoque.EstoqueId
                    };
                    lote.TypeMsg = lote.GetType().AssemblyQualifiedName;

                    lotes.Add(lote);
                }
                foreach (LotesVencidosVerificadosEvent lote in lotes)
                {
                    await client.SendMessageAsync(queue, JsonConvert.SerializeObject(lote)).ConfigureAwait(false);
                }
                return $"Lotes enviados para a fila : {lotes.Count}";
            }
            return $"Não existem lotes vencidos para descarte";
        }

        public async Task HandleSagaMessage(DescartarLoteEstoqueCommand msg, string body)
        {
            var request = JsonConvert.DeserializeObject<DescartarLoteEstoqueCommand>(body);

            //marcar no banco que o estoque foi descartado
            await _emailService.Enviar(request.Email, $"Seu lote {request.Lote} foi descartado com sucesso", String.Format("https://aobgkj4vt5.execute-api.sa-east-1.amazonaws.com/v1/cancelar?msgid={0}", request.IdMsr));

            string requestString = JsonConvert.SerializeObject(request);

            //publicar event
            var evento = JsonConvert.DeserializeObject<LoteDescartadoEvent>(requestString);
            evento.TypeMsg = evento.GetType().AssemblyQualifiedName;

            await AWSServices.EnviarMensgemTopico(JsonConvert.SerializeObject(evento), evento.TypeMsg, _topicArn);
        }
    }
}
