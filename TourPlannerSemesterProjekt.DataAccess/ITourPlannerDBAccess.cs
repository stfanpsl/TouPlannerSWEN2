using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.DataAccess
{
    public interface ITourPlannerDBAccess
    {

        public List<TourObjekt> GetTours(string searchText = "");

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "");

        public void AddNewTour(TourObjekt newtour);

        public void DeleteTour(TourObjekt newtour);

        public void EditTour(TourObjekt newtour);

    }
}
