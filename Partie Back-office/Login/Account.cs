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



        private void home_Load(object sender, EventArgs e)
        {
        }



        private int GetCurrentUserId()
        {
            // Access the property directly
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
                // Handle any errors gracefully
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void tableauClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle the cell content click if needed
        }

        private void label4_Click(object sender, EventArgs e)
        {
            // Handle label click if needed
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

        private void txtEmail_TextChanged(object sender, EventArgs e)
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

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

