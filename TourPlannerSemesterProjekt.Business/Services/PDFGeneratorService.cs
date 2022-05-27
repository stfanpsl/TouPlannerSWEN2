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
            tourNameHeader.SetFontSize(14)
                .SetBold();
            document.Add(tourNameHeader);
            document.Add(new Paragraph(tour.tourDescription));

            Paragraph listHeader = new Paragraph("Tour Data:");
            List list = new List()
                      .SetSymbolIndent(12);

            Paragraph fromList = new Paragraph();
            fromList.Add(new Text("From: ").SetBold())
                .Add(new Text(tour.from));
            Paragraph toList = new Paragraph();
            toList.Add(new Text("To: ").SetBold())
                .Add(new Text(tour.to));
            Paragraph transportList = new Paragraph();
            transportList.Add(new Text("Transport-Type: ").SetBold())
                .Add(new Text(tour.transportType));
            Paragraph distanceList = new Paragraph();
            distanceList.Add(new Text("Distance: ").SetBold())
                .Add(new Text(tour.tourDistance.ToString()))
                .Add(new Text(" km").SetBold());
            Paragraph estTimeList = new Paragraph();
            estTimeList.Add(new Text("Estimated Time: ").SetBold())
                .Add(new Text(tour.estimatedTime));
            Paragraph routeInfoList = new Paragraph();
            routeInfoList.Add(new Text("Route-Information: ").SetBold())
                .Add(new Text(tour.routeInformation));

            ListItem li1 = new ListItem();
            li1.Add(fromList);
            ListItem li2 = new ListItem();
            li2.Add(toList);
            ListItem li3 = new ListItem();
            li3.Add(transportList);
            ListItem li4 = new ListItem();
            li4.Add(distanceList);
            ListItem li5 = new ListItem();
            li5.Add(estTimeList);
            ListItem li6 = new ListItem();
            li6.Add(routeInfoList);

            list.Add(li1)
                      .Add(li2)
                      .Add(li3)
                      .Add(li4)
                      .Add(li5)
                      .Add(li6);
            document.Add(listHeader);
            document.Add(list);

            Paragraph imageHeader = new Paragraph("Route-Image: ")
                    .SetFontSize(14)
                    .SetBold();
            document.Add(imageHeader);
            ImageData imageData = ImageDataFactory.Create(tour.imagePath);
            Image img = new Image(imageData);
            img.ScaleAbsolute(400, 400);
            img.SetHorizontalAlignment(iText.Layout.Properties.HorizontalAlignment.CENTER);
            document.Add(img);

            foreach (TourLogObjekt log in tour.tourlogs)
            {
                document.Add(new AreaBreak());
                Paragraph tourLogHeader = new Paragraph(log.l_date.ToLongDateString() + " " + log.l_date.ToShortTimeString());
                tourLogHeader.SetFontSize(16)
                    .SetBold();
                document.Add(tourLogHeader);

                Paragraph loglistHeader = new Paragraph("Tour Log Data:");
                List loglist = new List()
                          .SetSymbolIndent(12);

                Paragraph commentList = new Paragraph();
                commentList.Add(new Text("Comment: ").SetBold())
                    .Add(new Text(log.l_comment));
                Paragraph diffList = new Paragraph();
                diffList.Add(new Text("Difficulty: ").SetBold())
                    .Add(new Text(log.l_difficulty));
                Paragraph timeList = new Paragraph();
                timeList.Add(new Text("Total Time: ").SetBold())
                    .Add(new Text(log.l_totaltime));
                Paragraph ratingList = new Paragraph();
                ratingList.Add(new Text("Rating: ").SetBold())
                    .Add(new Text(log.l_rating.ToString()));

                ListItem logli1 = new ListItem();
                logli1.Add(commentList);
                ListItem logli2 = new ListItem();
                logli2.Add(diffList);
                ListItem logli3 = new ListItem();
                logli3.Add(timeList);
                ListItem logli4 = new ListItem();
                logli4.Add(ratingList);

                loglist.Add(logli1)
                          .Add(logli2)
                          .Add(logli3)
                          .Add(logli4);
                document.Add(loglistHeader);
                document.Add(loglist);
            }



            document.Close();
        }

    }
}
