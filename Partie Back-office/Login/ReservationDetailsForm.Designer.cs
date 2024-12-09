namespace Login
{
    partial class ReservationDetailsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelClientName = new System.Windows.Forms.Label();
            this.labelRoomType = new System.Windows.Forms.Label();
            this.labelCheckInDate = new System.Windows.Forms.Label();
            this.labelCheckOutDate = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelTotalAmount = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelClientName
            // 
            this.labelClientName.AutoSize = true;
            this.labelClientName.Location = new System.Drawing.Point(30, 30);
            this.labelClientName.Name = "labelClientName";
            this.labelClientName.Size = new System.Drawing.Size(110, 17);
            this.labelClientName.TabIndex = 0;
            this.labelClientName.Text = "Client Name: ";
            // 
            // labelRoomType
            // 
            this.labelRoomType.AutoSize = true;
            this.labelRoomType.Location = new System.Drawing.Point(30, 70);
            this.labelRoomType.Name = "labelRoomType";
            this.labelRoomType.Size = new System.Drawing.Size(94, 17);
            this.labelRoomType.TabIndex = 1;
            this.labelRoomType.Text = "Room Type: ";
            // 
            // labelCheckInDate
            // 
            this.labelCheckInDate.AutoSize = true;
            this.labelCheckInDate.Location = new System.Drawing.Point(30, 110);
            this.labelCheckInDate.Name = "labelCheckInDate";
            this.labelCheckInDate.Size = new System.Drawing.Size(115, 17);
            this.labelCheckInDate.TabIndex = 2;
            this.labelCheckInDate.Text = "Check-in Date: ";
            // 
            // labelCheckOutDate
            // 
            this.labelCheckOutDate.AutoSize = true;
            this.labelCheckOutDate.Location = new System.Drawing.Point(30, 150);
            this.labelCheckOutDate.Name = "labelCheckOutDate";
            this.labelCheckOutDate.Size = new System.Drawing.Size(118, 17);
            this.labelCheckOutDate.TabIndex = 3;
            this.labelCheckOutDate.Text = "Check-out Date: ";
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(30, 190);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(58, 17);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Status: ";
            // 
            // labelTotalAmount
            // 
            this.labelTotalAmount.AutoSize = true;
            this.labelTotalAmount.Location = new System.Drawing.Point(30, 230);
            this.labelTotalAmount.Name = "labelTotalAmount";
            this.labelTotalAmount.Size = new System.Drawing.Size(108, 17);
            this.labelTotalAmount.TabIndex = 5;
            this.labelTotalAmount.Text = "Total Amount: ";
            // 
            // ReservationDetailsForm
            // 
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.Controls.Add(this.labelTotalAmount);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.labelCheckOutDate);
            this.Controls.Add(this.labelCheckInDate);
            this.Controls.Add(this.labelRoomType);
            this.Controls.Add(this.labelClientName);
            this.Name = "ReservationDetailsForm";
            this.Text = "Reservation Details";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        // Declare the labels as fields to be used throughout the form
        private System.Windows.Forms.Label labelClientName;
        private System.Windows.Forms.Label labelRoomType;
        private System.Windows.Forms.Label labelCheckInDate;
        private System.Windows.Forms.Label labelCheckOutDate;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelTotalAmount;
    }
}
