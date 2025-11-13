using LudoServer.Services;
using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class CreateGameHandler : IMessageHandler
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;
        public string MessageType => "CreateNewGame";

        public CreateGameHandler(GameService gameService, PlayerService playerService, GamePieceService gamePieceService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;
        }

        public async Task HandleAsync(string payload)
        {
            var game = System.Text.Json.JsonSerializer.Deserialize<Game>(payload);
            
            if (game == null) return;
            
            _gameService.CreateGame(game);
            
            await Task.CompletedTask;
        }
    }
}
