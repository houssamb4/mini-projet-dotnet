using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System;
using Partie_Font_Office.Models;
using MimeKit;
using MailKit.Net.Smtp;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting.Server;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp;
using System.IO; 


[ApiController]
[Route("api/reservation")]
public class ReservationController : Controller
{
    private readonly DatabaseHelper dbHelper;
    private readonly IConfiguration configuration;


    public ReservationController(IConfiguration configuration)
    {
        dbHelper = new DatabaseHelper();
        this.configuration = configuration;
    }

    [HttpPost]
    public IActionResult Reserve([FromBody] ReservationRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.FullName) || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new { success = false, message = "Invalid request. Full Name and Email are required." });
        }

        if (request.CheckOutDate <= request.CheckInDate)
        {
            return BadRequest(new { success = false, message = "Check-out date must be later than the check-in date." });
        }

        try
        {
            using (var connection = dbHelper.GetConnection())
            {
                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {
                    bool transactionCommitted = false;

                    try
                    {
                        int clientId = GetOrInsertClient(request, connection, transaction);
                        decimal totalAmount = InsertReservation(request, clientId, connection, transaction);

                        transaction.Commit();
                        transactionCommitted = true;

                        SendConfirmationEmail(request, totalAmount);

                        return Ok(new { success = true, message = "Reservation successful." });
                    }
                    catch (Exception ex)
                    {
                        if (!transactionCommitted)
                        {
                            transaction.Rollback();
                        }

                        LogError(ex);
                        return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
            return StatusCode(500, new { success = false, message = "An internal server error occurred." });
        }
    }

    private int GetOrInsertClient(ReservationRequest request, SqlConnection connection, SqlTransaction transaction)
    {
        const string checkClientQuery = "SELECT id FROM Client WHERE email = @Email";
        using (var checkCmd = new SqlCommand(checkClientQuery, connection, transaction))
        {
            checkCmd.Parameters.AddWithValue("@Email", request.Email);
            var result = checkCmd.ExecuteScalar();
            if (result != null)
            {
                return (int)result;
            }
        }

        const string insertClientQuery = @"
            INSERT INTO Client (nom_complet, email, telephone, Adresse, created_at)
            OUTPUT INSERTED.id
            VALUES (@FullName, @Email, @Phone, @Address, GETDATE())";

        using (var clientCmd = new SqlCommand(insertClientQuery, connection, transaction))
        {
            clientCmd.Parameters.AddWithValue("@FullName", request.FullName);
            clientCmd.Parameters.AddWithValue("@Email", request.Email);
            clientCmd.Parameters.AddWithValue("@Phone", request.Phone ?? (object)DBNull.Value);
            clientCmd.Parameters.AddWithValue("@Address", request.Address ?? (object)DBNull.Value);

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
        int numberOfNights = (int)duration.TotalDays;
        decimal totalAmount = numberOfNights * roomPrice;

        const string insertReservationQuery = @"
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

            reservationCmd.ExecuteNonQuery();
        }

        return totalAmount;
    }

    private void SendConfirmationEmail(ReservationRequest request, decimal totalAmount)
    {
        try
        {
            string roomDescription = request.RoomType switch
            {
                1 => "Single Room - $99.00 per night",
                2 => "Double Room - $199.00 per night",
                3 => "Lux Room - $299.00 per night",
                4 => "VIP Room - $399.00 per night",
                _ => "Unknown Room Type"
            };

            var pdfBytes = GenerateReservationPDF(request, totalAmount);

            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Golden Horizon Hotel", "golden_horizon@support.com"));
            email.To.Add(new MailboxAddress(request.FullName, request.Email));
            email.Subject = "Reservation Confirmation";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                <p>Dear {request.FullName},</p>
                <p>Thank you for choosing Golden Horizon Hotel.</p>
                <p>Attached, you will find the confirmation of your reservation in PDF format.</p>
                <p>Best Regards,<br>Golden Horizon Hotel</p>"
            };

            bodyBuilder.Attachments.Add("ReservationConfirmation.pdf", pdfBytes, new ContentType("application", "pdf"));
            email.Body = bodyBuilder.ToMessageBody();

            using (var smtpClient = new SmtpClient())
            {
                var smtpHost = configuration["Smtp:Host"];
                var smtpPort = int.Parse(configuration["Smtp:Port"]);
                var smtpUser = configuration["Smtp:Username"];
                var smtpPass = configuration["Smtp:Password"];

                smtpClient.Connect(smtpHost, smtpPort, false);
                smtpClient.Authenticate(smtpUser, smtpPass);
                smtpClient.Send(email);
                smtpClient.Disconnect(true);
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw; 
        }
    }

    private byte[] GenerateReservationPDF(ReservationRequest request, decimal totalAmount)
    {
        try
        {
            using (var document = new PdfDocument())
            {
                var page = document.AddPage();
                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    var titleFont = new XFont("Arial", 18);
                    var headerFont = new XFont("Arial", 14);
                    var bodyFont = new XFont("Arial", 12);
                    var footerFont = new XFont("Arial", 10);

                    var logoPath = @"C:\Users\houss\source\repos\houssamb4\mini-projet-dotnet\Partie-Font-Office\Partie-Font-Office\Resources\logo.png";

                    if (System.IO.File.Exists(logoPath)) 
                    {
                        using (var img = XImage.FromFile(logoPath))
                        {
                            gfx.DrawImage(img, 40, 20, 100, 50);  
                        }
                    }

                    gfx.DrawString("Reservation Confirmation", titleFont, XBrushes.Black,
                        new XRect(0, 80, page.Width, 40), XStringFormats.TopCenter);

                    var margin = 40;
                    var boxWidth = page.Width - (margin * 2);
                    var boxHeight = 300;
                    var boxY = 120;
                    gfx.DrawRectangle(XPens.LightGray, margin, boxY, boxWidth, boxHeight);

                    gfx.DrawString("Guest Information", headerFont, XBrushes.Black,
                        new XPoint(margin + 10, boxY + 20));
                    gfx.DrawString($"Name: {request.FullName}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 40));
                    gfx.DrawString($"Email: {request.Email}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 60));
                    gfx.DrawString($"Phone: {request.Phone ?? "N/A"}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 80));
                    gfx.DrawString($"Address: {request.Address ?? "N/A"}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 100));

                    gfx.DrawString("Reservation Details", headerFont, XBrushes.Black,
                        new XPoint(margin + 10, boxY + 140));
                    gfx.DrawString($"Room Type: {GetRoomDescription(request.RoomType)}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 160));
                    gfx.DrawString($"Check-In Date: {request.CheckInDate:yyyy-MM-dd}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 180));
                    gfx.DrawString($"Check-Out Date: {request.CheckOutDate:yyyy-MM-dd}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 200));
                    gfx.DrawString($"Number of Guests: {request.Guests}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 220));
                    gfx.DrawString($"Total Amount: ${totalAmount:0.00}", bodyFont, XBrushes.Black,
                        new XPoint(margin + 20, boxY + 240));

                    gfx.DrawString("Thank you for choosing Golden Horizon Hotel!", footerFont, XBrushes.Gray,
                        new XRect(0, page.Height - 60, page.Width, 20), XStringFormats.Center);
                    gfx.DrawString("For inquiries, contact us at support@goldenhorizon.com.", footerFont, XBrushes.Gray,
                        new XRect(0, page.Height - 40, page.Width, 20), XStringFormats.Center);
                }

                using (var memoryStream = new MemoryStream())
                {
                    document.Save(memoryStream, false);
                    return memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            LogError(ex);
            throw new InvalidOperationException("Failed to generate the reservation PDF.", ex);
        }
    }



    private string GetRoomDescription(int roomType)
    {
        return roomType switch
        {
            1 => "Single Room - $99.00 per night",
            2 => "Double Room - $199.00 per night",
            3 => "Lux Room - $299.00 per night",
            4 => "VIP Room - $399.00 per night",
            _ => "Unknown Room Type"
        };
    }


    private void LogError(Exception ex)
    {
        Console.Error.WriteLine($"[{DateTime.Now}] Error: {ex.Message}");
        Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        if (ex.InnerException != null)
        {
            Console.Error.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        }
    }
}

