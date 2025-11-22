using LudoServer.Handlers.MsgHandlerInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Server
{
    public class HandlerStore
    {
        private List<IMessageHandler> _handlers;

        public HandlerStore(IEnumerable<IMessageHandler> handlers)
        {
            _handlers = handlers.ToList();
        }

        //Iterate handlers until we find the one matching the messagetype
        public IMessageHandler? GetHandler(string messageType)
        {
            foreach (var handler in _handlers)
            {
                if (handler.MessageType == messageType)
                    return handler;
            }
            return null;
        }
    }
}
