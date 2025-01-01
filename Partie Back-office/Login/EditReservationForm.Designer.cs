namespace Login
{
    partial class EditReservationForm
    {
        private System.ComponentModel.IContainer components = null;

        // Declare UI controls
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.Label titleLabel;

        // Reservation details controls
        private System.Windows.Forms.Label clientNameLabel;
        private System.Windows.Forms.TextBox clientNameTextBox;
        private System.Windows.Forms.Label clientEmailLabel;
        private System.Windows.Forms.TextBox clientEmailTextBox;
        private System.Windows.Forms.Label roomTypeLabel;
        private System.Windows.Forms.ComboBox roomTypeComboBox;
        private System.Windows.Forms.Label checkInDateLabel;
        private System.Windows.Forms.DateTimePicker checkInDatePicker;
        private System.Windows.Forms.Label checkOutDateLabel;
        private System.Windows.Forms.DateTimePicker checkOutDatePicker;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.ComboBox statusComboBox;
        private System.Windows.Forms.Label totalAmountLabel;
        //private System.Windows.Forms.Label labelDialogResult;
        private System.Windows.Forms.TextBox totalAmountTextBox;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditReservationForm));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.clientNameLabel = new System.Windows.Forms.Label();
            this.clientNameTextBox = new System.Windows.Forms.TextBox();
            this.clientEmailLabel = new System.Windows.Forms.Label();
            this.clientEmailTextBox = new System.Windows.Forms.TextBox();
            this.roomTypeLabel = new System.Windows.Forms.Label();
            this.roomTypeComboBox = new System.Windows.Forms.ComboBox();
            this.checkInDateLabel = new System.Windows.Forms.Label();
            this.checkInDatePicker = new System.Windows.Forms.DateTimePicker();
            this.checkOutDateLabel = new System.Windows.Forms.Label();
            this.checkOutDatePicker = new System.Windows.Forms.DateTimePicker();
            this.statusLabel = new System.Windows.Forms.Label();
            this.statusComboBox = new System.Windows.Forms.ComboBox();
            this.totalAmountLabel = new System.Windows.Forms.Label();
            this.totalAmountTextBox = new System.Windows.Forms.TextBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.headerPanel.Controls.Add(this.titleLabel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(600, 60);
            this.headerPanel.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.Color.White;
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(600, 60);
            this.titleLabel.TabIndex = 0;
            this.titleLabel.Text = "Edit Reservation";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // clientNameLabel
            // 
            this.clientNameLabel.AutoSize = true;
            this.clientNameLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clientNameLabel.Location = new System.Drawing.Point(50, 80);
            this.clientNameLabel.Name = "clientNameLabel";
            this.clientNameLabel.Size = new System.Drawing.Size(94, 20);
            this.clientNameLabel.TabIndex = 1;
            this.clientNameLabel.Text = "Client Name:";
            // 
            // clientNameTextBox
            // 
            this.clientNameTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clientNameTextBox.Location = new System.Drawing.Point(200, 77);
            this.clientNameTextBox.Name = "clientNameTextBox";
            this.clientNameTextBox.Size = new System.Drawing.Size(350, 27);
            this.clientNameTextBox.TabIndex = 2;
            // 
            // clientEmailLabel
            // 
            this.clientEmailLabel.AutoSize = true;
            this.clientEmailLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clientEmailLabel.Location = new System.Drawing.Point(50, 120);
            this.clientEmailLabel.Name = "clientEmailLabel";
            this.clientEmailLabel.Size = new System.Drawing.Size(91, 20);
            this.clientEmailLabel.TabIndex = 3;
            this.clientEmailLabel.Text = "Client Email:";
            // 
            // clientEmailTextBox
            // 
            this.clientEmailTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.clientEmailTextBox.Location = new System.Drawing.Point(200, 117);
            this.clientEmailTextBox.Name = "clientEmailTextBox";
            this.clientEmailTextBox.Size = new System.Drawing.Size(350, 27);
            this.clientEmailTextBox.TabIndex = 4;
            // 
            // roomTypeLabel
            // 
            this.roomTypeLabel.AutoSize = true;
            this.roomTypeLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.roomTypeLabel.Location = new System.Drawing.Point(50, 160);
            this.roomTypeLabel.Name = "roomTypeLabel";
            this.roomTypeLabel.Size = new System.Drawing.Size(87, 20);
            this.roomTypeLabel.TabIndex = 5;
            this.roomTypeLabel.Text = "Room Type:";
            // 
            // roomTypeComboBox
            // 
            this.roomTypeComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.roomTypeComboBox.FormattingEnabled = true;
            this.roomTypeComboBox.Location = new System.Drawing.Point(200, 157);
            this.roomTypeComboBox.Name = "roomTypeComboBox";
            this.roomTypeComboBox.Size = new System.Drawing.Size(350, 28);
            this.roomTypeComboBox.TabIndex = 6;
            this.roomTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.ComboBox1_SelectedIndexChanged);
            // 
            // checkInDateLabel
            // 
            this.checkInDateLabel.AutoSize = true;
            this.checkInDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkInDateLabel.Location = new System.Drawing.Point(50, 200);
            this.checkInDateLabel.Name = "checkInDateLabel";
            this.checkInDateLabel.Size = new System.Drawing.Size(105, 20);
            this.checkInDateLabel.TabIndex = 7;
            this.checkInDateLabel.Text = "Check-In Date:";
            // 
            // checkInDatePicker
            // 
            this.checkInDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkInDatePicker.Location = new System.Drawing.Point(200, 197);
            this.checkInDatePicker.Name = "checkInDatePicker";
            this.checkInDatePicker.Size = new System.Drawing.Size(350, 27);
            this.checkInDatePicker.TabIndex = 8;
            // 
            // checkOutDateLabel
            // 
            this.checkOutDateLabel.AutoSize = true;
            this.checkOutDateLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkOutDateLabel.Location = new System.Drawing.Point(50, 240);
            this.checkOutDateLabel.Name = "checkOutDateLabel";
            this.checkOutDateLabel.Size = new System.Drawing.Size(117, 20);
            this.checkOutDateLabel.TabIndex = 9;
            this.checkOutDateLabel.Text = "Check-Out Date:";
            // 
            // checkOutDatePicker
            // 
            this.checkOutDatePicker.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.checkOutDatePicker.Location = new System.Drawing.Point(200, 237);
            this.checkOutDatePicker.Name = "checkOutDatePicker";
            this.checkOutDatePicker.Size = new System.Drawing.Size(350, 27);
            this.checkOutDatePicker.TabIndex = 10;
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusLabel.Location = new System.Drawing.Point(50, 280);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(52, 20);
            this.statusLabel.TabIndex = 11;
            this.statusLabel.Text = "Status:";
            // 
            // statusComboBox
            // 
            this.statusComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.statusComboBox.FormattingEnabled = true;
            this.statusComboBox.Location = new System.Drawing.Point(200, 277);
            this.statusComboBox.Name = "statusComboBox";
            this.statusComboBox.Size = new System.Drawing.Size(350, 28);
            this.statusComboBox.TabIndex = 12;
            // 
            // totalAmountLabel
            // 
            this.totalAmountLabel.AutoSize = true;
            this.totalAmountLabel.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.totalAmountLabel.Location = new System.Drawing.Point(50, 320);
            this.totalAmountLabel.Name = "totalAmountLabel";
            this.totalAmountLabel.Size = new System.Drawing.Size(102, 20);
            this.totalAmountLabel.TabIndex = 13;
            this.totalAmountLabel.Text = "Total Amount:";
            // 
            // totalAmountTextBox
            // 
            this.totalAmountTextBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.totalAmountTextBox.Location = new System.Drawing.Point(200, 317);
            this.totalAmountTextBox.Name = "totalAmountTextBox";
            this.totalAmountTextBox.Size = new System.Drawing.Size(350, 27);
            this.totalAmountTextBox.TabIndex = 14;
            // 
            // saveButton
            // 
            this.saveButton.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.saveButton.ForeColor = System.Drawing.Color.White;
            this.saveButton.Image = ((System.Drawing.Image)(resources.GetObject("saveButton.Image")));
            this.saveButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.saveButton.Location = new System.Drawing.Point(150, 370);
            this.saveButton.Name = "saveButton";
            this.saveButton.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.saveButton.Size = new System.Drawing.Size(130, 50);
            this.saveButton.TabIndex = 15;
            this.saveButton.Text = "Save";
            this.saveButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.saveButton.UseVisualStyleBackColor = false;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.IndianRed;
            this.cancelButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cancelButton.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.cancelButton.ForeColor = System.Drawing.Color.White;
            this.cancelButton.Image = ((System.Drawing.Image)(resources.GetObject("cancelButton.Image")));
            this.cancelButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.cancelButton.Location = new System.Drawing.Point(306, 370);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.cancelButton.Size = new System.Drawing.Size(144, 50);
            this.cancelButton.TabIndex = 16;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EditReservationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(600, 450);
            this.Controls.Add(this.headerPanel);
            this.Controls.Add(this.clientNameLabel);
            this.Controls.Add(this.clientNameTextBox);
            this.Controls.Add(this.clientEmailLabel);
            this.Controls.Add(this.clientEmailTextBox);
            this.Controls.Add(this.roomTypeLabel);
            this.Controls.Add(this.roomTypeComboBox);
            this.Controls.Add(this.checkInDateLabel);
            this.Controls.Add(this.checkInDatePicker);
            this.Controls.Add(this.checkOutDateLabel);
            this.Controls.Add(this.checkOutDatePicker);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.statusComboBox);
            this.Controls.Add(this.totalAmountLabel);
            this.Controls.Add(this.totalAmountTextBox);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "EditReservationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Reservation";
            this.headerPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}
