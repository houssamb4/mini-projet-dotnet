using System;
using System.Windows.Forms;

namespace Login
{
    public partial class PaymentForm : Form
    {
        // Properties to hold the payment data
        public DateTime PaymentDate { get; private set; }
        public string PaymentMethod { get; private set; }
        public bool IsPaymentReceived { get; private set; }

        public PaymentForm()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                PaymentDate = dateTimePicker3.Value; 
                PaymentMethod = cmbPaymentStatus.SelectedItem?.ToString(); 

                if (string.IsNullOrEmpty(PaymentMethod))
                {
                    MessageBox.Show("Please select a payment method.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IsPaymentReceived = true; 

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                label2.Text = "You need to agree to the Payment Statut.";
                label2.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }
    }
}
