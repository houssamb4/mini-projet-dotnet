using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Login
{
    public partial class Reservation : Form
    {
        Database bd = new Database();

        public Reservation()
        {
            InitializeComponent();
        }

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;
            LoadReservationDataIntoDataGridView();
            StyleDataGridView();
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
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ReservationID",
                    HeaderText = "Reservation ID",
                    Visible = false 
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "NomDuClient",
                    HeaderText = "Nom du client",
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "TypeChambre",
                    HeaderText = "Type Chambre",
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DateArrivee",
                    HeaderText = "Date Arrivée",
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "DateDepart",
                    HeaderText = "Date Départ",
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Status",
                });

                dataGridView1.Columns.Add(new DataGridViewButtonColumn
                {
                    Name = "detailsButton",
                    HeaderText = "Action",
                    Text = "Details",
                    UseColumnTextForButtonValue = true
                });

                bd.OpenConnexion();

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            dataGridView1.Rows.Add(
                                row["Reservation ID"],
                                row["Nom du client"],
                                row["Type Chambre"],
                                Convert.ToDateTime(row["Date Arrivée"]).ToString("yyyy-MM-dd"),
                                Convert.ToDateTime(row["Date Départ"]).ToString("yyyy-MM-dd"),
                                row["Status"]
                            );
                        }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "detailsButton")
            {
                string reservationId = dataGridView1.Rows[e.RowIndex].Cells["ReservationID"].Value?.ToString();

                if (!string.IsNullOrEmpty(reservationId))
                {
                    ReservationDetailsForm detailsForm = new ReservationDetailsForm(reservationId);
                    detailsForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Reservation ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void StyleDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            dataGridView1.GridColor = System.Drawing.Color.LightGray;

            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            headerStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle = headerStyle;
            dataGridView1.ColumnHeadersHeight = 35;

            DataGridViewCellStyle rowStyle = new DataGridViewCellStyle();
            rowStyle.BackColor = System.Drawing.Color.White;
            rowStyle.ForeColor = System.Drawing.Color.Black;
            rowStyle.Font = new System.Drawing.Font("Arial", 9.5F, System.Drawing.FontStyle.Regular);
            rowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(220, 220, 220);
            rowStyle.SelectionForeColor = System.Drawing.Color.Black;
            dataGridView1.DefaultCellStyle = rowStyle;

            DataGridViewCellStyle altRowStyle = new DataGridViewCellStyle();
            altRowStyle.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            altRowStyle.ForeColor = System.Drawing.Color.Black;
            dataGridView1.AlternatingRowsDefaultCellStyle = altRowStyle;

            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataGridView1.RowTemplate.Height = 30;

            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns["detailsButton"].DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(77, 148, 255);
            dataGridView1.Columns["detailsButton"].DefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dataGridView1.Columns["detailsButton"].DefaultCellStyle.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            dataGridView1.Columns["detailsButton"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }


        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBox1.Text.Trim();
            LoadReservationDataIntoDataGridView(searchQuery);
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

        private void button7_Click(object sender, EventArgs e)
        {
            Payments acc = new Payments();
            acc.Show();
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
