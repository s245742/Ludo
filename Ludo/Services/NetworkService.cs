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

        public async Task ConnectAsync(string host, int port)
        {
            _client = new TcpClient();
            await _client.ConnectAsync(host, port);
            _stream = _client.GetStream();
            Console.WriteLine("Connected to server!");
        }

        public async Task SendMessageAsync(string message)
        {
            if (_stream == null) return;

            var data = Encoding.UTF8.GetBytes(message);
            await _stream.WriteAsync(data, 0, data.Length);

            //read the respons
            var buffer = new byte[1024];
            int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Server response: {response}");
        }
    }
}