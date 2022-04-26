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
            MapQuestService _mapQuestService = new MapQuestService(newtour.from, newtour.to);
            string imagePath = _mapQuestService.LoadImage();
            double tourDistance = _mapQuestService.GetRouteDistance();

            newtour.tourDistance = tourDistance;
            newtour.imagePath = imagePath;

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

        public void EditTour(TourObjekt newtour)
        {
            try
            {
                _dBAccess.EditTour(newtour);
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }
        }


        public void DeleteTour(TourObjekt tour)
        {
            try
            {
                _dBAccess.DeleteTour(tour);
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }
        }

    }
}
