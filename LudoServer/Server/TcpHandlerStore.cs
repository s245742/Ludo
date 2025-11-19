using LudoServer.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Server
{
    public class TcpHandlerStore
    {
        private List<ITcpMessageHandler> _handlers;

        public TcpHandlerStore(IEnumerable<ITcpMessageHandler> handlers)
        {
            _handlers = handlers.ToList();
        }

        public ITcpMessageHandler? GetHandler(string messageType)
        {
            return _handlers.FirstOrDefault(h => h.MessageType == messageType);
        }
    }
}
