using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class JSONService
    {

        private ITourPlannerDBAccess _dBAccess;
        public JSONService()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _dBAccess = repository;
        }

        /*public void OpenFile(string path)
        {

        }*/

        public void ExportTour(TourObjekt tour)
        {
            using (StreamWriter file = File.CreateText("./" + tour.name +".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, tour);
            }
        }

        public void ImportTour(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                TourObjekt? tourImport = (TourObjekt)serializer.Deserialize(file, typeof(TourObjekt));

                _dBAccess.AddNewTour(tourImport);
            }
        }
    }
}
