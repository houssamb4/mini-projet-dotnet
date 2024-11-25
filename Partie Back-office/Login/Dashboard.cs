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

                // Count Clients
                SqlCommand cmdClients = new SqlCommand("SELECT COUNT(*) FROM Client", bd.getConnexion);
                int totalClients = (int)cmdClients.ExecuteScalar();
                //lblTotalClients.Text = totalClients.ToString();

                // Count Utilisateurs
                SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Utilisateur", bd.getConnexion);
                int totalUsers = (int)cmdUsers.ExecuteScalar();
                //lblTotalUsers.Text = totalUsers.ToString();

                // Count Reservations
                SqlCommand cmdReservations = new SqlCommand("SELECT COUNT(*) FROM reservation", bd.getConnexion);
                int totalReservations = (int)cmdReservations.ExecuteScalar();
                label13.Text = totalReservations.ToString();

                bd.CloseConnexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void tableauClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            // Create the columns
            dataGridView1.Columns.Add("Activity", "Activity");
            dataGridView1.Columns.Add("PerformedBy", "Performed By");
            dataGridView1.Columns.Add("DateTime", "Date & Time");
            dataGridView1.Columns.Add("Status", "Status");

            // Add sample rows
            dataGridView1.Rows.Add("Reservation #1024 confirmed", "Admin John", "2024-11-23 10:30 AM", "Confirmed");
            dataGridView1.Rows.Add("Payment of $300 received for Reservation #1020", "Admin Sarah", "2024-11-22 04:15 PM", "Paid");
            dataGridView1.Rows.Add("Room #101 assigned to John Doe", "Admin Sarah", "2024-11-22 02:00 PM", "Assigned");

            // Optionally, format the Status column
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string status = row.Cells["Status"].Value.ToString();
                if (status == "Confirmed")
                {
                    row.Cells["Status"].Style.BackColor = Color.Green;
                    row.Cells["Status"].Style.ForeColor = Color.White;
                }
                else if (status == "Paid")
                {
                    row.Cells["Status"].Style.BackColor = Color.Blue;
                    row.Cells["Status"].Style.ForeColor = Color.White;
                }
                else if (status == "Assigned")
                {
                    row.Cells["Status"].Style.BackColor = Color.Orange;
                    row.Cells["Status"].Style.ForeColor = Color.White;
                }
            }
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

        }

        private void button8_Click(object sender, EventArgs e)
        {

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

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}
