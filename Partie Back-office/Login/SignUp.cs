using System;
using System.Data.SqlClient;
using System.Data;

namespace Login
{
    class SignUp
    {
        Database bd = new Database();

        public bool RegisterUser(string fullName, string email, string password)
        {
            try
            {
                bd.OpenConnexion();

                SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM Utilisateur WHERE email = @Email", bd.getConnexion);
                checkCmd.Parameters.AddWithValue("@Email", email);
                int emailCount = (int)checkCmd.ExecuteScalar();

                if (emailCount > 0)
                {
                    bd.CloseConnexion();
                    return false;
                }

                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO Utilisateur (nom_complet, email, password) VALUES (@FullName, @Email, @Password)",
                    bd.getConnexion
                );

                cmd.Parameters.AddWithValue("@FullName", fullName);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password); 

                int rowsAffected = cmd.ExecuteNonQuery();
                bd.CloseConnexion();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                bd.CloseConnexion();
                throw new Exception("An error occurred during registration: " + ex.Message);
            }
        }
    }
}
