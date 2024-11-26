using System;
using System.Windows.Forms;
using Login;

namespace Login
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        Login lg = new Login();
        Database bd = new Database();

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (IsFormValid())
                {
                    if (lg.VerificationDesChamps(txtLogIn.Text, txtMdp.Text))
                    {
                        Dashboard dsh = new Dashboard();
                        this.Hide();
                        dsh.Show();
                    }
                    else
                    {
                        MessageBox.Show("Identifiant ou mot de passe Incorrect", "Log In", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Log In", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool IsFormValid()
        {
            if (txtLogIn.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Le Login est requis!", "Erreur de Login");
                return false;
            }
            if (txtMdp.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Le Mot de pass est requis!", "Erreur de Password");
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

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form2 form = new Form2();
            form.Show();
            this.Hide();
        }

        private void txtLogIn_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
