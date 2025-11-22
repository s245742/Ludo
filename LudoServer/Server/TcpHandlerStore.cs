using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Session;
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
        public GameSessionManager SessionManager { get; }
        public TcpHandlerStore(IEnumerable<ITcpMessageHandler> handlers, GameSessionManager sessionManager)
        {
            _handlers = handlers.ToList();
            SessionManager = sessionManager;
        }

        public ITcpMessageHandler? GetHandler(string messageType)
        {
            return _handlers.FirstOrDefault(h => h.MessageType == messageType);
        }
    }
}
