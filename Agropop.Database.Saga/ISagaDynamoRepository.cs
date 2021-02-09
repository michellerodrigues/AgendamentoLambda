using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Descarte.Messages;
using System.Threading.Tasks;

namespace Agropop.Database.Saga
{
    public interface ISagaDynamoRepository
    {
        public Task<SagaMessageTable> BuscarMensagemAgendamento<T>(string msgId) where T : BaseMessage;
        public Task<bool> IncluirMensagemAgendamento<T>(T msg) where T : BaseMessage;
    }
}
