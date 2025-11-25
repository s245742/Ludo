using SharedModels.Models;
using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;


namespace LudoServer.Services
{
    public class PlayerService : ServiceBase, IPlayerService
    {
        
        public bool CreatePlayer(Player player)
        {
            string query = "insert into player values (@Game_Name, @Player_Name,@Color); SELECT SCOPE_IDENTITY();";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Game_Name", player.Game_Name);
                command.Parameters.AddWithValue("@Player_Name", player.Player_Name);
                command.Parameters.AddWithValue("@Color", player.Color);

                try
                {
                    connection.Open();
                    object newId = command.ExecuteScalar(); //we set the player.id from the id the db generates so we later can link 4 gamepieces to this id.
                    if (newId != null && newId != DBNull.Value)
            {
                        // Convert the returned to the Player model's ID
                        player.Player_ID = Convert.ToInt32(newId);
                        foreach (Piece p in player.PlayerPieces)
                        {
                            p.Player_ID = player.Player_ID; //Must reflect the DB's Id
                        }

                        return true;
                    }
                    return false; //weird id value returned
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }


        }
        
        public bool deletePlayersfromGame(Game game)
        {
            string query = "DELETE Player FROM Player JOIN Game ON Player.Game_Name = Game.Game_Name WHERE Game.Game_Name = @Game_Name;";
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


        public ObservableCollection<Player> getAllPlayersFromGame(Game Game)
        {
            var playerList = new ObservableCollection<Player>();
            string query = "SELECT Color,Game_Name,Player_Name FROM player where Game_Name = @game_Name";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@game_Name", Game.Game_Name);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        int pColor = reader.GetInt32(0);
                        Player p = new Player((PieceColor) pColor);
                        p.Game_Name = reader.GetString(1);
                        p.Player_Name = reader.GetString(2);
                        
                        playerList.Add(p);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return playerList;
        }

        




    }
}
