using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Descarte.Messages;
using System.Threading.Tasks;

namespace Agropop.Database.Saga
{
    public interface ISagaDynamoRepository
    {
        public Task<BaseMessage> BuscarMensagemAgendamento(string msgId);
        public Task<bool> IncluirMensagemAgendamento(BaseMessage msg);               
    }
}
