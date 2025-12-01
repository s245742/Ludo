using Xunit;
using Moq;
using LudoServer.Handlers;
using LudoServer.Services;
using LudoServer.Session;
using SharedModels.Models;
using System.Text.Json;
using System.Threading.Tasks;

public class DeleteGameHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldDeleteGame_WhenSessionNotActive()
    {
        //Arrange
        var game = new Game { Game_Name = "GameA" };

        var gameService = new Mock<IGameService>();
        var playerService = new Mock<IPlayerService>();
        var pieceService = new Mock<IGamePieceService>();

        var manager = new GameSessionManager(); 

        var handler = new DeleteGameHandler(
            gameService.Object,
            playerService.Object,
            pieceService.Object,
            manager
        );

        string json = JsonSerializer.Serialize(game);
        //Act
        var result = await handler.HandleAsync(json);
        //Assert
        Assert.Contains("GameDeleted", result);
    }
}
