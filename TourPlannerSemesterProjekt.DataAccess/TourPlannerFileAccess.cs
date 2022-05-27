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
        private readonly string _filePath = ConfigurationManager.AppSettings["ImagePath"];

        public string SaveImage(Image image)
        {
            var fileName = System.IO.Path.GetRandomFileName() + ".jpg";
            var fullFilePath = _filePath + fileName;

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
