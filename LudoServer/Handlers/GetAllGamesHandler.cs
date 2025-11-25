using LudoServer.Handlers.MsgHandlerInterfaces;
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
    public class GetAllGamesHandler : IMessageHandler
    {
        private readonly IGameService _gameService;
        private readonly IPlayerService _playerService;
        public string MessageType => "GetAllGamesAndPlayers";

        public GetAllGamesHandler(IGameService gameService, IPlayerService playerService)
        {
            _gameService = gameService;
            _playerService = playerService;
        }

        public async Task<string> HandleAsync(string payload)
        {
            ObservableCollection<CreateGameDTO> DTO = new ObservableCollection<CreateGameDTO>();
            var games = _gameService.getAll();

            foreach (var game in games)
            {
                ObservableCollection<Player> players = _playerService.getAllPlayersFromGame(game);

                DTO.Add(new CreateGameDTO
                {
                    Game = game,
                    Players = players.ToList()
                });

            }

            string json = System.Text.Json.JsonSerializer.Serialize(DTO);

            return json; //return the games as json

        }
    }
}
