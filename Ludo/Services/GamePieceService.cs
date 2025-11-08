using Ludo.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ludo.Services
{
    public class GamePieceService : ServiceBase
    {
        
        #region CreateGamePiece
        public bool CreateGamePiece(GamePiece gamePiece)
        {
            string query = "insert into Game_Piece values (@Player_ID, @Board_Pos)";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
               
                command.Parameters.AddWithValue("@Player_ID", gamePiece.Player_ID);
                command.Parameters.AddWithValue("@Board_Pos", gamePiece.Board_Pos);
                
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
        
        public ObservableCollection<GamePiece> getAllGamePieceFromPlayer(Player player)
        {
            var gpList = new ObservableCollection<GamePiece>();
            string query = "SELECT Player_ID,Board_Pos FROM Game_Piece join Player on Game_Piece.Player_ID = Player.id where player.Game_Name = @game_Name AND player.color = @color";
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
                        GamePiece gp = new GamePiece();
                        gp.Player_ID = reader.GetInt32(0);
                        gp.Board_Pos = reader.GetInt32(1);
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

        

    }
}
