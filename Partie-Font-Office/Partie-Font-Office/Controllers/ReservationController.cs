using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Partie_Font_Office.Helpers;
using System.Reflection.Metadata;
using System;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Pdf.Canvas;
using Partie_Font_Office.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Data;
using iText.Commons.Utils;
using static System.Net.Mime.MediaTypeNames;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Cryptography.Xml;
using iText.IO.Font;
using iText.Kernel.Font;
using System.IO;  


[ApiController]
[Route("api/reservation")]
public class ReservationController : Controller
{
    private readonly DatabaseHelper dbHelper;
    private readonly PdfGenerator pdfGenerator;

    public ReservationController()
    {
        dbHelper = new DatabaseHelper();
        pdfGenerator = new PdfGenerator();
    }

    [HttpPost]
    public IActionResult Reserve([FromBody] ReservationRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { success = false, message = "Request cannot be null." });
        }

        if (string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { success = false, message = "Full Name and Email are required fields." });
        }

        try
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        int clientId = GetOrInsertClient(request, connection, transaction);

                        decimal totalAmount = InsertReservation(request, clientId, connection, transaction);

                        transaction.Commit();

                        SendConfirmationEmail(request, totalAmount);

                        return Ok(new { success = true, message = "Reservation successful." });
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return StatusCode(500, new { success = false, message = $"Error during transaction: {ex.Message}" });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, message = $"Server error: {ex.Message}" });
        }
    }

    private int GetOrInsertClient(ReservationRequest request, SqlConnection connection, SqlTransaction transaction)
    {
        string checkClientQuery = "SELECT id FROM Client WHERE email = @Email";
        using (var checkCmd = new SqlCommand(checkClientQuery, connection, transaction))
        {
            checkCmd.Parameters.AddWithValue("@Email", request.Email);

            var result = checkCmd.ExecuteScalar();
            if (result != null)
            {
                return (int)result;
            }
        }

        string insertClientQuery = @"
            INSERT INTO Client (nom_complet, email, telephone, Adresse, created_at)
            OUTPUT INSERTED.id
            VALUES (@FullName, @Email, @Phone, @Address, GETDATE())";

        using (var clientCmd = new SqlCommand(insertClientQuery, connection, transaction))
        {
            clientCmd.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = request.FullName;
            clientCmd.Parameters.Add("@Email", SqlDbType.NVarChar).Value = request.Email;
            clientCmd.Parameters.Add("@Phone", SqlDbType.NVarChar).Value = request.Phone ?? (object)DBNull.Value;
            clientCmd.Parameters.Add("@Address", SqlDbType.NVarChar).Value = request.Address ?? (object)DBNull.Value;

            return (int)clientCmd.ExecuteScalar();
        }
    }

    private decimal InsertReservation(ReservationRequest request, int clientId, SqlConnection connection, SqlTransaction transaction)
    {
        decimal roomPrice = request.RoomType switch
        {
            1 => 99.00m,
            2 => 199.00m,
            3 => 299.00m,
            4 => 399.00m,
            _ => throw new ArgumentException("Invalid Room Type")
        };

        TimeSpan duration = request.CheckOutDate - request.CheckInDate;
        if (duration.TotalDays <= 0)
        {
            throw new Exception("Check-out date must be later than the check-in date.");
        }

        int numberOfNights = (int)duration.TotalDays;

        decimal totalAmount = numberOfNights * roomPrice;

        string insertReservationQuery = @"
        INSERT INTO reservation (client_id, chambre_type_id, date_arrivee, date_depart, status, created_by, TotalAmount, guests)
        VALUES (@ClientId, @RoomType, @CheckInDate, @CheckOutDate, 'Pending', 'client', @TotalAmount, @Guests)";

        using (var reservationCmd = new SqlCommand(insertReservationQuery, connection, transaction))
        {
            reservationCmd.Parameters.AddWithValue("@ClientId", clientId);
            reservationCmd.Parameters.AddWithValue("@RoomType", request.RoomType);
            reservationCmd.Parameters.AddWithValue("@CheckInDate", request.CheckInDate);
            reservationCmd.Parameters.AddWithValue("@CheckOutDate", request.CheckOutDate);
            reservationCmd.Parameters.AddWithValue("@Guests", request.Guests);
            reservationCmd.Parameters.AddWithValue("@TotalAmount", totalAmount);

            int rowsAffected = reservationCmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception("No rows inserted into the Reservations table.");
            }
        }

        return totalAmount;
    }

    private void SendConfirmationEmail(ReservationRequest request, decimal totalAmount)
    {
        string roomDescription = request.RoomType switch
        {
            1 => "Single Room - $99.00 per night",
            2 => "Double Room - $199.00 per night",
            3 => "Lux Room - $299.00 per night",
            4 => "VIP Room - $399.00 per night",
            _ => "Unknown Room Type"
        };

        var pdfFilePath = GenerateReservationPdf(request, roomDescription, totalAmount);

        var email = new MimeMessage();
        email.From.Add(new MailboxAddress("Golden Horizon Hotel", "golden_horizon@support.com"));
        email.To.Add(new MailboxAddress(request.FullName, request.Email));
        email.Subject = "Reservation Confirmation";

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = $@"
    <html>
    <body style='font-family: Arial, sans-serif; background-color: #f9f9f9; margin: 0; padding: 0;'>
        <div style='max-width: 600px; margin: 20px auto; background-color: #ffffff; padding: 20px; border: 1px solid #dddddd; border-radius: 5px;'>
            <h2 style='color: #333333; text-align: center;'>Reservation Confirmation</h2>
            <p style='font-size: 16px; color: #555555;'>Hello <strong>{request.FullName}</strong>,</p>
            <p style='font-size: 16px; color: #555555;'>Thank you for making a reservation with us! Here are your reservation details:</p>
            <table style='width: 100%; border-collapse: collapse; margin: 20px 0;'>
                <tr>
                    <td style='padding: 10px; border: 1px solid #dddddd; background-color: #f4f4f4;'>Check-in Date:</td>
                    <td style='padding: 10px; border: 1px solid #dddddd;'>{request.CheckInDate:MMMM dd, yyyy}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border: 1px solid #dddddd; background-color: #f4f4f4;'>Check-out Date:</td>
                    <td style='padding: 10px; border: 1px solid #dddddd;'>{request.CheckOutDate:MMMM dd, yyyy}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border: 1px solid #dddddd; background-color: #f4f4f4;'>Room Type:</td>
                    <td style='padding: 10px; border: 1px solid #dddddd;'>{roomDescription}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border: 1px solid #dddddd; background-color: #f4f4f4;'>Guests:</td>
                    <td style='padding: 10px; border: 1px solid #dddddd;'>{request.Guests}</td>
                </tr>
                <tr>
                    <td style='padding: 10px; border: 1px solid #dddddd; background-color: #f4f4f4;'>Total Amount:</td>
                    <td style='padding: 10px; border: 1px solid #dddddd;'>${totalAmount:F2}</td>
                </tr>
            </table>
            <p style='font-size: 16px; color: #555555;'>We look forward to hosting you!</p>
            <p style='font-size: 16px; color: #555555;'>Best regards,</p>
            <p style='font-size: 16px; color: #555555;'><strong>The Golden Horizon Hotel Team</strong></p>
        </div>
    </body>
    </html>"
        };

        bodyBuilder.Attachments.Add(pdfFilePath);

        email.Body = bodyBuilder.ToMessageBody();

        using (var smtpClient = new SmtpClient())
        {
            smtpClient.Connect("smtp.gmail.com", 587, false);
            smtpClient.Authenticate("testphpmailer64@gmail.com", "mmom zfap shgg utbl");
            smtpClient.Send(email);
            smtpClient.Disconnect(true);
        }

        // Clean up the PDF file after sending
        if (System.IO.File.Exists(pdfFilePath))
        {
            System.IO.File.Delete(pdfFilePath);
        }

    }

    private string GenerateReservationPdf(ReservationRequest request, string roomDescription, decimal totalAmount)
    {
        string filePath = Path.Combine(Path.GetTempPath(), $"Reservation_{Guid.NewGuid()}.pdf");

        using (var pdfWriter = new PdfWriter(filePath))
        {
            using (var pdfDoc = new PdfDocument(pdfWriter))
            {
                iText.Layout.Document document = new iText.Layout.Document(pdfDoc);

                var font = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                document.Add(new Paragraph(new iText.Layout.Element.Text("Reservation Confirmation")
                                    .SetFontSize(20))
                                    .SetTextAlignment(TextAlignment.CENTER));


                document.Add(new Paragraph($"Full Name: {request.FullName}"));
                document.Add(new Paragraph($"Email: {request.Email}"));
                document.Add(new Paragraph($"Check-in Date: {request.CheckInDate:MMMM dd, yyyy}"));
                document.Add(new Paragraph($"Check-out Date: {request.CheckOutDate:MMMM dd, yyyy}"));
                document.Add(new Paragraph($"Room Type: {roomDescription}"));
                document.Add(new Paragraph($"Guests: {request.Guests}"));
                document.Add(new Paragraph($"Total Amount: ${totalAmount:F2}"));

                document.Close();
            }
        }
        return filePath;
    }

}
