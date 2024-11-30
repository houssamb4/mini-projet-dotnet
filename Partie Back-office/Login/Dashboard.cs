using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Login
{
    public partial class Dashboard : Form
    {
        char caractere, lettre;
        SqlDataReader result;

        public Dashboard()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;


            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                bd.OpenConnexion();

                try
                {
                    SqlCommand cmdReservations = new SqlCommand("SELECT COUNT(*) FROM reservation", bd.getConnexion);
                    object resultReservations = cmdReservations.ExecuteScalar();
                    int totalReservations = resultReservations != null ? Convert.ToInt32(resultReservations) : 0;
                    label13.Text = totalReservations.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading reservations count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label13.Text = "Error";
                }

                try
                {
                    SqlCommand cmdPaiements = new SqlCommand("SELECT COUNT(*) FROM paiement", bd.getConnexion);
                    object resultPaiements = cmdPaiements.ExecuteScalar();
                    int totalPaiements = resultPaiements != null ? Convert.ToInt32(resultPaiements) : 0;
                    label14.Text = totalPaiements.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading payments count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label14.Text = "Error";
                }

                try
                {
                    SqlCommand cmdPendingReservations = new SqlCommand("SELECT COUNT(*) FROM reservation WHERE status = 'pending'", bd.getConnexion);
                    object resultPendingReservations = cmdPendingReservations.ExecuteScalar();
                    int totalPendingReservations = resultPendingReservations != null ? Convert.ToInt32(resultPendingReservations) : 0;
                    label16.Text = totalPendingReservations.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading pending reservations count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label16.Text = "Error";
                }

                try
                {
                    SqlCommand cmdAvailableRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Available'", bd.getConnexion);
                    object resultAvailableRooms = cmdAvailableRooms.ExecuteScalar();
                    int totalAvailableRooms = resultAvailableRooms != null ? Convert.ToInt32(resultAvailableRooms) : 0;
                    label15.Text = totalAvailableRooms.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading available rooms count: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    label15.Text = "Error";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
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

        private void button4_Click(object sender, EventArgs e)
        {
            home hm= new home();
            hm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Employe emp = new Employe();
            emp.Show();
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
            Account acc = new Account();
            acc.Show();
            this.Hide();

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
