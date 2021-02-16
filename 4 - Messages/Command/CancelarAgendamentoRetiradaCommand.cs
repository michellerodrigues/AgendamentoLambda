using System;
using System.Collections.Generic;
using System.Text;

namespace Descarte.Messages.Command
{
    public class CancelarAgendamentoRetiradaCommand:BaseMessage
    {
        public string Email { get; set; }

        public Guid Lote { get; set; }

        public DateTime DataRetirada { get; set; }
    }
}
