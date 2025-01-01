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
    public partial class Reset : Form
    {
        private int userId;

        public Reset(int loggedInUserId)
        {
            InitializeComponent();
            userId = loggedInUserId;
            label7.Text = Login.LoggedInUserFullName;

        }

        Database bd = new Database();



        private void Reset_load(object sender, EventArgs e)
        {
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text.Trim();
            string confirmPassword = textBoxConfirmPassword.Text.Trim();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Password cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = bd.getConnexion)
                {
                    bd.OpenConnexion();

                    string updateQuery = "UPDATE Utilisateur SET password = @password WHERE id = @userId";
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@password", newPassword);
                        command.Parameters.AddWithValue("@userId", userId);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password reset successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Account acc = new Account();
                            acc.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Failed to reset password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            string newPassword = textBoxNewPassword.Text.Trim();
            string confirmPassword = textBoxConfirmPassword.Text.Trim();

            if (newPassword != confirmPassword)
            {
                label3.Text = "Passwords do not match";
                label3.ForeColor = Color.Red;
            }
            else
            {
                label3.Text = "Passwords match";
                label3.ForeColor = Color.Green;
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


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void releaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wMsg, int wParam, int lParam);



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
            Employe Emp = new Employe();
            Emp.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Rooms rom = new Rooms();
            rom.Show();
            this.Hide();
        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
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

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account acc = new Account();
            acc.Show();
            this.Hide();
        }


        private void panel1_MouseDown_2(object sender, MouseEventArgs e)
        {
            releaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
    }
}

