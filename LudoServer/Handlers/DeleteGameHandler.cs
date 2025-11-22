using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Services;
using LudoServer.Session;
using SharedModels.Models;
using SharedModels.Models.DTO;
using SharedModels.TransferMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class DeleteGameHandler : IMessageHandler
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;
        private readonly GameSessionManager _gameSessionManager;
        public string MessageType => "DeleteGame";

        public DeleteGameHandler(GameService gameService, PlayerService playerService, GamePieceService gamePieceService, GameSessionManager gameSessionManager)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;
            _gameSessionManager = gameSessionManager;
        }

        public async Task<string> HandleAsync(string payload)
        {
            var game = System.Text.Json.JsonSerializer.Deserialize<Game>(payload);

            if (game == null) return "game is null";
            
            var session = _gameSessionManager.GetSession(game);
            if (session != null)
            {
                return JsonSerializer.Serialize("Game is active, cannot be deleted");
                
            }
            //call gameservices (Should be transaction, but w/e)
            _gamePieceService.deleteGamePiecesfromGame(game);
            _playerService.deletePlayersfromGame(game);
            _gameService.delete(game);
            return JsonSerializer.Serialize("GameDeleted");
        }


       

    }
}
