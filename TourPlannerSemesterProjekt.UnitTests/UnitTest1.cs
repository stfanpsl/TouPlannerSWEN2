using NUnit.Framework;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void TourNamingTest()
        {
            TourObjekt tour = new TourObjekt();
            tour.name = "Test";
            Assert.AreEqual("Test", tour.name);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }
    }
}