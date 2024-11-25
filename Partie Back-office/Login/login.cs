using System.Data.SqlClient;
using System.Data;

namespace Login
{
    class Login
    {
        Database bd = new Database();

        public static string LoggedInUserFullName { get; private set; }

        public bool VerificationDesChamps(string email, string password)
        {
            bd.OpenConnexion();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Utilisateur WHERE email = @Email AND password = @Password", bd.getConnexion);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.CommandType = CommandType.Text;

            SqlDataReader resultat = cmd.ExecuteReader();

            if (resultat.HasRows)
            {
                while (resultat.Read())
                {
                    LoggedInUserFullName = resultat["nom_complet"].ToString();
                }

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
