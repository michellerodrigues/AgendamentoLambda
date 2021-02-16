using System;

namespace Descarte.Messages.Event
{
    public class LotesVencidosVerificadosEvent : BaseMessage
    {
        public string Email { get; set; }
        public Guid Lote { get; set; }
    }
}
