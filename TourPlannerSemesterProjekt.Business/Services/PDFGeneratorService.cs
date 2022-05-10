using System;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class PDFGeneratorService
    {

        public void printPdf(TourObjekt tour)
        {
            PdfWriter writer = new PdfWriter(tour.name + "_" + DateTime.Now.ToShortDateString() + ".pdf");
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph tourNameHeader = new Paragraph(tour.name);
            document.Add(tourNameHeader);
            document.Add(new Paragraph(tour.tourDescription));

            Paragraph listHeader = new Paragraph("Tour Data:");
            List list = new List()
                      .SetSymbolIndent(12)
                      .SetListSymbol("\u2022");
            list.Add(new ListItem("From: " + tour.from))
                      .Add(new ListItem("From: " + tour.from))
                      .Add(new ListItem("To: " + tour.to))
                      .Add(new ListItem("Transport-Type: " + tour.transportType))
                      .Add(new ListItem("Route-Information: " + tour.from))
                      .Add(new ListItem("Tour-Distance: " + tour.from));
            document.Add(listHeader);
            document.Add(list);

            Paragraph imageHeader = new Paragraph("Route:")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.GREEN);
            document.Add(imageHeader);
            ImageData imageData = ImageDataFactory.Create(tour.imagePath);
            imageData.SetXYRatio(0.3f);
            document.Add(new Image(imageData));

            document.Close();
        }

    }
}
