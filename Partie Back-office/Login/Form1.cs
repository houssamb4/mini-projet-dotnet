using System;
using System.Windows.Forms;
using Login;
using System.Drawing;

namespace Login
{
    public partial class Form1 : Form
    {
        private readonly Color PRIMARY_COLOR = Color.FromArgb(45, 52, 54);
        private readonly Color ACCENT_COLOR = Color.FromArgb(85, 239, 196);
        private readonly Color TEXT_COLOR = Color.FromArgb(223, 230, 233);

        public Form1()
        {
            InitializeComponent();
            this.AcceptButton = btnLogin; 
            InitializeStyles();
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
                if (!CheckDatabaseConnection())
                {
                    MessageBox.Show("La connexion à la base de données a échoué. Veuillez vérifier votre configuration réseau ou le serveur de base de données.",
                                    "Erreur de Connexion",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return; 
                }

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
                        MessageBox.Show("Identifiant ou mot de passe incorrect",
                                        "Erreur de Login",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtMdp_Enter(object sender, EventArgs e)
        {
            txtMdp.BackColor = System.Drawing.Color.White; 
            txtMdp.ForeColor = System.Drawing.Color.Black; 
            txtMdp.BorderStyle = BorderStyle.Fixed3D; 
        }

        private void txtMdp_Leave(object sender, EventArgs e)
        {
            txtMdp.BackColor = System.Drawing.Color.FromArgb(240, 240, 240); 
            txtMdp.ForeColor = System.Drawing.Color.Black;
            txtMdp.BorderStyle = BorderStyle.FixedSingle; 
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

        private void InitializeStyles()
        {
            this.BackColor = PRIMARY_COLOR;
            
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.BackColor = ACCENT_COLOR;
            btnLogin.ForeColor = PRIMARY_COLOR;
            btnLogin.Font = new Font("Segoe UI", 12f, FontStyle.Bold);
            btnLogin.Cursor = Cursors.Hand;
            
            foreach(Control c in this.Controls)
            {
                if(c is TextBox)
                {
                    TextBox textBox = (TextBox)c;
                    textBox.BorderStyle = BorderStyle.None;
                    textBox.BackColor = Color.FromArgb(240, 240, 240);
                    textBox.Font = new Font("Segoe UI", 11f);
                    textBox.Padding = new Padding(5);
                }
            }

            foreach(Control c in this.Controls)
            {
                if(c is Label)
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
    }
}
