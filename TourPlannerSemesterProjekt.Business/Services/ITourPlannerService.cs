using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public interface ITourPlannerService
    {
        public List<TourObjekt> GetAllTours();

        public void AddNewTour(TourObjekt newtour);

        //public void RemoveTour(TourObjekt newtour);



    }
}
