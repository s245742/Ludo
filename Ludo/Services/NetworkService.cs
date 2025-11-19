using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LudoClient.Services
{
    public class NetworkService
    {
        private TcpClient _client;
        private NetworkStream _stream;
        public bool IsConnected => _client?.Connected ?? false;
        public async Task ConnectAsync(string host, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);
            _stream = _client.GetStream();
            Console.WriteLine("Connected to server!");
        }

        public async Task<string> SendMessageAsync(string message)
        {
            if (_stream == null) return "not connected";

            //Convert message to bytes
            var data = Encoding.UTF8.GetBytes(message);
            var lengthBytes = BitConverter.GetBytes(data.Length);

            //Send 4-byte length prefix + data
            await _stream.WriteAsync(lengthBytes, 0, lengthBytes.Length);
            await _stream.WriteAsync(data, 0, data.Length);

            // Read 4-byte length prefix
            var lenBuffer = new byte[4];
            int read = await _stream.ReadAsync(lenBuffer, 0, 4);
            if (read < 4) throw new Exception("Failed to read message length from server");

            int responseLength = BitConverter.ToInt32(lenBuffer, 0);
            var responseBuffer = new byte[responseLength];
            int totalRead = 0;

            while (totalRead < responseLength)
            {
                int chunk = await _stream.ReadAsync(responseBuffer, totalRead, responseLength - totalRead);
                if (chunk == 0) throw new Exception("Server disconnected during message read");
                totalRead += chunk;
            }

            return Encoding.UTF8.GetString(responseBuffer);
        }

        // Continuous listening for messages from server
        public async Task StartListeningAsync(Action<string> onMessageReceived)
        {
            if (_stream == null) return;

            await Task.Yield(); // ensures loop starts asynchronously
            try
            {
                while (_client.Connected)
                {
                    var lenBuffer = new byte[4];
                    int read = await _stream.ReadAsync(lenBuffer, 0, 4).ConfigureAwait(false);
                    if (read == 0) break; // client disconnected
                    if (read < 4) throw new Exception("Invalid message length");

                    int msgLength = BitConverter.ToInt32(lenBuffer, 0);
                    var msgBuffer = new byte[msgLength];
                    int totalRead = 0;

                    while (totalRead < msgLength)
                    {
                        int chunk = await _stream.ReadAsync(msgBuffer, totalRead, msgLength - totalRead).ConfigureAwait(false);
                        if (chunk == 0) break;
                        totalRead += chunk;
                    }

                    string message = Encoding.UTF8.GetString(msgBuffer);
                    onMessageReceived?.Invoke(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Listening error: " + ex.Message);
            }
        }

        private void OnMessageReceived(string msg)
        {
            Console.WriteLine("Received: " + msg);
        }
        // Fire-and-forget send (no blocking read)
        public async Task SendAsync(string message)
        {
            if (_stream == null) return;

            var data = Encoding.UTF8.GetBytes(message);
            var lengthBytes = BitConverter.GetBytes(data.Length);

            await _stream.WriteAsync(lengthBytes, 0, lengthBytes.Length).ConfigureAwait(false);
            await _stream.WriteAsync(data, 0, data.Length).ConfigureAwait(false);
        }


    }
}