using LudoServer.Services;
using SharedModels.Models;
using SharedModels.Models.DTO;
using SharedModels.TransferMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class DeleteGameHandler : IMessageHandler
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;
        public string MessageType => "DeleteGame";

        public DeleteGameHandler(GameService gameService, PlayerService playerService, GamePieceService gamePieceService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;
        }

        public async Task<string> HandleAsync(string payload)
        {
            var game = System.Text.Json.JsonSerializer.Deserialize<Game>(payload);

            if (game == null) return "game is null";


            //call gameservices (Should be transaction, but w/e)
            _gamePieceService.deleteGamePiecesfromGame(game);
            _playerService.deletePlayersfromGame(game);
            _gameService.delete(game);
            return "GameDeleted";
        }


       

    }
}
