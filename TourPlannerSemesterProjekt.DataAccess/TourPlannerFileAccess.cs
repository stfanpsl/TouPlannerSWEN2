using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void SaveJSON()
        {

        }

        public void DeleteFile(string delFilePath)
        {
            File.Delete(delFilePath);
        }

    }
}
