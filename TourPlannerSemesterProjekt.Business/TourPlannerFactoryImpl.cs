using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Business.Services;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TourPlannerSemesterProjekt.Business
{
    public class TourPlannerFactoryImpl : ITourPlannerFactory
    {
        private ITourPlannerDBAccess _dBAccess;

        public TourPlannerFactoryImpl()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _dBAccess = repository;
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
            string imagePath = _mapQuestService.GetImage();
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

        public void GeneratePdf(TourObjekt newtour)
        {
            PDFGeneratorService _pdfGeneratorService = new PDFGeneratorService();
            _pdfGeneratorService.printPdf(newtour);
        }

        //JUST FOR TESTING: needs to be divided up to DAL (FileAccess) and own BL-Class (IO/JSON-Service)
        public void ExportTour(TourObjekt tour)
        {
            using (StreamWriter file = File.CreateText("./tour_export.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, tour);
            }
        }

        //JUST FOR TESTING: needs to be divided up to DAL (FileAccess) and own BL-Class (IO/JSON-Service)
        public void ImportTour()
        {
            using (StreamReader file = File.OpenText(@"./tour_export.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                TourObjekt? tourImport = (TourObjekt)serializer.Deserialize(file, typeof(TourObjekt));

                _dBAccess.AddNewTour(tourImport);
            }
        }
    }
}
