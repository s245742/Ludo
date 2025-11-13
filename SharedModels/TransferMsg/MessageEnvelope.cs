using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels.TransferMsg
{
    public class MessageEnvelope
    {
        public string MessageType { get; set; } // e.g., "CreateGame"
        public string Payload { get; set; } // JSON 

    }
}
