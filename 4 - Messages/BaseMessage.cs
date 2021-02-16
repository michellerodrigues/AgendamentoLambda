using System;
using System.Collections.Generic;
using System.Text;

namespace Descarte.Messages
{
    public class BaseMessage
    {
        public Guid IdMsr { get; set; } = Guid.NewGuid();

        public DateTime DateMsg { get; set; } = DateTime.UtcNow;

        public string TypeMsg { get; set; }

        public string BodyMsgQueue { get; set; }
    }
}
