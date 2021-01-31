using Agropop.Database.DataContext;
using Agropop.Database.Interfaces;
using Agropop.Database.Models;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.SQS;
using Descarte.Messages;
using Descarte.Messages.Command;
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

        public Function()
        {
            var resolver = new DependencyResolver();
            _estoqueRepository = resolver.GetService<IEstoqueRepository>();
            _emailService = resolver.GetService<IEmailService>();
            var context = resolver.GetService<DescarteDataContext>();
            _initialize = new InitializeDbContext(context);
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        //public async Task FunctionHandler(int i, ILambdaContext context)
        {
            BaseMessage baseMsg = JsonConvert.DeserializeObject<BaseMessage>(message.Body);
            Type tipo = Type.GetType(baseMsg.TypeMsg);
            dynamic instance = Activator.CreateInstance(tipo, false);
            HandleSagaMessage(instance);
        }

        ////public async Task FunctionHandler(SQSEvent.SQSMessage message, ILambdaContext context)
        //public async Task FunctionHandler(int i, ILambdaContext context)
        //{
        //    await _emailService.Enviar("mica.msr@gmail.com", "teste envio lambda", "oi, esta é uma mensagem com acento e quebra de linha \n");

        //    await Task.CompletedTask;

        //}

        public async Task<string> HandleSagaMessage(VerificarLotesVencidosCommand msg)
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

        public async Task<string> HandleSagaMessage(LotesVencidosVerificadosEvent msg)
        {
            var lotesVencidos = (List<Estoque>)_estoqueRepository.AtualizarLotesEnviadosParaDescarte(msg.Lote);

            var client = new AmazonSQSClient();

            string queue = "https://sqs.sa-east-1.amazonaws.com/428672449531/agendamento";

            AgendarRetiradaCommand agendarRetirada = new AgendarRetiradaCommand()
            {
                DateMsg = System.DateTime.Now,
                Email = msg.Email,
                IdMsr = msg.IdMsr,
                Lote = msg.Lote
            };
            agendarRetirada.TypeMsg = agendarRetirada.GetType().AssemblyQualifiedName;


            var retorno = await client.SendMessageAsync(queue, JsonConvert.SerializeObject(agendarRetirada)).ConfigureAwait(false);

            return retorno.MessageId;
        }
    }
}
