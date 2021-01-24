using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;

namespace Agropop.Database.Saga
{
    public class SagaDynamoService
    {
        private static AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public async static Task<string> BuscarMensagemAgendamento(string msgId)
        {
            DynamoDBContext context = new DynamoDBContext(client);

            var retorno = await GetSagaMessage(context, msgId);

            return retorno.BodyMessage;
        }

        public async static Task<bool> IncluirMensagemAgendamento(string msg, string msgId)
        {
            DynamoDBContext context = new DynamoDBContext(client);
                        
            return await PutSagaMessage(context, msg, msgId);
        }


        private async static Task<SagaMessageTable> GetSagaMessage(DynamoDBContext context, string msgId)
        {
            SagaMessageTable sagaItem = await context.LoadAsync<SagaMessageTable>(msgId).ConfigureAwait(false);

            return sagaItem;
        }

        private async static Task<bool> PutSagaMessage(DynamoDBContext context, string msg, string msgId)
        {
            
            SagaMessageTable sampleTableItems = new SagaMessageTable
            {
                BodyMessage = msg,
                Id = msgId
            };

            var batchWrite = context.CreateBatchWrite<SagaMessageTable>();

            batchWrite.AddPutItem(sampleTableItems);

            return true;
        }
    }
}
