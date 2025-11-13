using LudoServer.Services;
using Microsoft.Win32;
using SharedModels.Models;
using SharedModels.TransferMsg;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Server
{
    public class GameServer
    {
        private TcpListener _listener;
        private bool _running = false;
        private readonly HandlerStore _handlerStore;



        public GameServer(int port, HandlerStore handlerStore)
        {

            _listener = new TcpListener(IPAddress.Any, port);
            _handlerStore = handlerStore;
        }


        public async Task StartAsync()
        {
            _listener.Start();
            _running = true;
            Console.WriteLine("Server started...");

            while (_running)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Console.WriteLine("Client connected!");
                _ = HandleClientAsync(client);
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            var stream = client.GetStream();
            var buffer = new byte[4096];

            while (client.Connected)
            {
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                if (bytesRead == 0) break;

                var json = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var envelope = System.Text.Json.JsonSerializer.Deserialize<MessageEnvelope>(json);
               
                if (envelope != null)
                {
                    //get the hanlder matching the msgtype
                    
                    var handler = _handlerStore.GetHandler(envelope.MessageType);
                    if (handler != null)
                        //we can now handle the data
                        
                    await handler.HandleAsync(envelope.Payload);
                }

                var response = Encoding.UTF8.GetBytes("Server processed your message!\n");
                
                await stream.WriteAsync(response, 0, response.Length);
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}