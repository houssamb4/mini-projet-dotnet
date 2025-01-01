using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Data;

namespace Login
{
    public partial class Dashboard : Form
    {
        Database bd = new Database();

        public Dashboard()
        {
            InitializeComponent();
            LoadDataIntoDataGridView();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            string[] queries =
            {
                "SELECT COUNT(*) FROM reservation",
                "SELECT COUNT(*) FROM Client",
                "SELECT COUNT(*) FROM reservation WHERE status = 'En attente'",
                "SELECT COUNT(*) FROM chambre WHERE status = 'Available'"
            };

            Label[] labels = { label13, label14, label16, label15 };
            string[] errorMessages =
            {
                "Error loading reservations count",
                "Error loading clients count",
                "Error loading pending reservations count",
                "Error loading available rooms count"
            };

            try
            {
                bd.OpenConnexion();

                for (int i = 0; i < queries.Length; i++)
                {
                    try
                    {
                        using (SqlCommand cmd = new SqlCommand(queries[i], bd.getConnexion))
                        {
                            object result = cmd.ExecuteScalar();
                            labels[i].Text = result != null ? result.ToString() : "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"{errorMessages[i]}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        labels[i].Text = "Error";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading statistics: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                using (SqlCommand command = new SqlCommand(updateQuery, bd.getConnexion))
                {
                    command.Parameters.AddWithValue("@UserId", GetCurrentUserId());
                    command.ExecuteNonQuery();
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

        private void LoadDataIntoDataGridView()
        {
            string query = @"
    SELECT 
    ActivityLog.ActivityType AS [Type d'activité], 
    Utilisateur.nom_complet AS [Interprété par], 
    ActivityLog.created_at AS [Date et heure]
    FROM ActivityLog
    INNER JOIN Utilisateur ON ActivityLog.UserID = Utilisateur.id
    ORDER BY ActivityLog.created_at DESC";

            try
            {
                bd.OpenConnexion();

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    dataTable.Columns.Add("RelativeTime", typeof(string));

                    foreach (DataRow row in dataTable.Rows)
                    {
                        if (row["Date et heure"] != DBNull.Value)
                        {
                            DateTime dateTime = Convert.ToDateTime(row["Date et heure"]);
                            row["RelativeTime"] = GetRelativeTime(dateTime);
                        }
                        else
                        {
                            row["RelativeTime"] = "N/A";
                        }
                    }

                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Columns["dataGridViewTextBoxColumn1"].DataPropertyName = "Type d'activité";
                    dataGridView1.Columns["dataGridViewTextBoxColumn2"].DataPropertyName = "Interprété par";
                    dataGridView1.Columns["dataGridViewTextBoxColumn3"].DataPropertyName = "RelativeTime";

                    dataGridView1.DataSource = dataTable;
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"SQL Error: {sqlEx.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Data Load Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private string GetRelativeTime(DateTime dateTime)
        {
            TimeSpan timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "il y a quelques secondes";
            if (timeSpan.TotalMinutes < 60)
                return $"il y a {Math.Floor(timeSpan.TotalMinutes)} minute(s)";
            if (timeSpan.TotalHours < 24)
                return $"il y a {Math.Floor(timeSpan.TotalHours)} heure(s)";
            if (timeSpan.TotalDays < 30)
                return $"il y a {Math.Floor(timeSpan.TotalDays)} jour(s)";
            if (timeSpan.TotalDays < 365)
                return $"il y a {Math.Floor(timeSpan.TotalDays / 30)} mois";

            return $"il y a {Math.Floor(timeSpan.TotalDays / 365)} an(s)";
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
            InitializeComponent();
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

        private void button6_Click(object sender, EventArgs e)
        {
            Reservation res = new Reservation();
            res.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Payments pay = new Payments();
            pay.Show();
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
