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
        private readonly TcpHandlerStore _tcpHandlerStore;



        public GameServer(int port, HandlerStore handlerStore, TcpHandlerStore tcpHandlerStore)
        {

            _listener = new TcpListener(IPAddress.Any, port);
            _handlerStore = handlerStore;
            _tcpHandlerStore = tcpHandlerStore;
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

            try
            {
                while (client.Connected)
                {
                    // Read 4-byte length prefix
                    var lengthBuffer = new byte[4];
                    int read;
                    
                    try
                    {
                        read = await stream.ReadAsync(lengthBuffer, 0, 4);
                    }
                    catch
                    {
                        Console.WriteLine("Client read error – disconnecting safely.");
                        break;
                    }

                    if (read == 0)
                    {
                        Console.WriteLine("Client closed connection before sending data.");
                        break;
                    }
                    if (read < 4)
                    {
                        Console.WriteLine("Client sent incomplete prefix – disconnecting.");
                        break;
                    }

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
                        // try regular handlers
                        var handler = _handlerStore.GetHandler(envelope.MessageType);
                        if (handler != null)
                        {
                            handlerResult = await handler.HandleAsync(envelope.Payload);
                        }
                        else
                        {
                            // try TCP handlers
                            var tcpHandler = _tcpHandlerStore.GetHandler(envelope.MessageType);
                            if (tcpHandler != null)
                            {
                                handlerResult = await tcpHandler.HandleAsync(envelope.Payload, client);
                            }
                        }
                    }

                    // Send response with length prefix
                    var responseBytes = Encoding.UTF8.GetBytes(handlerResult);
                    var prefix = BitConverter.GetBytes(responseBytes.Length);
                    await stream.WriteAsync(prefix, 0, 4);
                    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Client error: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("Client disconnected.");


                _tcpHandlerStore.SessionManager.RemoveByClient(client);

                client.Close();
                client.Dispose();
            }
        }
    }
}