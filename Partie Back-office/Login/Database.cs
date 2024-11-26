using System;
using System.Data;
using System.Data.SqlClient;

namespace Login
{
    class Database
    {
        private SqlConnection con = new SqlConnection(@"Server=Hp\SQLEXPRESS;Database=hotel;Trusted_Connection=True;");

        public SqlConnection getConnexion
        {
            get { return con; }
        }

        public void OpenConnexion()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                    Console.WriteLine("Connection to the database is successful.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening connection: {ex.Message}");
            }
        }

        public void CloseConnexion()
        {
            try
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                    Console.WriteLine("Connection closed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing connection: {ex.Message}");
            }
        }
    }
}
