using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
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

        public async Task<object> BuscarMensagemAgendamento(string msgId)
        {
            DynamoDBContext context = new DynamoDBContext(_client);

            var retorno = await GetSagaMessage(context, msgId);

            return retorno.BodyMessage;
        }

        public async Task<bool> IncluirMensagemAgendamento(object msg, string msgId)
        {
            DynamoDBContext context = new DynamoDBContext(_client);
                        
            return await PutSagaMessage(context, msg, msgId);
        }


        private async Task<SagaMessageTable> GetSagaMessage(DynamoDBContext context, string msgId)
        {
            SagaMessageTable sagaItem = await context.LoadAsync<SagaMessageTable>(msgId).ConfigureAwait(false);

            return sagaItem;
        }

        private async Task<bool> PutSagaMessage(DynamoDBContext context, object msg, string msgId)
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
