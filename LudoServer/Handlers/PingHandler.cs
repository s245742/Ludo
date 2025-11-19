using LudoServer.Session;
using SharedModels.TransferMsg;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class PingHandler : ITcpMessageHandler
    {
        private readonly GameSessionManager _sessionManager;
        public string MessageType => "Ping";

        public PingHandler(GameSessionManager manager)
        {
            _sessionManager = manager;
        }

        public async Task<string> HandleAsync(string payload, TcpClient senderClient)
        {
            string message = JsonSerializer.Deserialize<string>(payload);

            foreach (var session in _sessionManager.GetAllSessions())
            {
                foreach (var kvp in session.PlayerConnections)
                {
                    var client = kvp.Value;
                    if (client.Connected)
                    {
                        await SendToClient(client, new MessageEnvelope
                        {
                            MessageType = "PingResponse",
                            Payload = JsonSerializer.Serialize(message)
                        });
                    }
                }
            }

            return "\"Ping delivered\"";
        }

        private async Task SendToClient(TcpClient client, MessageEnvelope envelope)
        {
            string json = JsonSerializer.Serialize(envelope);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(bytes.Length);

            var stream = client.GetStream();
            await stream.WriteAsync(length, 0, length.Length);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}
