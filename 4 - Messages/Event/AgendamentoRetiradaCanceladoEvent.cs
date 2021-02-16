using System;

namespace Descarte.Messages.Event
{
    public class AgendamentoRetiradaCanceladoEvent:BaseMessage
    {
        public string Email { get; set; }

        public Guid Lote { get; set; }

        public DateTime DataRetirada { get; set; }
    }
}
