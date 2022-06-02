using Moq;
using NUnit.Framework;
using System;
using TourPlannerSemesterProjekt.Business;
using TourPlannerSemesterProjekt.Business.Services;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.UnitTests
{
    public class BusinessLayerTests
    {
        ITourPlannerFactory _tourService;

        [SetUp]
        public void Setup()
        {
            _tourService = TourPlannerFactory.GetInstance();
        }

        [Test]
        public void RouteCheckFail()
        {
            TourObjekt testTour = new TourObjekt();
            testTour.from = "Test";
            testTour.to = "Test";

            Assert.IsFalse(_tourService.CheckTour(testTour)); 
        }

        [Test]
        public void RouteCheckPass()
        {
            TourObjekt testTour = new TourObjekt();
            testTour.from = "Wien";
            testTour.to = "Berlin";

            Assert.IsTrue(_tourService.CheckTour(testTour));
        }

        [Test]
        public void RouteCheckDistance()
        {
            double expectedDistance = 689.7;
            TourObjekt testTour = new TourObjekt();
            testTour.from = "Berlin";
            testTour.to = "Wien";

            MapQuestService _mapService = new MapQuestService(testTour.from, testTour.to);

            Assert.AreEqual(Math.Round(_mapService.GetRouteDistance(), 2), expectedDistance);
        }

        [Test]
        public void UniqueFeatureTest()
        {
            double expectedFuelUsage = 4.11;
            double expectedCalsBurnt = 1227.78;
            TourObjekt testTour = new TourObjekt();
            testTour.from = "Eisenstadt";
            testTour.to = "Wien";

            MapQuestService _mapService = new MapQuestService(testTour.from, testTour.to);
            testTour.tourDistance = _mapService.GetRouteDistance(); 

            testTour.transportType = "By Car";
            Assert.AreEqual(expectedFuelUsage, _tourService.GetFuelorCalories(testTour));

            testTour.transportType = "On Foot";
            Assert.AreEqual(expectedCalsBurnt, _tourService.GetFuelorCalories(testTour));
        }
    }
}