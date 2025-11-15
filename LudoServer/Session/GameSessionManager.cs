using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace LudoServer.Session
{
    using SharedModels.Models;
    using System.Collections.Concurrent;

    public class GameSessionManager
    {
        private ConcurrentDictionary<string, GameSession> _sessions = new();

       
        public GameSession GetOrCreateSession(Game game)
        {
            if (!_sessions.ContainsKey(game.Game_Name))
            {

                _sessions[game.Game_Name] = new GameSession(game);
            }
            return _sessions[game.Game_Name];
        }

        public IEnumerable<GameSession> GetAllSessions() => _sessions.Values;

       

        //used to check if no sessions in game before deleting
        public GameSession? GetSession(Game game)
        {
            _sessions.TryGetValue(game.Game_Name, out var session);
            return session;
        }

        public async Task BroadcastAsync(GameSession session, string message)
        {
            foreach (var conn in session.PlayerConnections.Values)
            {
                if (conn.Connected)
                {
                    var stream = conn.GetStream();
                    var bytes = Encoding.UTF8.GetBytes(message + "\n");
                    await stream.WriteAsync(bytes);
                }
            }
        }

    }   
}
