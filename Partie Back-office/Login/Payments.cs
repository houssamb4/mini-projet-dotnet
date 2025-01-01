using Org.BouncyCastle.Tls;
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
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PaymentId", 
                    HeaderText = "Payment ID",
                    Visible = false 
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ClientName",
                    HeaderText = "Client Name"
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "ReservationId",
                    HeaderText = "Reservation ID"
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Amount",
                    HeaderText = "Amount"
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "PaymentDate",
                    HeaderText = "Payment Date"
                });

                dataGridView1.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Status",
                    HeaderText = "Status"
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
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            dataGridView1.Rows.Add(
                                row["#"], 
                                row["Client Name"],
                                row["Reservation ID"],
                                row["Amount"],
                                Convert.ToDateTime(row["Payment Date"]).ToString("yyyy-MM-dd"),
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
                string paymentId = dataGridView1.Rows[e.RowIndex].Cells["PaymentId"].Value?.ToString();

                if (!string.IsNullOrEmpty(paymentId))
                {
                    try
                    {
                        PaymentDetails detailsForm = new PaymentDetails(paymentId);
                        detailsForm.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occurred while opening the payment details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Payment ID is missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            Rooms rooms = new Rooms();
            rooms.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reservation reservation = new Reservation();
            reservation.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Show();
            this.Hide();

        }


        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }


        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

