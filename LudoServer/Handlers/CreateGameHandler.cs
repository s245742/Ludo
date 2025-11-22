using LudoServer.Handlers.MsgHandlerInterfaces;
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

        public async Task<string> HandleAsync(string payload)
        {
            var DTO = System.Text.Json.JsonSerializer.Deserialize<CreateGameDTO>(payload);
            
            if (DTO == null) return "DTO is null";
            
            if (checkIfGameNameExists(DTO.Game.Game_Name))
            {
                return "error, GameName already exists\n";
            }

            _gameService.CreateGame(DTO.Game);
            //add each player and piece to DB
            foreach (var player in DTO.Players)
            {
                player.Game_Name = DTO.Game.Game_Name; //make sure they match
                _playerService.CreatePlayer(player);

                foreach (var piece in player.PlayerPieces)
                {
                    _gamePieceService.CreateGamePiece(piece);
                }

            }
            return "GameCreated";
        }
    

     //helper method
        private bool checkIfGameNameExists(string name)
        {
            
            var games = _gameService.getAll();
            foreach (var game in games)
            {
                if (game.Game_Name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

    }


}
