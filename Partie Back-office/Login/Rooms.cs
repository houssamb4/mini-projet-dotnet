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
    public partial class Rooms : Form
    {
        char caractere, lettre;
        SqlDataReader result;

        public Rooms()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;


            LoadStatistics();
            LoadData();
        }

        private int GetCurrentUserId()
        {
            return Login.LoggedInUserID;
        }

        private void LoadStatistics()
        {
            try
            {
                bd.OpenConnexion();

                SqlCommand cmdAvailableRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Available'", bd.getConnexion);
                object resultAvailableRooms = cmdAvailableRooms.ExecuteScalar();
                int totalAvailableRooms = resultAvailableRooms != null ? Convert.ToInt32(resultAvailableRooms) : 0;
                label9.Text = totalAvailableRooms.ToString();

                SqlCommand cmdOccupiedRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Occupied'", bd.getConnexion);
                object resultOccupiedRooms = cmdOccupiedRooms.ExecuteScalar();
                int totalOccupiedRooms = resultOccupiedRooms != null ? Convert.ToInt32(resultOccupiedRooms) : 0;
                label11.Text = totalOccupiedRooms.ToString();

                SqlCommand cmdRooms = new SqlCommand("SELECT COUNT(*) FROM chambre", bd.getConnexion);
                object resultRooms = cmdRooms.ExecuteScalar();
                int totalRooms = resultRooms != null ? Convert.ToInt32(resultRooms) : 0;
                label13.Text = totalRooms.ToString();

                SqlCommand cmdUnderRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Under Maintenance'", bd.getConnexion);
                object resultUnderRooms = cmdUnderRooms.ExecuteScalar();
                int totalUnderRooms = resultUnderRooms != null ? Convert.ToInt32(resultUnderRooms) : 0;
                label14.Text = totalUnderRooms.ToString();


                bd.CloseConnexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                bd.OpenConnexion();

            string query = @"
            SELECT
                c.num_chambre, 
                tc.description AS type_description, 
                c.status
            FROM chambre c
            INNER JOIN type_chambre tc ON c.type_chambre_id = tc.id";

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.AutoGenerateColumns = false;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["num_chambre"],
                            row["type_description"], 
                            row["status"]
                        );
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


        private void panel1_Paint(object sender, PaintEventArgs e)
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
            home hm = new home();
            hm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Employe emp = new Employe();
            emp.Show();
            this.Hide();

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms();
            rooms.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Show();
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

        private void label17_Click(object sender, EventArgs e)
        {

        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click_1(object sender, EventArgs e)
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

