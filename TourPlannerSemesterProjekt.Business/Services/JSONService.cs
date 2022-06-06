using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.Logging;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class JSONService
    {
        IConfiguration config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();

        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        private ITourPlannerDBAccess _dBAccess;
        public JSONService()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _dBAccess = repository;
        }


        public void ExportTour(TourObjekt tour)
        {
            using (StreamWriter file = File.CreateText(config["filePath"] + tour.name + "_" + DateTime.Now.ToShortDateString() + ".json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, tour);
                logger.Debug("Tour Exported: '" + tour.name);
            }
        }

        public void ImportTour(string filePath)
        {
            using (StreamReader file = File.OpenText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                TourObjekt? tourImport = (TourObjekt)serializer.Deserialize(file, typeof(TourObjekt));

                int id = _dBAccess.AddNewTour(tourImport);

                foreach (TourLogObjekt log in tourImport.tourlogs)
                {
                    log.l_tour = id;
                    _dBAccess.AddNewTourLog(log);
                }
                logger.Debug("Tour Imported: '" + tourImport.name);
            }
        }
    }
}
