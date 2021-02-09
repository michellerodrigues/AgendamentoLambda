using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Descarte.Messages;
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

        public async Task<BaseMessage> BuscarMensagemAgendamento(string msgId)
        {
            DynamoDBContext context = new DynamoDBContext(_client);

            var retorno = await GetSagaMessage(context, msgId);

            return retorno;
        }

        public async Task<bool> IncluirMensagemAgendamento(BaseMessage msg)
        {
            DynamoDBContext context = new DynamoDBContext(_client);
                        
            return await PutSagaMessage(context, msg);
        }


        private async Task<BaseMessage> GetSagaMessage(DynamoDBContext context, string msgId)
        {
            BaseMessage sagaItem = await context.LoadAsync<BaseMessage>(msgId).ConfigureAwait(false);

            return sagaItem;
        }

        private async Task<bool> PutSagaMessage(DynamoDBContext context, BaseMessage msg)
        {
            
            //SagaMessageTable sampleTableItems = new SagaMessageTable
            //{
            //    IdMsr = msgId
            //};

            var batchWrite = context.CreateBatchWrite<BaseMessage>();

            batchWrite.AddPutItem(msg);

            return true;
        }
    }
}
