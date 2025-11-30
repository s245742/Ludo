using LudoServer.Handlers.MsgHandlerInterfaces;
using LudoServer.Services;
using LudoServer.Session;
using Microsoft.Data.SqlClient;
using SharedModels.Models;
using SharedModels.Models.DTO;
using SharedModels.TransferMsg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LudoServer.Handlers
{
    public class MovePieceHandler : ITcpMessageHandler
    {
        private readonly IGamePieceService _gamePieceService;
        private readonly GameSessionManager _sessionManager;
        public string MessageType => "MovePiece";

        public MovePieceHandler(IGamePieceService gamePieceService, GameSessionManager sessionManager)
        {
            _gamePieceService = gamePieceService;
            _sessionManager = sessionManager;
        }

        public async Task<string> HandleAsync(string payload, TcpClient senderClient)
        {
            Console.WriteLine("Received MovePiece payload: " + payload);
            var moveDto = JsonSerializer.Deserialize<MovePieceDto>(payload);
            if (moveDto == null) return "\"Invalid payload\"";

            // Update DB
            Piece piece = new Piece(moveDto.Color, moveDto.SlotIndex, moveDto.SpaceIndex)
            {
                Player_ID = moveDto.Player_ID
            };

            int pieceId = _gamePieceService.GetPieceIDFromPiece(piece);
            bool updated = _gamePieceService.UpdatePieceFromPieceID(piece, pieceId);
            if (!updated) return "\"DB update failed\"";

            //Get the session belonging to this player
            var session = _sessionManager.GetSessionByClient(senderClient);
            if (session == null)
            {
                Console.WriteLine("MovePiece: No session found for client.");
                return "\"No session found\"";
            }

            
            //Broadcast ONLY to this session
            foreach (var kvp in session.PlayerConnections)
            {
                var client = kvp.Value;

                if (client == senderClient)
                    continue;

                if (client.Connected)
                {
                    await SendToClient(client, new MessageEnvelope
                    {
                        MessageType = "MovePiece",
                        Payload = JsonSerializer.Serialize(moveDto)
                    });
                }
            }

            return "\"MovePiece delivered\"";
        }


        private async Task SendToClient(TcpClient client, MessageEnvelope envelope)
        {
            string json = JsonSerializer.Serialize(envelope);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(bytes.Length);

            var stream = client.GetStream();
            await stream.WriteAsync(length, 0, length.Length);
            await stream.WriteAsync(bytes, 0, bytes.Length);
        }
    }
}