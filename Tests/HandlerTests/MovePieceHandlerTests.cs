using Xunit;
using Moq;
using LudoServer.Handlers;
using LudoServer.Services;
using LudoServer.Session;
using SharedModels.Models;
using SharedModels.Models.DTO;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Tasks;

public class MovePieceHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldMovePiece_WhenValid()
    {
        //arrange
        var pieceService = new Mock<IGamePieceService>();
        var manager = new GameSessionManager();

        var client = new TcpClient();
        var game = new Game { Game_Name = "MoveTest" };

        //Create session and put client in
        var session = manager.GetOrCreateSession(game);
        session.PlayerConnections[PieceColor.Red] = client;

        //Mock DB
        pieceService.Setup(s => s.GetPieceIDFromPiece(It.IsAny<Piece>()))
                    .Returns(1);

        pieceService.Setup(s => s.UpdatePieceFromPieceID(It.IsAny<Piece>(), 1))
                    .Returns(true);

        var handler = new MovePieceHandler(pieceService.Object, manager);

        var dto = new MovePieceDto
        {
            Player_ID = 1,
            SlotIndex = 0,
            SpaceIndex = 5,
            Color = PieceColor.Red
        };

        string json = JsonSerializer.Serialize(dto);

        //act
        var result = await handler.HandleAsync(json, client);
        //assert
        Assert.Contains("MovePiece delivered", result);
    }
}
