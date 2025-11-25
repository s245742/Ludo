using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Services;
using LudoServer.Session;
using SharedModels.Models;
using SharedModels.Models.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class JoinGameHandler : ITcpMessageHandler
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        private readonly IGamePieceService _gamePieceService;
        private readonly GameSessionManager _gameSessionManager;
        public string MessageType => "JoinGame";

        public JoinGameHandler(IGameService gameService, IPlayerService playerService, IGamePieceService gamePieceService, GameSessionManager gameSessionManager)
        {
            _gameService = gameService;
            _playerService = playerService;
            _gamePieceService = gamePieceService;
            _gameSessionManager = gameSessionManager;
        }

        public async Task<string> HandleAsync(string payload, TcpClient client)
        {
            var dto = JsonSerializer.Deserialize<JoinGameDTO>(payload);
            var session = _gameSessionManager.GetOrCreateSession(dto.Game);

            if (dto == null) return "game is null";

            //error this color shouldnt already be in this session!
            if (session.Players.ContainsKey(dto.PieceColor))
            {
                Console.WriteLine("this player is already connected");
                var resp1 = new JoinGameResponse
                {
                    Success = false,
                    Message = "This player/color is already connected!",
                    Players = session.Players.Values.ToList()
                };
                return JsonSerializer.Serialize(resp1);
            }

            Player player = _playerService.getAllPlayersFromGame(dto.Game).FirstOrDefault(p => p.Color == dto.PieceColor);
            session.Players[dto.PieceColor] = player;
            session.PlayerConnections[dto.PieceColor] = client;
            Console.WriteLine($"Currently {session.PlayerConnections.Count} clients connected");
            
            //Get player list
            ObservableCollection<Player> GamePlayers = _playerService.getAllPlayersFromGame(dto.Game);
            //
            foreach (Player players in GamePlayers)
            {
                ObservableCollection<Piece> gp = new ObservableCollection<Piece>();
                gp = _gamePieceService.getAllGamePieceFromPlayer(players);
                foreach (Piece gamepiece in gp)
                {
                    players.PlayerPieces.Add(gamepiece);
                }
            }

            var resp = new JoinGameResponse
            {
                Success = true,
                Message = "Success",
                Players = GamePlayers.ToList()
            };

            return JsonSerializer.Serialize(resp);
        }

    }
}
