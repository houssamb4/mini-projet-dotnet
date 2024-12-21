using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.IO;

public class PdfGenerator
{
    public byte[] GenerateReservationPdf(int reservationId, string fullName, DateTime checkInDate, DateTime checkOutDate, string roomDescription, decimal totalAmount, int guests)
    {
        using (var memoryStream = new MemoryStream())
        {
            using (var pdfWriter = new PdfWriter(memoryStream))
            using (var pdfDocument = new PdfDocument(pdfWriter))
            using (var document = new Document(pdfDocument))
            {
                PdfFont boldFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA_BOLD);
                PdfFont regularFont = PdfFontFactory.CreateFont(iText.IO.Font.Constants.StandardFonts.HELVETICA);

                document.Add(new Paragraph("Reservation Confirmation")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20)
                    .SetFont(boldFont));

                document.Add(new Paragraph($"Reservation ID: {reservationId}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Name: {fullName}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Check-in Date: {checkInDate:MMMM dd, yyyy}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Check-out Date: {checkOutDate:MMMM dd, yyyy}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Room Type: {roomDescription}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Number of Guests: {guests}")
                    .SetFontSize(14)
                    .SetFont(regularFont));
                document.Add(new Paragraph($"Total Amount: ${totalAmount:F2}")
                    .SetFontSize(14)
                    .SetFont(regularFont));

                document.Add(new Paragraph("Thank you for your reservation!")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(16)
                    .SetFont(boldFont)
                    .SetMarginTop(20));
            }

            return memoryStream.ToArray();
        }
    }
}
