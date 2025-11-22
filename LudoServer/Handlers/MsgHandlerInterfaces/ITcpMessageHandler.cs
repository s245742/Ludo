using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers.MsgHandlerInterfaces
{
    public interface ITcpMessageHandler
    {
        string MessageType { get; }
        Task<string> HandleAsync(string payload, TcpClient client);
        
    }
}
