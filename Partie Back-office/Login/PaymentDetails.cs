using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login
{
    public partial class PaymentDetails : Form
    {
        private string paymentId; 
        Database bd = new Database(); 

        public PaymentDetails(string paymentId)
        {
            InitializeComponent();
            this.paymentId = paymentId;
        }

        private void PaymentDetails_Load(object sender, EventArgs e)
        {
            LoadPaymentDetails();
        }

        private void LoadPaymentDetails()
        {
            string query = @"
    SELECT 
        p.id AS [Payment ID],
        c.nom_complet AS [Client Name],
        c.email AS [Client Email],
        c.telephone AS [Client Phone],
        p.reservation_id AS [Reservation ID],
        p.montant AS [Amount],
        r.guests AS [Number of Guests],
        r.status AS [Reservation Status],
        r.CreatedAt AS [Reservation Date],
        p.date AS [Payment Date],
        r.status AS [Status],
        r.date_arrivee AS [Reservation Start Date],
        r.date_depart AS [Reservation End Date],
        t.description AS [Room Type],
        p.mode_paiment AS [Payment Method],
        t.prix_par_nuit AS [Room Price per night]
    FROM [dbo].[paiement] p
    INNER JOIN [dbo].[reservation] r ON p.reservation_id = r.id
    INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id
    INNER JOIN [dbo].[Client] c ON r.client_id = c.id
    WHERE p.id = @PaymentID";

            try
            {
                bd.OpenConnexion();

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    command.Parameters.AddWithValue("@PaymentID", paymentId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            label20.Text = "Payment ID: " + reader["Payment ID"].ToString();
                            label7.Text = "Client Name: " + reader["Client Name"].ToString();
                            label8.Text = "Client Email: " + reader["Client Email"].ToString();
                            label9.Text = "Client Phone: " + reader["Client Phone"].ToString();
                            label10.Text = "Reservation ID: " + reader["Reservation ID"].ToString();
                            label14.Text = "Number of Guests: " + reader["Number of Guests"].ToString();
                            label17.Text = "Total Amount: $" + reader["Amount"].ToString();
                            label15.Text = "Reservation Status: " + reader["Reservation Status"].ToString(); 
                            label16.Text = "Booked on: " + Convert.ToDateTime(reader["Reservation Date"]).ToString("yyyy-MM-dd");
                            label22.Text = "Payment Date: " + Convert.ToDateTime(reader["Payment Date"]).ToString("yyyy-MM-dd");
                            label21.Text = "Payment Status: " + reader["Status"].ToString();

                            label11.Text = "Check-in Date: " + Convert.ToDateTime(reader["Reservation Start Date"]).ToString("yyyy-MM-dd");
                            label12.Text = "Check-out Date: " + Convert.ToDateTime(reader["Reservation End Date"]).ToString("yyyy-MM-dd");

                            DateTime checkInDate = Convert.ToDateTime(reader["Reservation Start Date"]);
                            DateTime checkOutDate = Convert.ToDateTime(reader["Reservation End Date"]);
                            int numberOfNights = (checkOutDate - checkInDate).Days;
                            label13.Text = "Number of Nights: " + numberOfNights;

                            label19.Text = "Room Type: " + reader["Room Type"].ToString();
                            label23.Text = "Payment Method: " + reader["Payment Method"].ToString();
                            label18.Text = "Room Price per Night: $" + reader["Room Price per night"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No payment details found for the given Payment ID.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading payment details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }



        private void label13_Click(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }
    }
}
