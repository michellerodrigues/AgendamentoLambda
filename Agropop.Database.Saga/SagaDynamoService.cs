using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Descarte.Messages;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Agropop.Database.Saga
{
    public class SagaDynamoRepository : ISagaDynamoRepository
    {
        private readonly AmazonDynamoDBClient _client;

        private readonly AwsConfigOptions _awsConfigOptions;
        public SagaDynamoRepository(AmazonDynamoDBClient client, AwsConfigOptions awsConfigOptions)
        {
            _awsConfigOptions = awsConfigOptions;
            _client = client;            
        }

        public async Task<T> BuscarMensagemAgendamento<T>(string msgId) where T : SagaMessageTable
        {
            DynamoDBContext context = new DynamoDBContext(_client);

            var retorno = await GetSagaMessage<T>(context, msgId);

            return retorno as T;
        }

        public async Task<bool> IncluirMensagemAgendamento<T>(T msg) where T : SagaMessageTable
        {
            DynamoDBContext context = new DynamoDBContext(_client);
                        
            return await PutSagaMessage<T>(context, msg);
        }
        
        private async Task<SagaMessageTable> GetSagaMessage<T>(DynamoDBContext context, string msgId) where T : SagaMessageTable
        {
            SagaMessageTable sagaItem = await context.LoadAsync<SagaMessageTable>(msgId).ConfigureAwait(false);

           // dynamic retorno = JsonConvert.DeserializeObject<T>(sagaItem.Message);

            return sagaItem;
        }

        private async Task<bool> PutSagaMessage<T>(DynamoDBContext context, T msg) where T : SagaMessageTable
        {

            SagaMessageTable sampleTableItems = new SagaMessageTable
            {
                IdMsg = msg.IdMsg.ToString(),
                Msg = msg.Msg,
                TypeMsg = msg.TypeMsg
            };

            var batchWrite = context.CreateBatchWrite<SagaMessageTable>();

            batchWrite.AddPutItem(sampleTableItems);
            await batchWrite.ExecuteAsync();
            
            return true;
        }
    }
}
