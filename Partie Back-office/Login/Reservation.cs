using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Login
{
    public partial class Reservation : Form
    {
        SqlDataReader result;
        Database bd = new Database();

        public Reservation()
        {
            InitializeComponent();
        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;
            LoadReservationDataIntoDataGridView();
        }

        private void LoadReservationDataIntoDataGridView(string searchQuery = "")
        {
            string query = @"
        SELECT 
            r.id AS [Reservation ID],
            c.nom_complet AS [Nom du client],
            t.type_name AS [Type Chambre],
            r.date_arrivee AS [Date Arrivée],
            r.date_depart AS [Date Départ],
            r.status AS [Status]
        FROM [dbo].[reservation] r
        INNER JOIN [dbo].[Client] c ON r.client_id = c.id
        INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id";

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                query += " WHERE c.nom_complet LIKE @search OR r.status LIKE @search";
            }

            try
            {
                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.Rows.Clear();

                bd.OpenConnexion();

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["Nom du client"],
                            row["Type Chambre"],
                            Convert.ToDateTime(row["Date Arrivée"]).ToString("yyyy-MM-dd"),
                            Convert.ToDateTime(row["Date Départ"]).ToString("yyyy-MM-dd"),
                            row["Status"],
                            "Details"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading reservation data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (bd.getConnexion.State == ConnectionState.Open)
                {
                    bd.CloseConnexion();
                }
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBox1.Text.Trim();
            LoadReservationDataIntoDataGridView(searchQuery);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rooms rom = new Rooms();
            rom.Show();
            this.Hide();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Account acc = new Account();
            acc.Show();
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

        private void button9_Click(object sender, EventArgs e)
        {
            Reservationform resf = new Reservationform();
            resf.Show();
            this.Hide();
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "Rechercher par nom ou statut de client...")
            {
                textBox1.Text = "";
                textBox1.ForeColor = System.Drawing.Color.Black; 
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Rechercher par nom ou statut de client...";
                textBox1.ForeColor = System.Drawing.Color.Gray; 
            }
        }

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void button10_Click(object sender, EventArgs e)
        {

        }
    }
}
