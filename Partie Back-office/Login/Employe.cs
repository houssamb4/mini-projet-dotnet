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

        private void utilisateur_Load(object sender, EventArgs e)
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
                //lblTotalReservations.Text = totalReservations.ToString();

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
            home hm = new home();
            hm.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {

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

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}


