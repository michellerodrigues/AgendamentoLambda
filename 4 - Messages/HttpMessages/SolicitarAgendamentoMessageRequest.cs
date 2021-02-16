using System;

namespace Descarte.Messages.HttpMessages
{
    public class SolicitarAgendamentoMessageRequest: BaseMessage
    {
        public Guid IdAgendamentoProposto { get; set; }
    }
}
