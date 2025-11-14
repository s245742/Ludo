using LudoServer.Services;
using SharedModels.Models;
using SharedModels.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class JoinGameHandler : IMessageHandler
    {
        private readonly GameService _gameService;
        private readonly PlayerService _playerService;
        private readonly GamePieceService _gamePieceService;
        public string MessageType => "JoinGame";

        public JoinGameHandler(GameService gameService, PlayerService playerService, GamePieceService gamePieceService)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;
        }

        public async Task<string> HandleAsync(string payload)
        {
            var game = System.Text.Json.JsonSerializer.Deserialize<Game>(payload);

            if (game == null) return "game is null";


            //Get player list
            ObservableCollection<Player> GamePlayers = _playerService.getAllPlayersFromGame(game);
            //
            foreach (Player player in GamePlayers)
            {
                ObservableCollection<Piece> gp = new ObservableCollection<Piece>();
                gp = _gamePieceService.getAllGamePieceFromPlayer(player);
                foreach (Piece gamepiece in gp)
                {
                    player.PlayerPieces.Add(gamepiece);
                }
            }
            string json = System.Text.Json.JsonSerializer.Serialize(GamePlayers);

            return json; //return the players
        }

    }
}
