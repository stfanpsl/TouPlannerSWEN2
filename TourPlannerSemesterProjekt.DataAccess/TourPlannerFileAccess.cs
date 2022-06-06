using Microsoft.Extensions.Configuration;
using System.Drawing;
using System.Drawing.Imaging;

namespace TourPlannerSemesterProjekt.DataAccess
{
    public class TourPlannerFileAccess
    {

        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();

        public string SaveImage(Image image)
        {
            var fileName = System.IO.Path.GetRandomFileName() + ".jpg";
            var fullFilePath = config["ImagePath"] + fileName;

            image.Save(fullFilePath, ImageFormat.Jpeg);

            return fullFilePath;
        }


        public void DeleteFile(string delFilePath)
        {
            File.Delete(delFilePath);
        }

    }
}
