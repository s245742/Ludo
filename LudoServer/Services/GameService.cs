using SharedModels.Models;
using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;


namespace LudoServer.Services
{
    
    public class GameService : ServiceBase
    {

        #region createGame
        public bool CreateGame(Game game)
        {
            string query = "insert into game values (@Game_Name)";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
             {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Game_Name", game.Game_Name);
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

        #region deleteGame
        public bool delete(Game game)
        {
            string query = "delete from Game where Game_Name = @Game_Name";
            int rowsAffected = 0;
            using (SqlConnection connection = new SqlConnection( connectionString))
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
        #endregion

        #region displayGame
        public ObservableCollection<Game> getAll()
        {
            var gamesList = new ObservableCollection<Game>();
            string query = "SELECT Game_Name FROM Game";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read()) {
                        gamesList.Add(new Game
                        {
                            Game_Name = reader.GetString(0)
                        });
                    }
                    reader.Close(); 
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return gamesList;
        }
        #endregion

    }
}
