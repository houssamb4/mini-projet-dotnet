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
    public partial class Payments : Form
    {
        char caractere, lettre;
        SqlDataReader result;

        public Payments()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;


            LoadStatistics();
            LoadDataIntoDataGridView();
        }

        private void LoadStatistics()
        {
            try
            {
                bd.OpenConnexion();

                SqlCommand cmdTotalAmount = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM reservation WHERE status = 'Payé'", bd.getConnexion);
                object resultTotalAmount = cmdTotalAmount.ExecuteScalar();
                decimal totalReservationAmount = resultTotalAmount != null ? Convert.ToDecimal(resultTotalAmount) : 0;
                label13.Text = $"${totalReservationAmount:N2}"; 

                SqlCommand cmdPendingTotalAmount = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM reservation WHERE status = 'En attente'", bd.getConnexion);
                object resultPendingTotalAmount = cmdPendingTotalAmount.ExecuteScalar();
                decimal totalPendingAmount = resultPendingTotalAmount != null ? Convert.ToDecimal(resultPendingTotalAmount) : 0;
                label9.Text = $"${totalPendingAmount:N2}";


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


        private void LoadDataIntoDataGridView()
        {
            string query = @"
    SELECT 
        p.id AS [#],
        c.nom_complet AS [Client Name],
        p.reservation_id AS [Reservation ID],
        p.montant AS [Amount],
        p.date AS [Payment Date],
        r.status AS [Status]
    FROM [dbo].[paiement] p
    INNER JOIN [dbo].[reservation] r ON p.reservation_id = r.id
    INNER JOIN [dbo].[Client] c ON r.client_id = c.id";

            try
            {
                dataGridView1.AutoGenerateColumns = false;

                bd.OpenConnexion(); 

                SqlDataAdapter adapter = new SqlDataAdapter(query, bd.getConnexion);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.Rows.Clear();
                foreach (DataRow row in dataTable.Rows)
                {
                    dataGridView1.Rows.Add(
                        row["#"],
                        row["Client Name"],
                        row["Reservation ID"],
                        row["Amount"],
                        row["Payment Date"],
                        row["Status"]
                    );
                }

                bd.CloseConnexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bd.getConnexion.State == ConnectionState.Open)
                {
                    bd.CloseConnexion();
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

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

