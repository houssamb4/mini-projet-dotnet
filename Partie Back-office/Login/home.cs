using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Login
{
    public partial class home : Form
    {
        char caractere, lettre;

        public home()
        {
            InitializeComponent();

            label7.Text = Login.LoggedInUserFullName;

        }

        Database bd = new Database();

        private bool IsFormValid()
        {
            if (txtNom.Text.Trim() == string.Empty)
            {
                ErrorNom.Visible = true;
                return false;
            }
            else
            {
                ErrorNom.Visible = false;
            }

            if (txtEmail.Text.Trim() == string.Empty)
            {
                ErrorEmail.Visible = true;
                return false;
            }
            else
            {
                ErrorEmail.Visible = false;
            }

            if (txtPhone.Text.Trim() == string.Empty)
            {
                ErrorPhone.Visible = true;
                return false;
            }
            else
            {
                ErrorPhone.Visible = false;
            }

            if (txtAdresse.Text.Trim() == string.Empty)
            {
                ErrorAdresse.Visible = true;
                return false;
            }
            else
            {
                ErrorAdresse.Visible = false;
            }

            return true;
        }

        Client client = new Client();

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (IsFormValid())
            {
                if (client.AjouterClient(txtNom.Text, txtEmail.Text, txtPhone.Text, txtAdresse.Text))
                {
                    MessageBox.Show(txtNom.Text + " Ajouté avec succès", "Ajout Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNom.Clear();
                    txtPhone.Clear();
                    txtEmail.Clear();
                    txtAdresse.Clear();

                    SelectClient(); 
                }
                else
                {
                    MessageBox.Show(txtNom.Text + " Erreur lors de l'insertion du client", "Ajout Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtEmail_KeyPress(object sender, KeyPressEventArgs e)
        {
            caractere = e.KeyChar;
            if (!char.IsDigit(caractere) && caractere != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            lettre = e.KeyChar;
            if (!char.IsDigit(lettre) && lettre != '+' && lettre != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void home_Load(object sender, EventArgs e)
        {
            SelectClient(); 
        }

        private void SelectClient(string searchQuery = "")
        {
            try
            {
                bd.OpenConnexion();
                tableauClient.Rows.Clear();

                string query = @"
            SELECT id, nom_complet, email, telephone, Adresse, created_at 
            FROM Client ";

                if (!string.IsNullOrWhiteSpace(searchQuery))
                {
                    query += "WHERE nom_complet LIKE @search OR email LIKE @search ";
                }

                query += "ORDER BY created_at DESC";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    if (!string.IsNullOrWhiteSpace(searchQuery))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchQuery + "%");
                    }

                    using (SqlDataReader result = cmd.ExecuteReader())
                    {
                        while (result.Read())
                        {
                            object createdAtObj = result["created_at"];
                            string relativeTime = createdAtObj != DBNull.Value
                                ? GetRelativeTime(Convert.ToDateTime(createdAtObj))
                                : "Unspecified";

                            tableauClient.Rows.Add(
                                result["id"].ToString(),
                                result["nom_complet"].ToString(),
                                result["email"].ToString(),
                                result["telephone"].ToString(),
                                result["Adresse"].ToString(),
                                relativeTime 
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while selecting clients: {ex.Message}", "Unexpected Error");
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private string GetRelativeTime(DateTime createdAt)
        {
            TimeSpan timeSpan = DateTime.Now - createdAt;

            if (timeSpan.TotalSeconds < 60)
                return "il y a quelques secondes";
            if (timeSpan.TotalMinutes < 60)
                return $"il y a {Math.Floor(timeSpan.TotalMinutes)} minute(s)";
            if (timeSpan.TotalHours < 24)
                return $"il y a {Math.Floor(timeSpan.TotalHours)} heure(s)";
            if (timeSpan.TotalDays < 7)
                return $"il y a {Math.Floor(timeSpan.TotalDays)} jour(s)";
            if (timeSpan.TotalDays < 30)
                return $"il y a {Math.Floor(timeSpan.TotalDays / 7)} semaine(s)";
            if (timeSpan.TotalDays < 365)
                return $"il y a {Math.Floor(timeSpan.TotalDays / 30)} mois";
            return $"il y a {Math.Floor(timeSpan.TotalDays / 365)} année(s)";
        }



        private void textBoxClientSearch_TextChanged(object sender, EventArgs e)
        {
            string searchQuery = textBox1.Text.Trim();
            SelectClient(searchQuery);
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (tableauClient.SelectedRows.Count > 0)
            {
                DialogResult dialogResult = MessageBox.Show("Voulez-vous vraiment supprimer ce Client ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    string id = tableauClient.CurrentRow.Cells[0].Value.ToString(); 
                    string nom = tableauClient.CurrentRow.Cells[1].Value.ToString();

                    if (client.DeleteClient(id))
                    {
                        MessageBox.Show(nom + " Supprimé avec succès", "Suppression Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        SelectClient(); 
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la Suppression de " + nom, "Suppression Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private int GetCurrentUserId()
        {
            return Login.LoggedInUserID;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (tableauClient.SelectedRows.Count > 0)
            {
                txtNom.Text = tableauClient.CurrentRow.Cells[1].Value.ToString(); // Access 'nom_complet'
                txtEmail.Text = tableauClient.CurrentRow.Cells[2].Value.ToString(); // Access 'email'
                txtPhone.Text = tableauClient.CurrentRow.Cells[3].Value.ToString(); // Access 'telephone'
                txtAdresse.Text = tableauClient.CurrentRow.Cells[4].Value.ToString(); // Access 'adresse'
                string id = tableauClient.CurrentRow.Cells[0].Value.ToString(); // Access 'id'

                btnUpdateClt.Visible = true;
                btnUpdate.Enabled = false;
            }
        }

        private void textBox1_GotFocus(object sender, EventArgs e)
        {
            if (textBox1.Text == "Entrez le nom complet ou l'e-mail du client")
            {
                textBox1.Text = "";
                textBox1.ForeColor = System.Drawing.Color.Black; 
            }
        }

        private void textBox1_LostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                textBox1.Text = "Entrez le nom complet ou l'e-mail du client";
                textBox1.ForeColor = System.Drawing.Color.Gray; 
            }
        }

        private void btnUpdateClt_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFormValid())
                {
                    string id = tableauClient.CurrentRow.Cells[0].Value.ToString(); 

                    if (client.UpdateClient(id, txtNom.Text, txtEmail.Text, txtPhone.Text, txtAdresse.Text))
                    {
                        MessageBox.Show(txtNom.Text + " Modifié avec succès", "Modification Client", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtNom.Clear();
                        txtPhone.Clear();
                        txtEmail.Clear();
                        txtAdresse.Clear();
                        SelectClient(); // Refresh client list

                        btnUpdateClt.Visible = false;
                        btnUpdate.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show(txtNom.Text + " Erreur lors de la Modification du client", "Modifier Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur d'insertion");
            }
        }

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);


        private void tableauClient_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  
            {
                this.tableauClient.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.LightCyan; // Hover effect on row
            }
        }

        private void tableauClient_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)  
            {
                this.tableauClient.Rows[e.RowIndex].DefaultCellStyle.BackColor = System.Drawing.Color.White; // Reset to original background
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableauClient_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Dashboard dashboard = new Dashboard();
            dashboard.Show();
            this.Hide(); 
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Employe employe = new Employe();
            employe.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide(); 
            var home = new home(); 
            home.Show(); 
            this.Close(); 
        }


        private void button5_Click(object sender, EventArgs e)
        {
            Rooms rom = new Rooms();
            rom.Show();
            this.Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Reservation res = new Reservation();
            res.Show();
            this.Hide();
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void ErrorPhone_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void ErrorNom_Click(object sender, EventArgs e)
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
