using System;
using System.Data.SqlClient;

public class DatabaseHelper
{
    private readonly string connectionString;

    public DatabaseHelper()
    {
        connectionString = "Server=HP\\SQLEXPRESS;Database=hotel;Trusted_Connection=True;";
    }


    public bool TestConnection()
    {
        try
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                return connection.State == System.Data.ConnectionState.Open;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Database connection test failed: {ex.Message}");
            return false;
        }
    }

    public SqlConnection GetConnection()
    {
        return new SqlConnection(connectionString);
    }
}
