using System.Collections.Generic;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business
{
    public interface ITourPlannerFactory
    {
        public List<TourObjekt> GetAllTours();

        public void AddNewTour(TourObjekt newtour);

        public void DeleteTour(TourObjekt tour);

        public void EditTour(TourObjekt newtour);
    }
}