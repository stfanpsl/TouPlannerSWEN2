using Npgsql;
using System.Diagnostics;
using TourPlannerSemesterProjekt.Business.Services;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Logging;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business
{
    public class TourPlannerFactoryImpl : ITourPlannerFactory
    {
        private ITourPlannerDBAccess _dBAccess;

        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        public TourPlannerFactoryImpl()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _dBAccess = repository;
        }

        public List<TourObjekt> GetTours(string searchText = "")
        {
            var allTours = new List<TourObjekt>();
            try
            {
                if (searchText != "")
                {
                    allTours = _dBAccess.GetTours(searchText);
                }
                else
                {
                    allTours = _dBAccess.GetTours();
                }
                return allTours;
            }
            catch (Exception ex)
            {
                logger.Fatal("Tours could not be loaded. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "")
        {
            var allTours = new List<TourLogObjekt>();
            try
            {
                if (searchText != "")
                {
                    allTours = _dBAccess.GetTourLogs(tour, searchText);
                }
                else
                {
                    allTours = _dBAccess.GetTourLogs(tour);
                }
                return allTours;
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour-Logs could not be loaded. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }


        public void AddNewTour(TourObjekt newtour)
        {
            MapQuestService _mapQuestService = new MapQuestService(newtour.from, newtour.to);
            string imagePath = _mapQuestService.GetImage();
            double tourDistance = _mapQuestService.GetRouteDistance();
            string arrivalTime = _mapQuestService.GetArrivalTime();


            newtour.tourDistance = Math.Round(tourDistance, 2);
            newtour.caloriefuel = GetFuelorCalories(newtour);
            newtour.estimatedTime = arrivalTime;
            newtour.imagePath = imagePath;

            try
            {
                logger.Debug("New Tour: '" + newtour.name + "' created.");
                _dBAccess.AddNewTour(newtour);
            }
            catch (Exception ex)
            {
                logger.Fatal("New Tour: '" + newtour.name + "' could not be added. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }

        public double GetFuelorCalories(TourObjekt tour)
        {
            double caloriefuel = 0;
            switch (tour.transportType)
            {
                case "By Car":
                    caloriefuel = (tour.tourDistance * 0.077);
                    break;
                case "By Bike":
                    caloriefuel = (tour.tourDistance * 62);
                    break;
                case "On Foot":
                    caloriefuel = (tour.tourDistance * 23);
                    break;
            }

            return Math.Round(caloriefuel, 2);
        }

        public bool CheckTour(TourObjekt newtour)
        {
            MapQuestService _mapQuestService = new MapQuestService(newtour.from, newtour.to);
            if (_mapQuestService.CheckRoute())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void EditTour(TourObjekt newtour)
        {
            try
            {

                newtour.caloriefuel = GetFuelorCalories(newtour);

                _dBAccess.EditTour(newtour);
                logger.Debug("Tour: '" + newtour.name + "' edited.");
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour: '" + newtour.name + "' could not be edited. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }


        public void DeleteTour(TourObjekt tour)
        {
            TourPlannerFileAccess fileAcess = new TourPlannerFileAccess();
            try
            {
                _dBAccess.DeleteTour(tour);
                logger.Debug("Tour: '" + tour.name + "' deleted.");
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour: '" + tour.name + "' could not be deleted. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }


        public void AddNewTourLog(TourLogObjekt newtourlog)
        {
            try
            {
                _dBAccess.AddNewTourLog(newtourlog);
                logger.Debug("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' deleted.");
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' could not be added. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }

        public void EditTourLog(TourLogObjekt newtourlog)
        {
            try
            {
                _dBAccess.EditTourLog(newtourlog);
                logger.Debug("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' edited.");
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' could not be edited. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }


        public void DeleteTourLog(TourLogObjekt newtourlog)
        {
            TourPlannerFileAccess fileAcess = new TourPlannerFileAccess();
            try
            {
                _dBAccess.DeleteTourLog(newtourlog);
                logger.Debug("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' deleted.");
            }
            catch (Exception ex)
            {
                logger.Fatal("Tour-Log: '" + newtourlog.l_date.ToShortDateString() + "' could not be deleted. Exception: " + ex);
                throw new Exception("Error occurred.", ex);
            }
        }

        public void GeneratePdf(TourObjekt tour)
        {
            PDFGeneratorService _pdfGeneratorService = new PDFGeneratorService();
            _pdfGeneratorService.printPdf(tour);
        }

        public void GenerateSumPdf(List<TourObjekt> tours)
        {
            PDFGeneratorService _pdfGeneratorService = new PDFGeneratorService();
            _pdfGeneratorService.printSumPdf(tours);
        }

        public void ExportTour(TourObjekt tour)
        {
            JSONService _jSONService = new JSONService();
            _jSONService.ExportTour(tour);
        }

        public void ImportTour(string filePath)
        {
            JSONService _jSONService = new JSONService();
            _jSONService.ImportTour(filePath);
        }
    }
}
