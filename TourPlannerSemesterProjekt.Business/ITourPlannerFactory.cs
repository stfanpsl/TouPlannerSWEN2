using System.Collections.Generic;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business
{
    public interface ITourPlannerFactory
    {
        public List<TourObjekt> GetTours(string searchText = "");

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "");

        public void AddNewTour(TourObjekt newtour);

        public void DeleteTour(TourObjekt tour);

        public void EditTour(TourObjekt newtour);

        public void GeneratePdf(TourObjekt newtour);
        //JUST FOR TESTING: needs to be divided up to DAL (FileAccess) and own BL-Class (IO/JSON-Service)
        public void ExportTour(TourObjekt tour);
        //JUST FOR TESTING: needs to be divided up to DAL (FileAccess) and own BL-Class (IO/JSON-Service)
        public void ImportTour(string FilePath);
    }
}