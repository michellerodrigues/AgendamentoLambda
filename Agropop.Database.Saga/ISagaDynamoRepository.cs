using Agropop.Database.Saga.Tables;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Threading.Tasks;

namespace Agropop.Database.Saga
{
    public interface ISagaDynamoRepository
    {
        public Task<object> BuscarMensagemAgendamento(string msgId);
        public Task<bool> IncluirMensagemAgendamento(object msg, string msgId);               
    }
}
