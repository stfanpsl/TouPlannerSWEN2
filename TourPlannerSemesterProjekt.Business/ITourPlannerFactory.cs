using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business
{
    public interface ITourPlannerFactory
    {
        public List<TourObjekt> GetTours(string searchText = "");

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "");

        public bool CheckTour(TourObjekt newtour);

        public void AddNewTour(TourObjekt newtour);

        public void DeleteTour(TourObjekt tour);

        public void EditTour(TourObjekt newtour, bool newRoute = false);

        public void AddNewTourLog(TourLogObjekt newtourlog);

        public void DeleteTourLog(TourLogObjekt tourlog);

        public void EditTourLog(TourLogObjekt newtourlog);

        public double GetFuelorCalories(TourObjekt tour);

        public void GeneratePdf(TourObjekt newtour);

        public void GenerateSumPdf(List<TourObjekt> tours);

        public void ExportTour(TourObjekt tour);
        public void ImportTour(string FilePath);
    }
}