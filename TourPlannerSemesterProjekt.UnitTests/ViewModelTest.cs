using Moq;
using NUnit.Framework;
using TourPlannerSemesterProjekt.ViewModels;
using TourPlannerSemesterProjekt.Models;
using System;
using System.Collections.ObjectModel;

namespace TourPlannerSemesterProjekt.UnitTests
{
    internal class ViewModelTest
    {
        TourObjekt tour = new TourObjekt();

        TourLogObjekt log = new TourLogObjekt();

        [SetUp]
        public void Setup()
        {
            tour = new TourObjekt
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

            log = new TourLogObjekt
            {
                l_comment = "Test 1",
                l_date = DateTime.Now,
                l_difficulty = "Medium",
                l_rating = 3,
                l_totaltime = "10:00:00",
                l_tour = 1
            };
        }

        [Test]
        public void AddCanBeExecutedAlways() { 
            MainWindowViewModel viewModel = new MainWindowViewModel();
            Assert.IsTrue(viewModel.AddCommand.CanExecute(null));

            viewModel.TourItems.Add(tour);
            viewModel.CurrentTour = tour;

            Assert.IsTrue(viewModel.AddCommand.CanExecute(null));
        }

        [Test]
        public void EditTourExecuteTest()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            Assert.IsFalse(viewModel.EditCommand.CanExecute(null));

            viewModel.TourItems.Add(tour);
            viewModel.CurrentTour = tour;

            Assert.IsTrue(viewModel.EditCommand.CanExecute(null));
        }


        [Test]
        public void DeleteExecuteTest()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            Assert.IsFalse(viewModel.DeleteCommand.CanExecute(null));

            viewModel.TourItems.Add(tour);
            viewModel.CurrentTour = tour;

            Assert.IsTrue(viewModel.DeleteCommand.CanExecute(null));
        }


        [Test]
        public void EditLogExecuteTest()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            Assert.IsFalse(viewModel.EditLogCommand.CanExecute(null));


            viewModel.TourLogItems = new ObservableCollection<TourLogObjekt>();
            viewModel.TourLogItems.Add(log);
            viewModel.CurrentTourLog = log;

            Assert.IsTrue(viewModel.EditLogCommand.CanExecute(null));
        }


        [Test]
        public void DeleteTourLogExecuteTest()
        {
            MainWindowViewModel viewModel = new MainWindowViewModel();
            Assert.IsFalse(viewModel.DeleteLogCommand.CanExecute(null));

            viewModel.TourLogItems = new ObservableCollection<TourLogObjekt>();
            viewModel.TourLogItems.Add(log);
            viewModel.CurrentTourLog = log;

            Assert.IsTrue(viewModel.DeleteLogCommand.CanExecute(null));
        }


    }
}
