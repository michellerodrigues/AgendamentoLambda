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

        public async Task<SagaMessageTable> BuscarMensagemAgendamento<T>(string msgId) where T : BaseMessage
        {
            DynamoDBContext context = new DynamoDBContext(_client);

            var retorno = await GetSagaMessage<T>(context, msgId);

            return retorno;
        }

        public async Task<bool> IncluirMensagemAgendamento<T>(T msg) where T : BaseMessage
        {
            DynamoDBContext context = new DynamoDBContext(_client);
                        
            return await PutSagaMessage(context, msg);
        }


        private async Task<SagaMessageTable> GetSagaMessage<T>(DynamoDBContext context, string msgId) where T : BaseMessage
        {
            SagaMessageTable sagaItem = await context.LoadAsync<SagaMessageTable>(msgId).ConfigureAwait(false);

           // dynamic retorno = JsonConvert.DeserializeObject<T>(sagaItem.Message);

            return sagaItem;
        }

        private async Task<bool> PutSagaMessage<T>(DynamoDBContext context, T msg) where T : BaseMessage
        {

            SagaMessageTable sampleTableItems = new SagaMessageTable
            {
                IdMsg = msg.IdMsr.ToString(),
              //  TypeMessage = msg.TypeMsg,
             //   Message = JsonConvert.SerializeObject(msg)
            };

            var batchWrite = context.CreateBatchWrite<SagaMessageTable>();

            batchWrite.AddPutItem(sampleTableItems);

            return true;
        }
    }
}
