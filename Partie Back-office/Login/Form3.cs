using System;
using System.Windows.Forms;
using Login;
using System.Drawing;
using MimeKit;
using MailKit.Net.Smtp;
using System.Data.SqlClient;


namespace Login
{
    public partial class Form3 : Form
    {
        private readonly Color PRIMARY_COLOR = Color.FromArgb(45, 52, 54);
        private readonly Color ACCENT_COLOR = Color.FromArgb(85, 239, 196);
        private readonly Color TEXT_COLOR = Color.FromArgb(223, 230, 233);

        public Form3()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin;
            InitializeStyles();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Database bd = new Database();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            string email = txtLogIn.Text.Trim();

            string password = GetPasswordByEmail(email);

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Email not found in our records.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SendPasswordEmail(email, password);
                MessageBox.Show("Password has been sent to your email address.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1 form = new Form1();
                form.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while sending the email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetPasswordByEmail(string email)
        {

            string query = $"SELECT password FROM Utilisateur WHERE email = '{email}'";
            return bd.ExecuteScalar(query)?.ToString(); 
        }


        private void SendPasswordEmail(string recipientEmail, string password)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Golden Horizon", "GoldenHorizon@support.com")); 
            message.To.Add(new MailboxAddress("", recipientEmail));
            message.Subject = "This Is Your Password";

            message.Body = new TextPart("plain")
            {
                Text = $"Hello,\n\nYour password is: {password}\n\nPlease keep it secure.\n\nBest regards,\nYour Admin Golden Horizon Team"
            };

            using (var smtpClient = new MailKit.Net.Smtp.SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls); 
                smtpClient.Authenticate("testphpmailer64@gmail.com", "mmom zfap shgg utbl");

                smtpClient.Send(message);
                smtpClient.Disconnect(true);
            }
        }



        private bool CheckDatabaseConnection()
        {
            return bd.IsConnectionAvailable();
        }



        private bool IsFormValid()
        {
            if (txtLogIn.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Le Login est requis!", "Erreur de Login");
                return false;
            }
            return true;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
            this.Hide();
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void txtLogIn_Enter(object sender, EventArgs e)
        {
            txtLogIn.BackColor = System.Drawing.Color.White;
            txtLogIn.ForeColor = System.Drawing.Color.Black;
            txtLogIn.BorderStyle = BorderStyle.Fixed3D;
        }

        private void txtLogIn_Leave(object sender, EventArgs e)
        {
            txtLogIn.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            txtLogIn.ForeColor = System.Drawing.Color.Black;
            txtLogIn.BorderStyle = BorderStyle.FixedSingle;
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

        private void InitializeStyles()
        {
            this.BackColor = PRIMARY_COLOR;

            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = ACCENT_COLOR;
            btnLogin.ForeColor = PRIMARY_COLOR;
            btnLogin.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;

            foreach (Control c in this.Controls)
            {
                if (c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.BackColor = Color.FromArgb(240, 240, 240);
                    textBox.Font = new Font("Segoe UI", 11f);
                    textBox.Padding = new Padding(5);
                }
            }

            foreach (Control c in this.Controls)
            {
                if (c is Label)
                {
                    Label label = (Label)c;
                    label.ForeColor = TEXT_COLOR;
                    label.Font = new Font("Segoe UI", 10f);
                }
            }
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = Color.FromArgb(129, 236, 236);
            btnLogin.ForeColor = Color.FromArgb(45, 52, 54);
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = ACCENT_COLOR;
            btnLogin.ForeColor = PRIMARY_COLOR;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            this.Hide();
        }
    }
}
