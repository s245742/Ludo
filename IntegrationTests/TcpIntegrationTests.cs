using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Server;
using LudoServer.Session;
using SharedModels.TransferMsg;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

//Fake test TCP handler
public class TestTcpHandler : ITcpMessageHandler
{
    public string MessageType => "TestMsg";

    public Task<string> HandleAsync(string payload, TcpClient client)
    {
        return Task.FromResult("SERVER_RECEIVED:" + payload);
    }
}

public class TcpIntegrationTests
{
    [Fact]
    public async Task ServerClient_Roundtrip_Works()
    {
        
        int port = 5051;
        var sessionManager = new GameSessionManager();
        var handlerStore = new HandlerStore(new List<IMessageHandler>());
        var tcpHandlers = new List<ITcpMessageHandler>
        {
            new TestTcpHandler()
        };
        var tcpStore = new TcpHandlerStore(tcpHandlers, sessionManager);



        //Start server
        var server = new GameServer(port, handlerStore, tcpStore);
        _ = Task.Run(() => server.StartAsync());
        await Task.Delay(300); // give server time to start

        
        //Client, NO NetworkService, only TcpClient
        using var tcpClient = new TcpClient();
        await tcpClient.ConnectAsync("127.0.0.1", port);
        var stream = tcpClient.GetStream();

        //Build message
        var envelope = new MessageEnvelope
        {
            MessageType = "TestMsg",
            Payload = "HelloServer"
        };

        string json = JsonSerializer.Serialize(envelope);
        byte[] data = Encoding.UTF8.GetBytes(json);
        byte[] prefix = BitConverter.GetBytes(data.Length);

        //SEND
        await stream.WriteAsync(prefix, 0, 4);
        await stream.WriteAsync(data, 0, data.Length);

        //RECEIVE
        byte[] responsePrefix = new byte[4];
        int read = await stream.ReadAsync(responsePrefix, 0, 4);
        Assert.Equal(4, read);

        int responseLength = BitConverter.ToInt32(responsePrefix, 0);
        byte[] responseData = new byte[responseLength];

        int totalRead = 0;
        while (totalRead < responseLength)
        {
            int chunk = await stream.ReadAsync(responseData, totalRead, responseLength - totalRead);
            totalRead += chunk;
        }

        string response = Encoding.UTF8.GetString(responseData);

        //Assert
        Assert.Equal("SERVER_RECEIVED:HelloServer", response);

        tcpClient.Close();
    }

    
        [Fact]
        public async Task TwoClients_CanConnect_And_IndependentlyCommunicate()
        {
            int port = 5060;

            //SETUP SERVER 
            var sessionManager = new GameSessionManager();

            var handlerStore = new HandlerStore(new List<IMessageHandler>());
            var tcpHandlers = new List<ITcpMessageHandler> { new TestTcpHandler() };
            var tcpStore = new TcpHandlerStore(tcpHandlers, sessionManager);

            var server = new GameServer(port, handlerStore, tcpStore);
            _ = Task.Run(() => server.StartAsync());

            await Task.Delay(300);

            //Cliént 1
            using var client1 = new TcpClient();
            await client1.ConnectAsync("127.0.0.1", port);
            var stream1 = client1.GetStream();

            //Client 2
            using var client2 = new TcpClient();
            await client2.ConnectAsync("127.0.0.1", port);
            var stream2 = client2.GetStream();

            //Messages
            string msg1 = JsonSerializer.Serialize(new MessageEnvelope
            {
                MessageType = "TestMsg",
                Payload = "FromClient1"
            });

            string msg2 = JsonSerializer.Serialize(new MessageEnvelope
            {
                MessageType = "TestMsg",
                Payload = "FromClient2"
            });

            //Send from client1
            await SendAsync(stream1, msg1);
            string resp1 = await ReceiveAsync(stream1);

            //Send from client2
            await SendAsync(stream2, msg2);
            string resp2 = await ReceiveAsync(stream2);

            //ASSERT
            Assert.Equal("SERVER_RECEIVED:FromClient1", resp1);
            Assert.Equal("SERVER_RECEIVED:FromClient2", resp2);
        }

        
        //Helpers
        private async Task SendAsync(NetworkStream stream, string json)
        {
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] prefix = BitConverter.GetBytes(data.Length);
            await stream.WriteAsync(prefix, 0, 4);
            await stream.WriteAsync(data, 0, data.Length);
        }

        private async Task<string> ReceiveAsync(NetworkStream stream)
        {
            byte[] lenBuf = new byte[4];
            int read = await stream.ReadAsync(lenBuf, 0, 4);
            int length = BitConverter.ToInt32(lenBuf, 0);

            byte[] dataBuf = new byte[length];
            int total = 0;
            while (total < length)
            {
                int chunk = await stream.ReadAsync(dataBuf, total, length - total);
                total += chunk;
            }
            return Encoding.UTF8.GetString(dataBuf);
        }
}
