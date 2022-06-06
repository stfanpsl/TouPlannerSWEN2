using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.DataAccess
{
    public interface ITourPlannerDBAccess
    {

        public List<TourObjekt> GetTours(string searchText = "");

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "");

        public int AddNewTour(TourObjekt newtour);

        public void DeleteTour(TourObjekt newtour);

        public void EditTour(TourObjekt newtour);

        public void AddNewTourLog(TourLogObjekt newtourlog);

        public void DeleteTourLog(TourLogObjekt newtourlog);

        public void EditTourLog(TourLogObjekt newtourlog);

    }
}
