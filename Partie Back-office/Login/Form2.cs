using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Login
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Login lg = new Login();
        Database bd = new Database();

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            try
            {
                SignUp signUp = new SignUp();

                if (IsFormValid())
                {
                    bool isRegistered = signUp.RegisterUser(
                        textBox2.Text.Trim(), 
                        txtLogIn.Text.Trim(),  
                        txtMdp.Text.Trim()    
                    );

                    if (isRegistered)
                    {
                        var result = MessageBox.Show(
                            "Registration successful! Would you like to log in now?",
                            "Sign Up Successful",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (result == DialogResult.Yes)
                        {
                            this.Hide();
                            Form1 loginForm = new Form1(); 
                            loginForm.Show();
                        }
                        else
                        {
                            MessageBox.Show(
                                "You can log in later from the login page.",
                                "Sign Up",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information
                            );
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Email already exists. Please use a different email.",
                            "Sign Up",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error: " + ex.Message,
                    "Sign Up",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }



        private bool IsFormValid()
        {
            // Check if Full Name is empty
            if (textBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Full Name is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if Email is empty
            if (txtLogIn.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Email is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Validate Email Format
            if (!IsValidEmail(txtLogIn.Text.Trim()))
            {
                MessageBox.Show("Enter a valid email address!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if Password is empty
            if (txtMdp.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Password is required!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Check if Confirm Password matches Password
            if (textBox3.Text.Trim() != txtMdp.Text.Trim())
            {
                MessageBox.Show("Passwords do not match!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void txtLogIn_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
