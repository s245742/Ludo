using Xunit;
using Moq;
using LudoServer.Handlers;
using LudoServer.Services;
using SharedModels.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

public class GetAllGamesHandlerTests
{
    [Fact]
    public async Task HandleAsync_ShouldReturnAllGames()
    {
        //Arrange
        var gameService = new Mock<IGameService>();
        var playerService = new Mock<IPlayerService>();

        var game = new Game { Game_Name = "Game1" };

        gameService.Setup(s => s.getAll()).Returns(new ObservableCollection<Game> { game });


        playerService.Setup(s => s.getAllPlayersFromGame(game))
            .Returns(new ObservableCollection<Player>
            {
                new Player(PieceColor.Red)
            });


        var handler = new GetAllGamesHandler(gameService.Object,playerService.Object);

        //Act
        var result = await handler.HandleAsync("");

        //Assert
        Assert.Contains("Game1", result);
    }
}
