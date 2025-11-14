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

            while (client.Connected)
            {
                // Read 4-byte length prefix
                var lengthBuffer = new byte[4];
                int read = await stream.ReadAsync(lengthBuffer, 0, 4);
                if (read == 0) break; // client disconnected
                if (read < 4) throw new Exception("Invalid message length received");

                int messageLength = BitConverter.ToInt32(lengthBuffer, 0);
                var messageBuffer = new byte[messageLength];
                int totalRead = 0;

                while (totalRead < messageLength)
                {
                    int chunk = await stream.ReadAsync(messageBuffer, totalRead, messageLength - totalRead);
                    if (chunk == 0) throw new Exception("Client disconnected during message read");
                    totalRead += chunk;
                }

                string json = Encoding.UTF8.GetString(messageBuffer);
                var envelope = System.Text.Json.JsonSerializer.Deserialize<MessageEnvelope>(json);

                string handlerResult = "No handler found";

                if (envelope != null)
                {
                    var handler = _handlerStore.GetHandler(envelope.MessageType);
                    if (handler != null)
                        handlerResult = await handler.HandleAsync(envelope.Payload);
                }

                // Send response back with length prefix
                var responseBytes = Encoding.UTF8.GetBytes(handlerResult);
                var lengthPrefix = BitConverter.GetBytes(responseBytes.Length);
                await stream.WriteAsync(lengthPrefix, 0, 4);
                await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
            }

            client.Close();
            Console.WriteLine("Client disconnected.");
        }
    }
}