using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using MimeKit;
using MailKit.Net.Smtp;

namespace Login
{
    public partial class ReservationDetailsForm : Form
    {
        Database bd = new Database();
        private string reservationId;

        public ReservationDetailsForm(string reservationId)
        {
            InitializeComponent();
            this.reservationId = reservationId;
            LoadReservationDetails();
        }

        private void LoadReservationDetails()
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
                SELECT 
                    r.id AS [Reservation ID],
                    r.client_id AS [Client ID],
                    c.email AS [Client Email],
                    c.created_at AS [Created on],
                    c.nom_complet AS [Nom du client],
                    t.type_name AS [Type Chambre],
                    r.date_arrivee AS [Date Arrivée],
                    r.date_depart AS [Date Départ],
                    r.status AS [Status],
                    r.created_by AS [created by],
                    r.CreatedAt AS [Date of Creation],
                    r.TotalAmount AS [Total Amount] -- Assuming you have a TotalAmount field
                FROM [dbo].[reservation] r
                INNER JOIN [dbo].[Client] c ON r.client_id = c.id
                INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id
                WHERE r.id = @reservationId";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        label1.Text = "Client ID: "+ reader["Client ID"].ToString();
                        label2.Text = "Client Email: " + reader["Client Email"].ToString();
                        label3.Text = "Created On: " + reader["Created on"].ToString();
                        label6.Text = "Reserved on: " + reader["Date of Creation"].ToString();
                        label7.Text = "Reserved By: " + reader["created by"].ToString();
                        labelReservationId.Text = "Reservation ID: " + reader["Reservation ID"].ToString();
                        labelClientName.Text = "Client Name: " + reader["Nom du client"].ToString();
                        labelRoomType.Text = "Room Type: " + reader["Type Chambre"].ToString();
                        labelCheckInDate.Text = "Check-in Date: " + Convert.ToDateTime(reader["Date Arrivée"]).ToString("yyyy-MM-dd");
                        labelCheckOutDate.Text = "Check-out Date: " + Convert.ToDateTime(reader["Date Départ"]).ToString("yyyy-MM-dd");
                        labelStatus.Text = "Status: " + reader["Status"].ToString();
                        labelTotalAmount.Text = "Total Amount: $" + reader["Total Amount"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("No reservation found with the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading reservation details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(this.reservationId))
                {
                    MessageBox.Show("Reservation ID is not set.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                UpdateReservationStatus(this.reservationId, "Annulé");

                MessageBox.Show("Reservation status updated to 'Annulé'.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update reservation: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateReservationStatus(string reservationId, string status)
        {
            bd.OpenConnexion();

            using (SqlConnection connection = bd.getConnexion)
            {
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    connection.Open();
                }

                string query = "UPDATE reservation SET status = @Status WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@Status", System.Data.SqlDbType.NVarChar) { Value = status });
                    cmd.Parameters.Add(new SqlParameter("@Id", System.Data.SqlDbType.NVarChar) { Value = reservationId });

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new InvalidOperationException("No reservation found with the specified ID.");
                    }
                }
            }
        }


        private void ReservationDetailsForm_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelClientName_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            EditReservationForm edit = new EditReservationForm(reservationId);
            edit.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bd.OpenConnexion();
                string query = @"
            SELECT 
                r.id AS [Reservation ID],
                c.nom_complet AS [Client Name],
                c.email AS [Client Email],
                r.date_arrivee AS [Check-in Date],
                r.date_depart AS [Check-out Date],
                r.TotalAmount AS [Total Amount],
                r.chambre_type_id,
                t.description AS [Room Type],
                t.prix_par_nuit AS [Price night]
            FROM [dbo].[reservation] r
            INNER JOIN [dbo].[Client] c ON r.client_id = c.id
            INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id
            WHERE r.id = @reservationId";

                string clientName = string.Empty;
                string clientEmail = string.Empty;
                string checkInDate = string.Empty;
                string checkOutDate = string.Empty;
                string reservationId = string.Empty;
                decimal pricepnight = 0;
                decimal totalAmount = 0;
                string roomtype = string.Empty;

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", this.reservationId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            clientName = reader["Client Name"].ToString();
                            clientEmail = reader["Client Email"].ToString();
                            checkInDate = Convert.ToDateTime(reader["Check-in Date"]).ToString("yyyy-MM-dd");
                            checkOutDate = Convert.ToDateTime(reader["Check-out Date"]).ToString("yyyy-MM-dd");
                            reservationId = reader["Reservation ID"].ToString();
                            roomtype = reader["Room Type"].ToString();
                            pricepnight = Convert.ToDecimal(reader["Price night"]);
                            totalAmount = Convert.ToDecimal(reader["Total Amount"]);
                        }
                        else
                        {
                            MessageBox.Show("No reservation found with the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Golden Horizon", "goldenhorizon@gmail.com"));
                message.To.Add(new MailboxAddress(clientName, clientEmail));
                message.Subject = "Reservation Confirmation";

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = $@"
                <html>
                <head>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            line-height: 1.6;
                            background-color: #f9f9f9;
                            margin: 0;
                            padding: 0;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background: #fff;
                            border-radius: 8px;
                            padding: 20px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }}
                        .header {{
                            text-align: center;
                            color: #007BFF;
                        }}
                        .header h1 {{
                            margin: 0;
                        }}
                        .content ul {{
                            list-style: none;
                            padding: 0;
                        }}
                        .content ul li {{
                            margin: 8px 0;
                        }}
                        .footer {{
                            text-align: center;
                            margin-top: 20px;
                            color: #777;
                            font-size: 12px;
                        }}
                    </style>
                </head>
                <body>
                    <div class='email-container'>
                        <div class='header'>
                            <h1>Reservation Confirmation</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {clientName},</p>
                            <p>Your reservation has been successfully confirmed. Below are the details:</p>
                            <ul>
                                <li><strong>Reservation ID:</strong> {reservationId}</li>
                                <li><strong>Room Type:</strong> {roomtype}</li>
                                <li><strong>Check-in Date:</strong> {checkInDate}</li>
                                <li><strong>Check-out Date:</strong> {checkOutDate}</li>
                                <li><strong>Price per Night:</strong> ${pricepnight:F2}</li>
                                <li><strong>Total Amount:</strong> ${totalAmount:F2}</li>
                            </ul>
                            <p>Thank you for choosing us!</p>
                        </div>
                        <div class='footer'>
                            &copy; {DateTime.Now.Year} Golden Horizon. All rights reserved.
                        </div>
                    </div>
                </body>
                </html>",
                    TextBody = $@"
                Dear {clientName},

                Your reservation has been successfully confirmed.

                Details:
                - Reservation ID: {reservationId}
                - Room Type: {roomtype}
                - Check-in Date: {checkInDate}
                - Check-out Date: {checkOutDate}
                - Price per Night: ${pricepnight:F2}
                - Total Amount: ${totalAmount:F2}

                Thank you for choosing us!"
                };

                message.Body = bodyBuilder.ToMessageBody();

                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect("smtp.gmail.com", 587, false);
                    smtpClient.Authenticate("testphpmailer64@gmail.com", "mmom zfap shgg utbl");
                    smtpClient.Send(message);
                    smtpClient.Disconnect(true);
                }

                MessageBox.Show("Email sent successfully to the client!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }




        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                bd.OpenConnexion();

                string query = @"
        SELECT 
            r.id AS [Reservation ID],
            c.nom_complet AS [Client Name],
            c.email AS [Client Email],
            c.telephone AS [Client Phone],
            t.type_name AS [Room Type],
            r.date_arrivee AS [Check-in Date],
            r.date_depart AS [Check-out Date],
            r.TotalAmount AS [Total Amount],
            r.status AS [Status]
        FROM [dbo].[reservation] r
        INNER JOIN [dbo].[Client] c ON r.client_id = c.id
        INNER JOIN [dbo].[type_chambre] t ON r.chambre_type_id = t.id
        WHERE r.id = @reservationId";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                        string outputPath = Path.Combine(downloadsPath, $"Reservation_{reservationId}.pdf");

                        Document document = new Document(PageSize.A4, 36, 36, 72, 72);
                        PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));

                        writer.PageEvent = new PdfPageEventHelperWithHeaderFooter();

                        document.Open();

                        string logoPath = @"C:\Users\houss\Downloads\logo.png"; 
                        if (File.Exists(logoPath))
                        {
                            Image logo = Image.GetInstance(logoPath);
                            logo.ScaleToFit(100f, 100f);
                            logo.Alignment = Image.ALIGN_CENTER;
                            document.Add(logo);
                        }

                        Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
                        Paragraph title = new Paragraph("Reservation Details", titleFont)
                        {
                            Alignment = Element.ALIGN_CENTER,
                            SpacingAfter = 20
                        };
                        document.Add(title);

                        // Add Table
                        PdfPTable table = new PdfPTable(2) { WidthPercentage = 100 };
                        table.SetWidths(new float[] { 1, 2 });

                        Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, BaseColor.WHITE);
                        Font cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 11);

                        PdfPCell headerCell1 = new PdfPCell(new Phrase("Field", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY };
                        PdfPCell headerCell2 = new PdfPCell(new Phrase("Value", headerFont)) { BackgroundColor = BaseColor.DARK_GRAY };

                        table.AddCell(headerCell1);
                        table.AddCell(headerCell2);

                        AddTableRow(table, "Reservation ID", reader["Reservation ID"].ToString(), cellFont);
                        AddTableRow(table, "Client Name", reader["Client Name"].ToString(), cellFont);
                        AddTableRow(table, "Client Email", reader["Client Email"].ToString(), cellFont);
                        AddTableRow(table, "Client Phone", reader["Client Phone"].ToString(), cellFont);
                        AddTableRow(table, "Room Type", reader["Room Type"].ToString(), cellFont);
                        AddTableRow(table, "Check-in Date", Convert.ToDateTime(reader["Check-in Date"]).ToString("yyyy-MM-dd"), cellFont);
                        AddTableRow(table, "Check-out Date", Convert.ToDateTime(reader["Check-out Date"]).ToString("yyyy-MM-dd"), cellFont);

                        AddTableRow(table, "Total Amount", $"${reader["Total Amount"]}", cellFont, BaseColor.LIGHT_GRAY);

                        AddTableRow(table, "Status", reader["Status"].ToString(), cellFont, reader["Status"].ToString() == "Paid" ? BaseColor.GREEN : BaseColor.RED);

                        document.Add(table);

                        Paragraph footer = new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", cellFont)
                        {
                            Alignment = Element.ALIGN_RIGHT,
                            SpacingBefore = 20
                        };
                        document.Add(footer);

                        document.Close();
                        writer.Close();

                        MessageBox.Show($"PDF successfully generated: {outputPath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No reservation found for the provided ID.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                bd.OpenConnexion();

                string query = "DELETE FROM reservation WHERE id = @reservationId;";

                using (SqlCommand cmd = new SqlCommand(query, bd.getConnexion))
                {
                    cmd.Parameters.AddWithValue("@reservationId", reservationId);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Reservation deleted successfully.");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No reservation found with the provided ID.");
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Please enter a valid numeric Reservation ID.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                bd.CloseConnexion();
            }
        }


        private void AddTableRow(PdfPTable table, string field, string value, Font font, BaseColor backgroundColor = null)
        {
            PdfPCell fieldCell = new PdfPCell(new Phrase(field, font));
            PdfPCell valueCell = new PdfPCell(new Phrase(value, font));

            if (backgroundColor != null)
            {
                fieldCell.BackgroundColor = backgroundColor;
                valueCell.BackgroundColor = backgroundColor;
            }

            table.AddCell(fieldCell);
            table.AddCell(valueCell);
        }

        public class PdfPageEventHelperWithHeaderFooter : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footer = new PdfPTable(1) { TotalWidth = 500f };
                footer.AddCell(new PdfPCell(new Phrase("Golden Horizon | www.goldenhorizon.com", FontFactory.GetFont(FontFactory.HELVETICA, 8)))
                {
                    Border = Rectangle.NO_BORDER,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });

                footer.WriteSelectedRows(0, -1, 36, 30, writer.DirectContent);
            }
        }



        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
