using SharedModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace LudoServer.Session
{
    public class GameSession
    {
        public Game Game { get; set; }
        public Dictionary<PieceColor, Player> Players { get; set; } = new();
        public Dictionary<PieceColor, TcpClient> PlayerConnections { get; set; } = new();

        public GameSession(Game game)
        {
            Game = game;
        }

    }
}
