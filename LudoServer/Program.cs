using LudoServer.Handlers;
using LudoServer.Server;
using LudoServer.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<GameService>();
        services.AddSingleton<PlayerService>();
        services.AddSingleton<GamePieceService>();

        //Handler as singleton
        services.AddSingleton<IMessageHandler, CreateGameHandler>();

        //set TCPserver singleton and inject the others
        services.AddSingleton<HandlerStore>(serviceProvider =>
            new HandlerStore(serviceProvider.GetServices<IMessageHandler>()));

        var serviceProvider = services.BuildServiceProvider();

        var store = serviceProvider.GetRequiredService<HandlerStore>();

        var server = new GameServer(5000, store);
        await server.StartAsync();
        
    }



}