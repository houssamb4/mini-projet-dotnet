using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login
{
    public partial class EditReservationForm : Form
    {
        private readonly Database bd = new Database();
        private readonly string reservationId;

        // Constructor accepting reservationId
        public EditReservationForm(string reservationId)
        {
            InitializeComponent();
            this.reservationId = reservationId;

            // Load form data
            LoadReservationDetails();
            PopulateRoomTypeComboBox();
            PopulateStatusComboBox();
        }

        // Draw a border around the form
        private void EditReservationForm_Paint(object sender, PaintEventArgs e)
        {
            using (var borderPen = new System.Drawing.Pen(System.Drawing.Color.Blue, 5))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, this.ClientSize.Width - 1, this.ClientSize.Height - 1);
            }
        }

        // Populate Room Type ComboBox
        private void PopulateRoomTypeComboBox()
        {
            roomTypeComboBox.Items.AddRange(new[] { "Single", "Double", "Lux", "VIP" });
        }

        // Populate Status ComboBox
        private void PopulateStatusComboBox()
        {
            statusComboBox.Items.AddRange(new[] { "En attente", "Payé", "Annulé" });
        }

        // Load reservation details into the form
        private void LoadReservationDetails()
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
                SELECT 
                    c.nom_complet AS [ClientName],
                    c.email AS [ClientEmail],
                    t.type_name AS [RoomType],
                    r.date_arrivee AS [CheckInDate],
                    r.date_depart AS [CheckOutDate],
                    r.status AS [Status],
                    r.TotalAmount AS [TotalAmount]
                FROM [dbo].[reservation] r
                INNER JOIN [dbo].[Client] c ON r.client_id = c.id
                INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id
                WHERE r.id = @reservationId";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientNameTextBox.Text = reader["ClientName"].ToString();
                            clientEmailTextBox.Text = reader["ClientEmail"].ToString();
                            roomTypeComboBox.Text = reader["RoomType"].ToString();
                            checkInDatePicker.Value = Convert.ToDateTime(reader["CheckInDate"]);
                            checkOutDatePicker.Value = Convert.ToDateTime(reader["CheckOutDate"]);
                            statusComboBox.Text = reader["Status"].ToString();
                            totalAmountTextBox.Text = reader["TotalAmount"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("No reservation found with the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearFormFields();
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        // Save changes to the reservation
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (ValidateFormFields())
            {
                DialogResult result = MessageBox.Show(
                    "Do you want to save the changes?",
                    "Save Changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    SaveChanges();
                }
            }
        }

        private void SaveChanges()
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
                UPDATE [dbo].[reservation]
                SET 
                    date_arrivee = @checkInDate,
                    date_depart = @checkOutDate,
                    status = @status,
                    TotalAmount = @totalAmount,
                    chambre_type_id = (SELECT id FROM [dbo].[type_chambre] WHERE type_name = @roomType)
                WHERE id = @reservationId";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);
                    cmd.Parameters.AddWithValue("@checkInDate", checkInDatePicker.Value);
                    cmd.Parameters.AddWithValue("@checkOutDate", checkOutDatePicker.Value);
                    cmd.Parameters.AddWithValue("@status", statusComboBox.Text);
                    cmd.Parameters.AddWithValue("@totalAmount", totalAmountTextBox.Text);
                    cmd.Parameters.AddWithValue("@roomType", roomTypeComboBox.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No changes were made.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedType = this.roomTypeComboBox.SelectedItem.ToString();

            switch (selectedType)
            {
                case "Single":
                    this.totalAmountTextBox.Text = "99.00";
                    break;
                case "Double":
                    this.totalAmountTextBox.Text = "199.00";
                    break;
                case "Lux":
                    this.totalAmountTextBox.Text = "299.00";
                    break;
                case "VIP":
                    this.totalAmountTextBox.Text = "399.00";
                    break;
                default:
                    this.totalAmountTextBox.Text = "00.00";
                    break;
            }
        }

        private void ClearFormFields()
        {
            clientNameTextBox.Clear();
            clientEmailTextBox.Clear();
            roomTypeComboBox.SelectedIndex = -1;
            checkInDatePicker.Value = DateTime.Now;
            checkOutDatePicker.Value = DateTime.Now;
            statusComboBox.SelectedIndex = -1;
            totalAmountTextBox.Clear();
        }

        private bool ValidateFormFields()
        {
            if (string.IsNullOrWhiteSpace(roomTypeComboBox.Text) ||
                string.IsNullOrWhiteSpace(statusComboBox.Text) ||
                string.IsNullOrWhiteSpace(totalAmountTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
}
