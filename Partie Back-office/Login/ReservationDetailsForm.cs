using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class ReservationDetailsForm : Form
    {
        private int reservationId;
        Database bd = new Database();

        public ReservationDetailsForm(int reservationId)
        {
            InitializeComponent();
            this.reservationId = reservationId;
            LoadReservationDetails(); 
        }

        private void LoadReservationDetails()
        {
            try
            {
                // Your code to fetch and display the reservation details using reservationId
                // For example:
                string query = "SELECT * FROM reservation WHERE id = @reservationId";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Populate your controls with data
                        labelClientName.Text = reader["client_name"].ToString(); // Assuming column name is client_name
                        labelRoomType.Text = reader["room_type"].ToString(); // Assuming column name is room_type
                        labelCheckInDate.Text = reader["check_in_date"].ToString(); // Assuming column name is check_in_date
                        labelCheckOutDate.Text = reader["check_out_date"].ToString(); // Assuming column name is check_out_date
                        labelStatus.Text = reader["status"].ToString(); // Assuming column name is status
                        labelTotalAmount.Text = reader["total_amount"].ToString(); // Assuming column name is total_amount
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading the reservation details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
