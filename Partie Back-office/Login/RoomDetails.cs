using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace Login
{
    public partial class RoomDetails : Form
    {
        private string roomId;
        private string numRoom;
        private string typeDescription;
        private string status;
        private int type_chambre_id;  

        Database bd = new Database();

        public RoomDetails(string roomId, string numRoom, int type_chambre_id, string typeDescription, string status)
        {
            InitializeComponent();
            this.roomId = roomId;
            this.numRoom = numRoom;
            this.typeDescription = typeDescription;
            this.type_chambre_id = type_chambre_id;  
            this.status = status;
        }

        private void RoomDetails_Load(object sender, EventArgs e)
        {
            lblRoomId.Text = "Room ID: " + roomId;
            lblRoomNumber.Text = "Room Num: " + numRoom;
            lblStatus.Text = "Status: " + status;

            LoadRoomImage(type_chambre_id);
            FetchRoomDetails(roomId);
        }

        private void FetchRoomDetails(string roomId)
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
                    SELECT type_chambre_id
                    FROM chambre
                    WHERE id = @roomId";

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    command.Parameters.AddWithValue("@roomId", roomId);
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string typeChambreId = result.ToString();

                        string additionalQuery = @"
                            SELECT description, prix_par_nuit, occupation_max, type_name
                            FROM type_chambre
                            WHERE id = @typeChambreId";

                        using (SqlCommand additionalCommand = new SqlCommand(additionalQuery, bd.getConnexion))
                        {
                            additionalCommand.Parameters.AddWithValue("@typeChambreId", typeChambreId);

                            using (SqlDataReader reader = additionalCommand.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string description = reader["description"].ToString();
                                    string pricePerNight = reader["prix_par_nuit"].ToString();
                                    string maxOccupancy = reader["occupation_max"].ToString();
                                    string typeName = reader["type_name"].ToString();

                                    lblRoomDescription.Text = $"Room Description: {description}";
                                    lblPricePerNight.Text = $"Price per Night: {pricePerNight}$";
                                    lblMaxOccupancy.Text = $"Max Occupancy: {maxOccupancy} people";
                                }
                                else
                                {
                                    MessageBox.Show("No additional details found for this room.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No type_chambre_id found for this room.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error fetching room details: " + ex.Message);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadRoomImage(int roomType)
        {
            string imagePath = string.Empty;

            switch (roomType)
            {
                case 1:
                    imagePath = "C:\\Users\\houss\\source\\repos\\houssamb4\\mini-projet-dotnet\\Partie Back-office\\Login\\Resources\\single.jpg";  
                    break;
                case 2:
                    imagePath = "C:\\Users\\houss\\source\\repos\\houssamb4\\mini-projet-dotnet\\Partie Back-office\\Login\\Resources\\double.png";  
                    break;
                case 3:
                    imagePath = "C:\\Users\\houss\\source\\repos\\houssamb4\\mini-projet-dotnet\\Partie Back-office\\Login\\Resources\\lux.jpeg"; 
                    break;
                case 4:
                    imagePath = "C:\\Users\\houss\\source\\repos\\houssamb4\\mini-projet-dotnet\\Partie Back-office\\Login\\Resources\\vip.jpg";  
                    break;
                default:
                    imagePath = "C:\\Users\\houss\\source\\repos\\houssamb4\\mini-projet-dotnet\\Partie Back-office\\Login\\Resources\\default.png";  
                    break;
            }
            try
            {
                pictureBoxRoom.Image = Image.FromFile(imagePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }
        }




        private void btnBook_Click(object sender, EventArgs e)
        {

            if (status == "Available")
            {
                Reservationform reservationForm = new Reservationform();
                reservationForm.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("This room is not available for booking.");
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            RoomEditForm editForm = new RoomEditForm(roomId);
            editForm.ShowDialog();
        }

        private void lblAdditionalDetails_Click(object sender, EventArgs e)
        {
        }

        private void pictureBoxRoom_Click(object sender, EventArgs e)
        {

        }
    }
}
