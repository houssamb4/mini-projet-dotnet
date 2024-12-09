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
    public partial class Employe : Form
    {
        char caractere, lettre;
        SqlDataReader result;

        public Employe()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Employe_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;


            LoadData();
        }

        private void LoadData(string searchQuery = "")
        {
            try
            {
                bd.OpenConnexion();

                string query = "SELECT nom_complet, email, status, last_login, created_at FROM Utilisateur";

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query += " WHERE nom_complet LIKE @search OR email LIKE @search";
                }

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        command.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                    }

                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.Rows.Clear(); // Clear existing rows
                    dataGridView1.AutoGenerateColumns = false;

                    foreach (DataRow row in dataTable.Rows)
                    {
                        DateTime lastLogin = row["last_login"] == DBNull.Value
                            ? DateTime.Now
                            : Convert.ToDateTime(row["last_login"]);
                        string lastLoginText = GetRelativeTime(lastLogin);

                        DateTime createdAt = row["created_at"] == DBNull.Value
                            ? DateTime.Now
                            : Convert.ToDateTime(row["created_at"]);
                        string createdAtText = GetRelativeTime(createdAt);

                        dataGridView1.Rows.Add(
                            row["nom_complet"],
                            row["email"],
                            row["status"],
                            lastLoginText,
                            createdAtText
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBox1.Text.Trim();
            LoadData(searchQuery);
        }


        private string GetRelativeTime(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Just now";
            else if (timeSpan.TotalMinutes < 60)
                return $"{Math.Floor(timeSpan.TotalMinutes)} minutes ago";
            else if (timeSpan.TotalHours < 24)
                return $"{Math.Floor(timeSpan.TotalHours)} hours ago";
            else if (timeSpan.TotalDays < 7)
                return $"{Math.Floor(timeSpan.TotalDays)} days ago";
            else
                return dateTime.ToString("MMM dd, yyyy");
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "Entrez nom complet ou e-mail")
            {
                textBox1.Text = "";
                textBox1.ForeColor = System.Drawing.Color.Black; 
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Entrez nom complet ou e-mail";
                textBox1.ForeColor = System.Drawing.Color.Gray;
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
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

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
            home hm = new home();
            hm.Show();
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}


