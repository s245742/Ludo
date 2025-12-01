using Xunit;
using Moq;
using LudoServer.Handlers;
using LudoServer.Services;
using LudoServer.Session;
using SharedModels.Models;
using SharedModels.Models.DTO;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Net.Sockets;
using System.Threading.Tasks;

public class JoinGameHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldJoinGame_WhenValid()
    {
        //Arrange
        var gameService = new Mock<IGameService>();
        var playerService = new Mock<IPlayerService>();
        var pieceService = new Mock<IGamePieceService>();

        var manager = new GameSessionManager();

        var game = new Game { Game_Name = "MyGame" };

        var session = manager.GetOrCreateSession(game);
        session.Players[PieceColor.Red] = new Player(PieceColor.Red);

        //Mock DBplayer
        playerService.Setup(s => s.getAllPlayersFromGame(game))
            .Returns(new ObservableCollection<Player>
            {
            new Player(PieceColor.Red)
            });

        //no pieces
        pieceService.Setup(s => s.getAllGamePieceFromPlayer(It.IsAny<Player>()))
            .Returns(new ObservableCollection<Piece>());

        var handler = new JoinGameHandler(
            gameService.Object,
            playerService.Object,
            pieceService.Object,
            manager
        );

        var dto = new JoinGameDTO
        {
            Game = game,
            PieceColor = PieceColor.Red
        };

        var json = JsonSerializer.Serialize(dto);
        //act
        var result = await handler.HandleAsync(json, new TcpClient());
        //assert
        Assert.Contains("Success", result);
    }

}
