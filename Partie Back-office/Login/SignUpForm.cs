using System;
using System.Windows.Forms;
using Login;

namespace Login
{
    public partial class SignUpForm : Form
    {
        public SignUpForm()
        {
            InitializeComponent();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            string nom = txtNom.Text;
            string prenom = txtPrenom.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string role = txtRole.Text;

            SignUp signUp = new SignUp();

            if (signUp.RegisterUser(nom, prenom, email, password, role))
            {
                MessageBox.Show("Account created successfully!", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Email already exists or registration failed. Please try again.", "Sign Up", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
