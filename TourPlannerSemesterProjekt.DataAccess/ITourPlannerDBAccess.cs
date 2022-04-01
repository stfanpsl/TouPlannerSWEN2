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

        public List<TourObjekt> GetAllTours();

        public void AddNewTour(TourObjekt newtour);

    }
}
