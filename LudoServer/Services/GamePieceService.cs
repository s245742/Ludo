using SharedModels.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Numerics;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace LudoServer.Services
{
    public class GamePieceService : ServiceBase
    {
        
        #region CreateGamePiece
        public bool CreateGamePiece(Piece gamePiece)
        {
            string query = "insert into Game_Piece values (@Player_ID, @SpaceIndex,@SlotIndex)";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
               
                command.Parameters.AddWithValue("@Player_ID", gamePiece.Player_ID);
                command.Parameters.AddWithValue("@SpaceIndex", gamePiece.SpaceIndex);
                command.Parameters.AddWithValue("@SlotIndex", gamePiece.SlotIndex);

                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; //true if updated rows
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }


        }
        #endregion
        public bool deleteGamePiecesfromGame(Game game)
        {
            string query = "delete Game_Piece from Game_Piece join Player on Player_ID = Player.ID where Game_Name = @Game_Name";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Game_Name", game.Game_Name);
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; //return true if any row update
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }

            }
        }
        
        public ObservableCollection<Piece> getAllGamePieceFromPlayer(Player player)
        {
            var gpList = new ObservableCollection<Piece>();
            string query = "SELECT Player_ID,SlotIndex,SpaceIndex FROM Game_Piece join Player on Game_Piece.Player_ID = Player.id where player.Game_Name = @game_Name AND player.color = @color";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@game_Name", player.Game_Name);
                command.Parameters.AddWithValue("@color", player.Color);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int slotI = reader.GetInt32(1);
                        int spaceI = reader.GetInt32(2);
                        Piece gp = new Piece(player.Color,slotI,spaceI);
                        gp.Player_ID = reader.GetInt32(0);
                        gpList.Add(gp);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return gpList;
        }

        public int GetPieceIDFromPiece(Piece piece)
        {
            int slotI = 0;
            string query = "SELECT GamePiece_ID FROM Game_Piece where player_ID = @player_id AND SlotIndex = @slotIndex";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@player_ID", piece.Player_ID);
                command.Parameters.AddWithValue("@slotindex", piece.SlotIndex);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        slotI = reader.GetInt32(0);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return slotI;
        }
        public bool UpdatePieceFromPieceID(Piece pieceToUpdate, int pieceId)
        {
            int rowsAffected = 0;
            string query = "update Game_Piece set SpaceIndex = @spaceIndex where GamePiece_ID = @gamepiece_ID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@spaceIndex", pieceToUpdate.SpaceIndex);
                command.Parameters.AddWithValue("@gamepiece_ID", pieceId);
                try
                {
                    connection.Open();
                    rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; //return true if any row update
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
        }




    }
}
