using System;

namespace Descarte.Messages.Command
{
    public class DescartarLoteEstoqueCommand : BaseMessage
    {
        public string Email { get; set; }

        public Guid Lote { get; set; }

        public DateTime DataRetirada { get; set; }
    }
}
