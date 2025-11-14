using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public interface IMessageHandler
    {
        string MessageType { get; }
        Task<string> HandleAsync(string payload);
    }
}
