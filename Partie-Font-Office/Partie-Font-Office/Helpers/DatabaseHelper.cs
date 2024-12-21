using System;
using System.Data.SqlClient;

namespace Partie_Font_Office.Helpers
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        // Constructor initializes the connection string
        public DatabaseHelper()
        {
            connectionString = "Server=DESKTOP-9G04GPU\\SQLEXPRESS;Database=hotel;Trusted_Connection=True;";
        }

        // Returns a new SqlConnection without opening it
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Tests if the database connection can be successfully opened
        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return connection.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Connection test failed: " + ex.Message);
                return false;
            }
        }
    }
}
