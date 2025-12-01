using Xunit;
using Moq;
using LudoServer.Handlers;
using LudoServer.Services;
using SharedModels.Models;
using SharedModels.Models.DTO;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

public class CreateGameHandlerTests
{
    private Mock<IGameService> _gameService;
    private Mock<IPlayerService> _playerService;
    private Mock<IGamePieceService> _pieceService;

    private CreateGameHandler CreateHandler()
    {
        _gameService = new Mock<IGameService>();
        _playerService = new Mock<IPlayerService>();
        _pieceService = new Mock<IGamePieceService>();

        return new CreateGameHandler(
            _gameService.Object,
            _playerService.Object,
            _pieceService.Object
        );
    }

   
    [Fact]
    public async Task HandleAsync_ShouldCreateGame_WhenNameIsUnique()
    {
        //Arrange
        var handler = CreateHandler();

        _gameService.Setup(s => s.getAll())
                    .Returns(new ObservableCollection<Game>()); // no existing games

        var dto = new CreateGameDTO
        {
            Game = new Game { Game_Name = "UniqueGame" },
            Players = new List<Player>
            {
                new Player(PieceColor.Red)
                {
                    PlayerPieces = new ObservableCollection<Piece>
                    {
                        new Piece(PieceColor.Red, 0, 0),
                        new Piece(PieceColor.Red, 1, 0)
                    }
                }
            }
        };

        string json = JsonSerializer.Serialize(dto);
        //Act
        var result = await handler.HandleAsync(json);
        //Assert
        Assert.Equal("GameCreated", result);
    }

    

    
}
