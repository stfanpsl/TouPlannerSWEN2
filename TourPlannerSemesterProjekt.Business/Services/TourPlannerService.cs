using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.DataAccess;
using System.Diagnostics;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class TourPlannerService : ITourPlannerService
    {
        private ITourPlannerDBAccess _dBAccess;

        public TourPlannerService(ITourPlannerDBAccess dbAccess)
        {
            _dBAccess = dbAccess;
        }

        public List<TourObjekt> GetAllTours()
        {
            try
            {
                var allTours = _dBAccess.GetAllTours();
                return allTours;
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }
        }


        public void AddNewTour(TourObjekt newtour)
        {
            try
            {
                _dBAccess.AddNewTour(newtour);
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }
        }

    }
}
