using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class Account : Form
    {
        char caractere, lettre;
        SqlDataReader result;

        public Account()
        {
            InitializeComponent();

            label7.Text = Login.LoggedInUserFullName;

        }

        Database bd = new Database();



        private void Account_load(object sender, EventArgs e)
        {
            LoadUserDetails();
        }



        private int GetCurrentUserId()
        {
            return Login.LoggedInUserID;
        }


        private void UpdateUserStatus()
        {
            string updateQuery = @"
        UPDATE Utilisateur
        SET status = 'offline', last_login = GETDATE()
        WHERE id = @UserId";

            try
            {
                bd.OpenConnexion();
                using (SqlConnection connection = bd.getConnexion)
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        int userId = GetCurrentUserId();

                        command.Parameters.AddWithValue("@UserId", userId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateUserStatus();

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void tableauClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void LoadUserDetails()
        {
            try
            {
                int userId = GetCurrentUserId(); 
                bd.OpenConnexion();
                string query = "SELECT * FROM Utilisateur WHERE id = @userId";

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    command.Parameters.AddWithValue("@userId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNom.Text = reader["nom_complet"].ToString();
                            txtEmail.Text = reader["email"].ToString();
                            createdat.Text = reader["created_at"].ToString();
                            status.Text = reader["status"].ToString();
                            if (reader["last_login"] == DBNull.Value || string.IsNullOrEmpty(reader["last_login"].ToString()))
                            {
                                last_login.Text = "Now"; 
                            }
                            else
                            {
                                last_login.Text = Convert.ToDateTime(reader["last_login"]).ToString("g"); 
                            }

                        }
                        else
                        {
                            MessageBox.Show("User details not found!");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void txtNom_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
        }

        private void createdat_TextChanged(object sender, EventArgs e)
        {
        }

       private void last_login_TextChanged(object sender, EventArgs e)
        {
        }

        private void status_TextChanged(object sender, EventArgs e)
        {
        }


        private void label4_Click(object sender, EventArgs e)
        {
        }


        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableauClient_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Employe Emp = new Employe();
            Emp.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rooms rom = new Rooms();
            rom.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void ErrorPhone_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ErrorNom_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Do you really want to delete this account?",
                "Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    int userId = GetCurrentUserId(); 
                    bd.OpenConnexion();
                    string query = "DELETE FROM Utilisateur WHERE id = @userId";

                    using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                    {
                        command.Parameters.AddWithValue("@userId", userId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Account deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            this.Hide(); 
                            Form1 form1 = new Form1();
                            form1.Show();
                        }
                        else
                        {
                            MessageBox.Show("Account deletion failed. User not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    bd.CloseConnexion();
                }
            }
        }


        private void button9_Click(object sender, EventArgs e)
        {
            Reset res = new Reset(Login.LoggedInUserID);
            res.Show();
            this.Hide();
        }


        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

