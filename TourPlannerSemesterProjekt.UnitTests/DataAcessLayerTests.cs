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
        public void SearchToursTest()
        {

            var testTour1 = new TourObjekt
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
            var testTour2 = new TourObjekt
            {
                name = "Text 2",
                tourDescription = "Desc. 2",
                from = "Salzburg",
                to = "Eisenstadt",
                estimatedTime = "12:00:00",
                routeInformation = "Info 2",
                transportType = "By Bike",
                tourDistance = 200
            };

            var mockList = new List<TourObjekt>
                {
                    testTour1, testTour2
                };

            var searchString = "Text";

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(t => t.GetTours(It.IsAny<String>()))
                .Returns(mockList.Where(x => x.name.Contains(searchString)).ToList);

            List<TourObjekt> testlist = mockDB.Object.GetTours();

            CollectionAssert.Contains(testlist, testTour2);
            Assert.IsTrue((testlist.Count == 1));
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
                      if (obj != null) mockList.Remove(obj);
                      mockList.Add(item);
                  }
                  );

            mockDB.Object.EditTour(editItem);

            CollectionAssert.Contains(mockList, editItem);
            Assert.IsTrue((mockList.Count == 2));
        }

        [Test]
        public void GetTourLogsTest()
        {
            var mockList = new List<TourLogObjekt>
                {
                    new TourLogObjekt
                    {
                        l_comment = "Test 1",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 3,
                        l_totaltime = "10:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_comment = "Test 1-2",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 1,
                        l_totaltime = "14:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_comment = "Test 2",
                        l_date = DateTime.Now,
                        l_difficulty = "Easy",
                        l_rating = 2,
                        l_totaltime = "12:00:00",
                        l_tour = 2
                    }
                };

            TourObjekt testTour = new TourObjekt
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
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(t => t.GetTourLogs(It.IsAny<TourObjekt>(), It.IsAny<String>()))
                .Returns(mockList.Where(x => x.l_tour == testTour.id).ToList);

            List<TourLogObjekt> testlist = mockDB.Object.GetTourLogs(testTour);

            mockList.RemoveAll(x => x.l_tour == 2);

            Assert.AreEqual(mockList, testlist);
        }

        [Test]
        public void SearchTourLogsTest()
        {
            var searchString = "Text";

            var testLog1 = new TourLogObjekt
            {
                l_comment = "Test 1",
                l_date = DateTime.Now,
                l_difficulty = "Medium",
                l_rating = 3,
                l_totaltime = "10:00:00",
                l_tour = 1
            };
            var testLog2 = new TourLogObjekt
            {
                l_comment = "Text 1-2",
                l_date = DateTime.Now,
                l_difficulty = "Medium",
                l_rating = 1,
                l_totaltime = "14:00:00",
                l_tour = 1
            };
            var testLog3 = new TourLogObjekt
            {
                l_comment = "Test 2",
                l_date = DateTime.Now,
                l_difficulty = "Easy",
                l_rating = 2,
                l_totaltime = "12:00:00",
                l_tour = 2
            };

            var mockList = new List<TourLogObjekt>
                {
                    testLog1, testLog2, testLog3
                };

            TourObjekt testTour = new TourObjekt
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
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(t => t.GetTourLogs(It.IsAny<TourObjekt>(), It.IsAny<String>()))
                .Returns(mockList.Where(x => x.l_tour == testTour.id & x.l_comment.Contains(searchString)).ToList);

            List<TourLogObjekt> testlist = mockDB.Object.GetTourLogs(testTour, searchString);

            CollectionAssert.Contains(testlist, testLog2);
        }



        [Test]
        public void AddTourLogTest()
        {
            var insertItem = new TourLogObjekt
            {
                l_comment = "Test 1",
                l_date = DateTime.Now,
                l_difficulty = "Medium",
                l_rating = 3,
                l_totaltime = "10:00:00",
                l_tour = 1
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            var itemsInserted = new List<TourLogObjekt>();

            mockDB.Setup(i => i.AddNewTourLog(It.IsAny<TourLogObjekt>()))
                  .Callback((TourLogObjekt item) => itemsInserted.Add(item));

            mockDB.Object.AddNewTourLog(insertItem);

            Assert.Contains(insertItem, itemsInserted);
        }

        [Test]
        public void DeleteTourLogTest()
        {
            var mockList = new List<TourLogObjekt>
                {
                    new TourLogObjekt
                    {
                        l_comment = "Test 1",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 3,
                        l_totaltime = "10:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_comment = "Test 1-2",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 1,
                        l_totaltime = "14:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_comment = "Test 2",
                        l_date = DateTime.Now,
                        l_difficulty = "Easy",
                        l_rating = 2,
                        l_totaltime = "12:00:00",
                        l_tour = 2
                    }
                };

            var deleteItem = new TourLogObjekt()
                    {
                        l_comment = "Test 1-2",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 1,
                        l_totaltime = "14:00:00",
                        l_tour = 1
                    };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(i => i.DeleteTourLog(It.IsAny<TourLogObjekt>()))
                  .Callback((TourLogObjekt item) => mockList.Remove(item));

            mockDB.Object.DeleteTourLog(deleteItem);

            CollectionAssert.DoesNotContain(mockList, deleteItem);
        }

        [Test]
        public void EditTourLogTest()
        {
            var mockList = new List<TourLogObjekt>
                {
                    new TourLogObjekt
                    {
                        l_id = 1,
                        l_comment = "Test 1",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 3,
                        l_totaltime = "10:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_id = 2,
                        l_comment = "Test 1-2",
                        l_date = DateTime.Now,
                        l_difficulty = "Medium",
                        l_rating = 1,
                        l_totaltime = "14:00:00",
                        l_tour = 1
                    },
                    new TourLogObjekt
                    {
                        l_id = 3,
                        l_comment = "Test 2",
                        l_date = DateTime.Now,
                        l_difficulty = "Easy",
                        l_rating = 2,
                        l_totaltime = "12:00:00",
                        l_tour = 2
                    }
                };

            var editItem = new TourLogObjekt
            {
                l_id = 2,
                l_comment = "Test Edited",
                l_date = DateTime.Now,
                l_difficulty = "Easy",
                l_rating = 4,
                l_totaltime = "22:00:00",
                l_tour = 1
            };

            Mock<ITourPlannerDBAccess> mockDB = new Mock<ITourPlannerDBAccess>();

            mockDB.Setup(i => i.EditTourLog(It.IsAny<TourLogObjekt>()))
                  .Callback((TourLogObjekt item) =>
                  {
                      var obj = mockList.FirstOrDefault(x => x.l_id == item.l_id);
                      if (obj != null) mockList.Remove(obj);
                      mockList.Add(item);
                  }
                  );

            mockDB.Object.EditTourLog(editItem);

            CollectionAssert.Contains(mockList, editItem);
            Assert.IsTrue((mockList.Count == 3));
        }

    }
}
