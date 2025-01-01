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
using System.Resources;


namespace Login
{
    public partial class Rooms : Form
    {

        public Rooms()
        {
            InitializeComponent();
        }

        Database bd = new Database();

        private void Rooms_Load(object sender, EventArgs e)
        {
            label2.Text = Login.LoggedInUserFullName;

            comboBox1.Items.AddRange(new object[] { "Select Status", "Available", "Occupied", "Under Maintenance" });
            comboBox2.Items.AddRange(new object[] { "Select Floor", "Floor 1", "Floor 2", "Floor 3", "Floor 4", "Floor 5" });
            comboBox3.Items.AddRange(new object[] { "Select Room Type", "Single", "Double", "Lux", "VIP" });

            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;

            StyleDataGridView();
            LoadStatistics();
            LoadData();
        }

        private int GetCurrentUserId()
        {
            return Login.LoggedInUserID;
        }

        private void LoadStatistics()
        {
            try
            {
                bd.OpenConnexion();

                SqlCommand cmdAvailableRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Available'", bd.getConnexion);
                object resultAvailableRooms = cmdAvailableRooms.ExecuteScalar();
                int totalAvailableRooms = resultAvailableRooms != null ? Convert.ToInt32(resultAvailableRooms) : 0;
                label9.Text = totalAvailableRooms.ToString();

                SqlCommand cmdOccupiedRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Occupied'", bd.getConnexion);
                object resultOccupiedRooms = cmdOccupiedRooms.ExecuteScalar();
                int totalOccupiedRooms = resultOccupiedRooms != null ? Convert.ToInt32(resultOccupiedRooms) : 0;
                label11.Text = totalOccupiedRooms.ToString();

                SqlCommand cmdRooms = new SqlCommand("SELECT COUNT(*) FROM chambre", bd.getConnexion);
                object resultRooms = cmdRooms.ExecuteScalar();
                int totalRooms = resultRooms != null ? Convert.ToInt32(resultRooms) : 0;
                label13.Text = totalRooms.ToString();

                SqlCommand cmdUnderRooms = new SqlCommand("SELECT COUNT(*) FROM chambre WHERE status = 'Under Maintenance'", bd.getConnexion);
                object resultUnderRooms = cmdUnderRooms.ExecuteScalar();
                int totalUnderRooms = resultUnderRooms != null ? Convert.ToInt32(resultUnderRooms) : 0;
                label14.Text = totalUnderRooms.ToString();


                bd.CloseConnexion();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
            SELECT
                c.id,
                c.num_chambre, 
                tc.description AS type_description, 
                c.status,
                c.type_chambre_id  -- Add this to fetch type_chambre_id directly
            FROM chambre c
            INNER JOIN type_chambre tc ON c.type_chambre_id = tc.id";

                using (SqlCommand command = new SqlCommand(query, bd.getConnexion))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    dataGridView1.AutoGenerateColumns = false;
                    dataGridView1.Rows.Clear();

                    if (dataGridView1.Columns.Count == 0)
                    {
                        DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
                        {
                            HeaderText = "RoomId",
                            Name = "RoomId",
                            Visible = false
                        };
                        DataGridViewTextBoxColumn typeChambreIdColumn = new DataGridViewTextBoxColumn
                        {
                            HeaderText = "Type Chambre Id",
                            Name = "type_chambre_id",
                            Visible = false
                        };
                        dataGridView1.Columns.Add(idColumn);
                        dataGridView1.Columns.Add(typeChambreIdColumn);

                        dataGridView1.Columns.Add("num_chambre", "Num Chambre");
                        dataGridView1.Columns.Add("type_description", "Type Description");
                        dataGridView1.Columns.Add("status", "Status");

                        DataGridViewButtonColumn detailsButton = new DataGridViewButtonColumn
                        {
                            HeaderText = "Action",
                            Name = "detailsButton",
                            Text = "Details",
                            UseColumnTextForButtonValue = true
                        };
                        dataGridView1.Columns.Add(detailsButton);
                    }

                    foreach (DataRow row in dataTable.Rows)
                    {
                        dataGridView1.Rows.Add(
                            row["id"],                 
                            row["type_chambre_id"],    
                            row["num_chambre"],       
                            row["type_description"],   
                            row["status"]
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void ApplyFilters()
        {
            try
            {
                string selectedStatus = comboBox1.SelectedIndex > 0 ? comboBox1.SelectedItem.ToString() : string.Empty;
                string selectedFloor = comboBox2.SelectedIndex > 0 ? comboBox2.SelectedItem.ToString() : string.Empty;
                string selectedRoomType = comboBox3.SelectedIndex > 0 ? comboBox3.SelectedItem.ToString() : string.Empty;

                Dictionary<string, string> roomTypeMapping = new Dictionary<string, string>
        {
            { "Single", "1" },
            { "Double", "2" },
            { "Lux", "3" },
            { "VIP", "4" }
        };

                int floorNumber = 0;

                if (!string.IsNullOrEmpty(selectedFloor))
                {
                    floorNumber = int.Parse(selectedFloor.Replace("Floor ", ""));
                }

                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.IsNewRow) continue;

                    bool matchesStatus = string.IsNullOrEmpty(selectedStatus) || row.Cells["status"].Value.ToString() == selectedStatus;
                    bool matchesFloor = floorNumber == 0 || row.Cells["num_chambre"].Value.ToString().StartsWith(floorNumber.ToString());
                    bool matchesRoomType = string.IsNullOrEmpty(selectedRoomType) ||
                                           (roomTypeMapping.ContainsKey(selectedRoomType) &&
                                           row.Cells["type_chambre_id"].Value.ToString() == roomTypeMapping[selectedRoomType]);

                    row.Visible = matchesStatus && matchesFloor && matchesRoomType;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error applying filters: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void filter_Click(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "detailsButton")
            {
                string roomId = dataGridView1.Rows[e.RowIndex].Cells["RoomId"].Value?.ToString();
                string numRoom = dataGridView1.Rows[e.RowIndex].Cells["num_chambre"].Value?.ToString();
                string typeDescription = dataGridView1.Rows[e.RowIndex].Cells["type_description"].Value?.ToString();
                string type_chambre_id = dataGridView1.Rows[e.RowIndex].Cells["type_chambre_id"].Value?.ToString();
                string status = dataGridView1.Rows[e.RowIndex].Cells["status"].Value?.ToString();

                if (!string.IsNullOrEmpty(roomId) && !string.IsNullOrEmpty(numRoom) &&
                    !string.IsNullOrEmpty(typeDescription) && !string.IsNullOrEmpty(status) &&
                    !string.IsNullOrEmpty(type_chambre_id))
                {
                    int typeChambreId;
                    if (int.TryParse(type_chambre_id, out typeChambreId))
                    {
                        RoomDetails roomDetails = new RoomDetails(roomId, numRoom, typeChambreId, typeDescription, status);
                        roomDetails.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Invalid Room Type ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("One or more required fields are missing.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }


        private void StyleDataGridView()
        {
            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.White;

            if (dataGridView1.Columns["detailsButton"] is DataGridViewButtonColumn detailsColumn)
            {
                detailsColumn.DefaultCellStyle.BackColor = Color.FromArgb(46, 204, 113);
                detailsColumn.DefaultCellStyle.ForeColor = Color.White;
                detailsColumn.FlatStyle = FlatStyle.Flat;
            }
        }


        private void UpdateUserStatus()
        {
            string updateQuery = @"
        UPDATE Utilisateur
        SET status = 'offline', last_login = GETDATE()
        WHERE id = @UserId";

            try
            {
                bd.OpenConnexion();
                using (SqlConnection connection = bd.getConnexion)
                {
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        int userId = GetCurrentUserId();

                        command.Parameters.AddWithValue("@UserId", userId);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating user status: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateUserStatus();

                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);

        private void tableauClient_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
        }


        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilters();
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

        private void button6_Click(object sender, EventArgs e)
        {
           Reservation reservation = new Reservation();
            reservation.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        { 

        }

        private void button7_Click(object sender, EventArgs e)
        {
            Payments pay = new Payments();
            pay.Show();
            this.Hide();
        }


        private void button8_Click(object sender, EventArgs e)
        {
            Account account = new Account();
            account.Show();
            this.Hide();
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

