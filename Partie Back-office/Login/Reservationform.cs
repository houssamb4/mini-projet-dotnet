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
    public partial class Reservationform : Form
    {

        public Reservationform()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;
            LoadSuggestions();
        }



        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);


        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            home hm = new home();
            hm.Show();
            this.Hide();

        }

        private void TextBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "Entrez le nom complet ou l'adresse e-mail")
            {
                textBox1.Text = ""; 
                textBox1.ForeColor = System.Drawing.Color.Black; 
            }
        }

        private void TextBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Entrez le nom complet ou l'adresse e-mail"; 
                textBox1.ForeColor = System.Drawing.Color.Gray; 
            }
        }

        private void LoadSuggestions()
        {
            try
            {
                bd.OpenConnexion();

                string query = "SELECT id, nom_complet, email FROM Client";

                SqlCommand cmd = new SqlCommand(query, bd.getConnexion);
                SqlDataReader reader = cmd.ExecuteReader();

                AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();

                while (reader.Read())
                {
                    string suggestion = $"{reader["nom_complet"]} ({reader["email"]}) - {reader["id"]}";
                    autoCompleteCollection.Add(suggestion);
                }

                textBox1.AutoCompleteCustomSource = autoCompleteCollection;
                textBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                textBox1.AutoCompleteSource = AutoCompleteSource.CustomSource;

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading suggestions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
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

        private void button9_Click(object sender, EventArgs e)
        {
            Reservation res = new Reservation();
            res.Show();
            this.Hide();
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }


        private void buttonInsert_Click(object sender, EventArgs e)
        {
            InsertReservation();
        }

        private int InsertReservation()
        {
            try
            {
                int clientId = GetSelectedClientId(textBox1.Text);
                DateTime dateArrivee = dateTimePicker1.Value;
                DateTime dateDepart = dateTimePicker2.Value;
                string status = "Pending"; // Initially setting the status to 'Pending'
                int chambreTypeId = GetSelectedChambreTypeId(comboBox1.SelectedItem?.ToString());
                decimal totalAmount = decimal.Parse(textBox2.Text.Trim('$'));
                string createdBy = "admin";

                bd.OpenConnexion();

                string query = @"
            INSERT INTO reservation 
            (client_id, date_arrivee, date_depart, status, CreatedAt, TotalAmount, created_by, chambre_type_id)
            VALUES (@client_id, @date_arrivee, @date_depart, @status, @CreatedAt, @TotalAmount, @created_by, @chambre_type_id);
            SELECT SCOPE_IDENTITY();";  // Get the last inserted reservation ID

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@client_id", clientId);
                    cmd.Parameters.AddWithValue("@date_arrivee", dateArrivee);
                    cmd.Parameters.AddWithValue("@date_depart", dateDepart);
                    cmd.Parameters.AddWithValue("@status", status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TotalAmount", totalAmount);
                    cmd.Parameters.AddWithValue("@created_by", createdBy);
                    cmd.Parameters.AddWithValue("@chambre_type_id", chambreTypeId);

                    int reservationId = Convert.ToInt32(cmd.ExecuteScalar());

                    return reservationId; 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                bd.CloseConnexion();
            }
        }



        private void UpdateTotalAmount()
        {
            try
            {
                if (dateTimePicker1.Value.Date >= dateTimePicker2.Value.Date)
                {
                    MessageBox.Show("Check-out date must be later than check-in date.", "Invalid Dates", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBox2.Text = "0.00"; 
                    return;
                }

                int numberOfNights = (dateTimePicker2.Value.Date - dateTimePicker1.Value.Date).Days;

                string selectedType = comboBox1.SelectedItem?.ToString();
                double pricePerNight = 0;

                switch (selectedType)
                {
                    case "Single room":
                        pricePerNight = 99.00;
                        break;
                    case "Double room":
                        pricePerNight = 199.00;
                        break;
                    case "Lux room":
                        pricePerNight = 299.00;
                        break;
                    case "VIP room":
                        pricePerNight = 399.00;
                        break;
                    default:
                        textBox2.Text = "Invalid room type";
                        return;
                }

                double totalAmount = numberOfNights * pricePerNight;

                textBox2.Text = $"{totalAmount:0.00}"; 
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while calculating the total amount: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTotalAmount();
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotalAmount();
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateTotalAmount();
        }

        private int GetSelectedClientId(string selectedClient)
        {
            if (!string.IsNullOrWhiteSpace(selectedClient) && selectedClient.Contains("-"))
            {
                string[] parts = selectedClient.Split('-');
                if (int.TryParse(parts.LastOrDefault()?.Trim(), out int clientId))
                {
                    return clientId;
                }
            }
            throw new Exception("Invalid client selected.");
        }

        private int GetSelectedChambreTypeId(string chambreType)
        {
            switch (chambreType)
            {
                case "Single room": return 1;
                case "Double room": return 2;
                case "Lux room": return 3;
                case "VIP room": return 4;
                default: throw new Exception("Invalid chambre type selected.");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Payments payments = new Payments();
            payments.Show();
            this.Hide();
        }

        private void SavePaymentData(int reservationId, DateTime paymentDate, string paymentMethod, bool isPaymentReceived)
        {
            try
            {
                decimal paymentAmount = decimal.Parse(textBox2.Text.Trim('$')); 

                string query = @"
            INSERT INTO paiement 
            (reservation_id, date, montant, mode_paiment, received)
            VALUES (@reservation_id, @date, @montant, @mode_paiment, @received)";

                bd.OpenConnexion();

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservation_id", reservationId);
                    cmd.Parameters.AddWithValue("@date", paymentDate);
                    cmd.Parameters.AddWithValue("@montant", paymentAmount);
                    cmd.Parameters.AddWithValue("@mode_paiment", paymentMethod);
                    cmd.Parameters.AddWithValue("@received", isPaymentReceived);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Payment data inserted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to insert payment data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving payment data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void UpdateReservationStatus(int reservationId)
        {
            try
            {
                string query = "UPDATE reservation SET status = 'Payé' WHERE id = @reservation_id";

                bd.OpenConnexion();

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservation_id", reservationId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation status updated to 'Payé'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update reservation status.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating reservation status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() == "Payé")
            {
                using (PaymentForm pay = new PaymentForm())
                {
                    if (pay.ShowDialog() == DialogResult.OK)
                    {
                        DateTime paymentDate = pay.PaymentDate;
                        string paymentMethod = pay.PaymentMethod;
                        bool isPaymentReceived = pay.IsPaymentReceived;

                        int reservationId = InsertReservation();

                        if (reservationId > 0)
                        {
                            UpdateReservationStatus(reservationId);

                            SavePaymentData(reservationId, paymentDate, paymentMethod, isPaymentReceived);

                            MessageBox.Show($"Payment Details:\n\nDate: {paymentDate:yyyy-MM-dd}\nMethod: {paymentMethod}\nReceived: {isPaymentReceived}",
                                "Payment Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Failed to insert reservation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }
    }
}


