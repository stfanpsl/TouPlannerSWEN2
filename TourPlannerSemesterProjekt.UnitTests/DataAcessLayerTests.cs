using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TourPlannerSemesterProjekt.Business;
using TourPlannerSemesterProjekt.Business.Services;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.UnitTests
{
    internal class DataAcessLayerTests
    {
        [Test]
        public void GetToursTest()
        {
            var mockList = new List<TourObjekt>
                {
                    new TourObjekt
                    {
                        name = "Test 1",
                        tourDescription = "Desc. 1",
                        from = "Wien",
                        to = "Linz",
                        estimatedTime = "10:00:00",
                        routeInformation = "Info 1",
                        transportType = "By Car",
                        tourDistance = 100
                    },
                    new TourObjekt
                    {
                        name = "Test 2",
                        tourDescription = "Desc. 2",
                        from = "Salzburg",
                        to = "Eisenstadt",
                        estimatedTime = "12:00:00",
                        routeInformation = "Info 2",
                        transportType = "By Bike",
                        tourDistance = 200
                    }
                };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(t => t.GetTours(It.IsAny<String>())).Returns(mockList);

            List<TourObjekt> testlist = mockDB.Object.GetTours();

            Assert.AreEqual(mockList, testlist);
        }

        [Test]
        public void AddTourTest()
        {
            var insertItem = new TourObjekt
            {
                name = "Test 1",
                tourDescription = "Desc. 1",
                from = "Wien",
                to = "Linz",
                estimatedTime = "10:00:00",
                routeInformation = "Info 1",
                transportType = "By Car",
                tourDistance = 100
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            var itemsInserted = new List<TourObjekt>();

            mockDB.Setup(i => i.AddNewTour(It.IsAny<TourObjekt>()))
                  .Callback((TourObjekt item) => itemsInserted.Add(item));

            mockDB.Object.AddNewTour(insertItem);

            Assert.Contains(insertItem, itemsInserted);
        }

        [Test]
        public void DeleteTourTest()
        {
            var mockList = new List<TourObjekt>
                {
                    new TourObjekt
                    {
                        name = "Test 1",
                        tourDescription = "Desc. 1",
                        from = "Wien",
                        to = "Linz",
                        estimatedTime = "10:00:00",
                        routeInformation = "Info 1",
                        transportType = "By Car",
                        tourDistance = 100
                    },
                    new TourObjekt
                    {
                        name = "Test 2",
                        tourDescription = "Desc. 2",
                        from = "Salzburg",
                        to = "Eisenstadt",
                        estimatedTime = "12:00:00",
                        routeInformation = "Info 2",
                        transportType = "By Bike",
                        tourDistance = 200
                    }
                };

            var deleteItem = new TourObjekt
            {
                name = "Test 1",
                tourDescription = "Desc. 1",
                from = "Wien",
                to = "Linz",
                estimatedTime = "10:00:00",
                routeInformation = "Info 1",
                transportType = "By Car",
                tourDistance = 100
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(i => i.DeleteTour(It.IsAny<TourObjekt>()))
                  .Callback((TourObjekt item) => mockList.Remove(item));

            mockDB.Object.DeleteTour(deleteItem);

            CollectionAssert.DoesNotContain(mockList, deleteItem);
        }

        [Test]
        public void EditTourTest()
        {
            var mockList = new List<TourObjekt>
                {
                    new TourObjekt
                    {
                        id = 1,
                        name = "Test 1",
                        tourDescription = "Desc. 1",
                        from = "Wien",
                        to = "Linz",
                        estimatedTime = "10:00:00",
                        routeInformation = "Info 1",
                        transportType = "By Car",
                        tourDistance = 100
                    },
                    new TourObjekt
                    {
                        id = 2,
                        name = "Test 2",
                        tourDescription = "Desc. 2",
                        from = "Salzburg",
                        to = "Eisenstadt",
                        estimatedTime = "12:00:00",
                        routeInformation = "Info 2",
                        transportType = "By Bike",
                        tourDistance = 200
                    }
                };

            var editItem = new TourObjekt
            {
                id = 2,
                name = "Test Edited",
                tourDescription = "Desc. Edited",
                from = "Berlin",
                to = "Wien",
                estimatedTime = "10:00:00",
                routeInformation = "Info Edited",
                transportType = "On Foot",
                tourDistance = 300
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(i => i.EditTour(It.IsAny<TourObjekt>()))
                  .Callback((TourObjekt item) =>
                  {
                      var obj = mockList.FirstOrDefault(x => x.id == item.id);
                      if (obj != null) obj = editItem;
                  }
                  );

            mockDB.Object.EditTour(editItem);

            CollectionAssert.Contains(mockList, editItem);
            //Contains 2 items TBD !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            //CollectionAssert.
        }
    }
}
