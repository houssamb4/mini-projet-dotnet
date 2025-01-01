using System;
using System.Data.SqlClient;

namespace Login
{
    class Client
    {
        Database bd = new Database();

        public bool AjouterClient(string nom, string email, string phone, string adresse)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO Client (nom_complet, email, telephone, Adresse) VALUES (@nom, @email, @phone, @adresse)", bd.getConnexion);
                cmd.Parameters.AddWithValue("@nom", nom);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@phone", phone);
                cmd.Parameters.AddWithValue("@adresse", adresse);

                bd.OpenConnexion();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    SqlCommand logCmd = new SqlCommand("INSERT INTO ActivityLog (UserID, ActivityType) VALUES (@userID, @activityType)", bd.getConnexion);
                    logCmd.Parameters.AddWithValue("@userID", Login.LoggedInUserID); 
                    logCmd.Parameters.AddWithValue("@activityType", "New Client Created");

                    logCmd.ExecuteNonQuery();

                    bd.CloseConnexion();
                    return true;
                }
                else
                {
                    bd.CloseConnexion();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                bd.CloseConnexion();
                return false;
            }
        }


        public bool DeleteClient(string id)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Client WHERE id = @id", bd.getConnexion);
            cmd.Parameters.AddWithValue("@id", id);

            bd.OpenConnexion();

            if (cmd.ExecuteNonQuery() == 1)
            {
                bd.CloseConnexion();
                return true;
            }
            else
            {
                bd.CloseConnexion();
                return false;
            }
        }

        public bool UpdateClient(string id, string nom, string email, string phone, string adresse)
        {
            SqlCommand cmd = new SqlCommand("UPDATE client SET nom_complet = @nom, email = @email, telephone = @phone, Adresse = @adresse WHERE id = @id", bd.getConnexion);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nom", nom);
            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@phone", phone);
            cmd.Parameters.AddWithValue("@adresse", adresse);

            bd.OpenConnexion();

            if (cmd.ExecuteNonQuery() == 1)
            {
                bd.CloseConnexion();
                return true;
            }
            else
            {
                bd.CloseConnexion();
                return false;
            }
        }
    }
}
