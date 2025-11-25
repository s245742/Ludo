using LudoServer.Handlers;
using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Server;
using LudoServer.Services;
using LudoServer.Session;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IGameService, GameService>();
        services.AddSingleton<IPlayerService, PlayerService>();
        services.AddSingleton<IGamePieceService, GamePieceService>();

        //Handler as singleton
        services.AddSingleton<IMessageHandler, CreateGameHandler>();
        services.AddSingleton<IMessageHandler, GetAllGamesHandler>();
        services.AddSingleton<IMessageHandler, DeleteGameHandler>();
        
        services.AddSingleton<GameSessionManager>();
        services.AddSingleton<ITcpMessageHandler, JoinGameHandler>();
        services.AddSingleton<ITcpMessageHandler, MovePieceHandler>();

        //set TCPserver singleton and inject the others
        services.AddSingleton<HandlerStore>(serviceProvider =>
            new HandlerStore(serviceProvider.GetServices<IMessageHandler>()));
       
        services.AddSingleton<TcpHandlerStore>(sp =>
            new TcpHandlerStore(sp.GetServices<ITcpMessageHandler>(), sp.GetRequiredService<GameSessionManager>()));

        services.AddSingleton<GameSessionManager>();


        var serviceProvider = services.BuildServiceProvider();

        var store = serviceProvider.GetRequiredService<HandlerStore>();
        var tcpstore = serviceProvider.GetRequiredService<TcpHandlerStore>();
        var server = new GameServer(5000, store,tcpstore);
        await server.StartAsync();

        
    }



}