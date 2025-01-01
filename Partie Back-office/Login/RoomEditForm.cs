using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Login
{
    public partial class RoomEditForm : Form
    {
        private string roomId;
        Database bd = new Database();

        public RoomEditForm(string roomId)
        {
            InitializeComponent();
            this.roomId = roomId;
        }

        private void RoomEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                bd.OpenConnexion();

                string query = "SELECT * FROM chambre WHERE id = @roomId";
                SqlCommand cmd = new SqlCommand(query, bd.getConnexion);
                cmd.Parameters.AddWithValue("@roomId", roomId);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtRoomNumber.Text = reader["num_chambre"].ToString();
                    if (reader["type_chambre_id"].ToString() == "1")
                    {
                        comboBoxRoomType.SelectedItem = "Single";
                    }
                    else if (reader["type_chambre_id"].ToString() == "2")
                    {
                        comboBoxRoomType.SelectedItem = "Double";
                    }
                    comboBoxStatus.SelectedItem = reader["status"].ToString();
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading room details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                bd.OpenConnexion();

                string roomTypeId = "";
                if (comboBoxRoomType.SelectedItem.ToString() == "Single")
                {
                    roomTypeId = "1"; 
                }
                else if (comboBoxRoomType.SelectedItem.ToString() == "Double")
                {
                    roomTypeId = "2"; 
                }
                else if (comboBoxRoomType.SelectedItem.ToString() == "Lux")
                {
                    roomTypeId = "3";
                }
                else if (comboBoxRoomType.SelectedItem.ToString() == "VIP")
                {
                    roomTypeId = "4";
                }

                string query = "UPDATE chambre SET num_chambre = @roomNumber, type_chambre_id = @roomType, status = @status WHERE id = @roomId";
                SqlCommand cmd = new SqlCommand(query, bd.getConnexion);
                cmd.Parameters.AddWithValue("@roomId", roomId);
                cmd.Parameters.AddWithValue("@roomNumber", txtRoomNumber.Text);
                cmd.Parameters.AddWithValue("@roomType", roomTypeId); 
                cmd.Parameters.AddWithValue("@status", comboBoxStatus.SelectedItem.ToString());

                cmd.ExecuteNonQuery();

                MessageBox.Show("Room details updated successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating room details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }
    }
}
