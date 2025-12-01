using LudoServer.Session;
using SharedModels.Models;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using Xunit;

public class GameSessionManagerTests
{
    private Game CreateGame(string name)
    {
        return new Game { Game_Name = name };
    }

    private Player CreatePlayer(int id, PieceColor color)
    {
        return new Player(color) { Player_ID = id };
    }


    //GetOrCreateSession should create a new session
    [Fact]
    public void GetOrCreateSession_CreatesSession_WhenNotExisting()
    {
        //Arrange
        var manager = new GameSessionManager();
        var game = CreateGame("TestGame");

        //act
        var session = manager.GetOrCreateSession(game);
        //assert
        Assert.NotNull(session);
        Assert.Equal("TestGame", session.Game.Game_Name);
    }

    //GetOrCreateSession returns existing session
    [Fact]
    public void GetOrCreateSession_ReturnsSameSession_WhenAlreadyExists()
    {
        //Arrange 
        var manager = new GameSessionManager();
        var game = CreateGame("TestGame");

        //act
        var session1 = manager.GetOrCreateSession(game);
        var session2 = manager.GetOrCreateSession(game);
        //assert
        Assert.Same(session1, session2);
    }

    //GetSession finds session by game
    [Fact]
    public void GetSession_ReturnsCorrectSession()
    {
        //arrange
        var manager = new GameSessionManager();
        var game = CreateGame("GameA");
        var session = manager.GetOrCreateSession(game);
        //act
        var found = manager.GetSession(game);
        //assert
        Assert.Same(session, found);
    }

    //GetSessionByPlayer finds correct session
    [Fact]
    public void GetSessionByPlayer_ReturnsSessionWithPlayer()
    {
        //arrange
        var manager = new GameSessionManager();
        var game = CreateGame("GameX");

        var session = manager.GetOrCreateSession(game);

        var player = CreatePlayer(42, PieceColor.Blue);

        session.Players[player.Color] = player;

        //act
        var found = manager.GetSessionByPlayer(42);
        //assert
        Assert.Same(session, found);
    }

    //GetSessionByClient finds correct session
    [Fact]
    public void GetSessionByClient_ReturnsCorrectSession()
    {
        //arrange
        var manager = new GameSessionManager();
        var game = CreateGame("GameZ");

        var session = manager.GetOrCreateSession(game);

        var client = new TcpClient(); // real instance is fine
        session.PlayerConnections[PieceColor.Red] = client;
        //act
        var found = manager.GetSessionByClient(client);
        //assert
        Assert.Same(session, found);
    }

    // 6. RemoveByClient removes client + player
    [Fact]
    public void RemoveByClient_RemovesPlayerAndConnection()
    {
        //arrange
        var manager = new GameSessionManager();
        var game = CreateGame("GameRemove");

        var session = manager.GetOrCreateSession(game);
        var client = new TcpClient();

        var player = CreatePlayer(100, PieceColor.Yellow);

        session.Players[player.Color] = player;
        session.PlayerConnections[player.Color] = client;
        //act
        manager.RemoveByClient(client);
        //assert
        Assert.Empty(session.PlayerConnections);
        Assert.Empty(session.Players);
    }
}
