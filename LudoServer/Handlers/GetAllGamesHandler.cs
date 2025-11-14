using LudoServer.Services;
using SharedModels.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class GetAllGamesHandler : IMessageHandler
    {
        private readonly GameService _gameService;
        public string MessageType => "GetAllGames";

        public GetAllGamesHandler(GameService gameService)
        {
            _gameService = gameService;
        }

        public async Task<string> HandleAsync(string payload)
        {
            var games = _gameService.getAll();

            string json = System.Text.Json.JsonSerializer.Serialize(games);

            return json; //return the games as json

        }
    }
}
