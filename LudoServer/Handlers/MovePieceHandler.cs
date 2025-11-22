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
        private readonly GamePieceService _gamePieceService;
        private readonly GameSessionManager _sessionManager;
        public string MessageType => "MovePiece";

        public MovePieceHandler(GamePieceService gamePieceService, GameSessionManager sessionManager)
        {
            _gamePieceService = gamePieceService;
            _sessionManager = sessionManager;
        }

        public async Task<string> HandleAsync(string payload, TcpClient senderClient)
        {
            Console.WriteLine("Received MovePiece payload: " + payload);
            var moveDto = JsonSerializer.Deserialize<MovePieceDto>(payload);
            if (moveDto == null) return "\"Invalid payload\"";

            Piece piece = new Piece(moveDto.Color,moveDto.SlotIndex,moveDto.SpaceIndex)
            {
                Player_ID = moveDto.Player_ID,
            };



            //Update DB
            int pieceid = _gamePieceService.GetPieceIDFromPiece(piece);
            bool updated = _gamePieceService.UpdatePieceFromPieceID(piece, pieceid);

            if (!updated) return "\"DB update failed\"";

            //Broadcast to clients
            foreach (var session in _sessionManager.GetAllSessions())
            {
                foreach (var kvp in session.PlayerConnections)
                {
                    var client = kvp.Value;

                    // skip sender
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