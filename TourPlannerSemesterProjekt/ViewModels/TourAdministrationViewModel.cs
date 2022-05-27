using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Business;
using TourPlannerSemesterProjekt.Logging;
using System.Windows.Input;
using System.Windows;

namespace TourPlannerSemesterProjekt.ViewModels
{
    public class TourAdministrationViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        private Window _window;
        private MainWindowViewModel _mainView;

        private TourObjekt currentItem = new TourObjekt("New Tour", "Tour Description", "From", "To", "Transport-type", "Route Information");
        public TourObjekt CurrentItem
        {
            get { return currentItem; }
            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChanged(nameof(CurrentItem));
                }
            }
        }

        private ITourPlannerFactory _tourservice;

        public ICommand SaveCommand { get; set; }

        //public ICommand CloseWindowCommand { get; set; }

        public TourAdministrationViewModel(Window window, MainWindowViewModel mainView)
        {
            _window = window;
            _mainView = mainView;
            _tourservice = TourPlannerFactory.GetInstance();

            //RaisePropertyChanged(nameof(CurrentItem));

            SaveCommand = new RelayCommand(o => {
                //var newTourItem = CurrentItem;
                InsertTour(currentItem);
            });
        }

        public TourAdministrationViewModel(Window window, MainWindowViewModel mainView, TourObjekt tour)
        {
            _window = window;
            _mainView = mainView;

            currentItem = tour;

            _tourservice = TourPlannerFactory.GetInstance();

            //RaisePropertyChanged(nameof(CurrentItem));

            SaveCommand = new RelayCommand(o => {
                UpdateTour(currentItem);
            });
        }

        private void InsertTour(TourObjekt newTour)
        {
            logger.Debug("New Tour: '" + newTour.name + "' created.");
            if (_tourservice.CheckTour(newTour))
            {
                _tourservice.AddNewTour(newTour);
                _mainView.GetTours();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Please check if the 'To' and 'From' fields have been correctly filled in.'", "Route could not be found!");
            }
        }

        private void UpdateTour(TourObjekt newTour)
        {
            _tourservice.EditTour(newTour);
            _mainView.GetTours();
            _window.Close();
        }

        private void CloseWindow(Window window)
        {
            if (window != null)
            {
                window.Close();
            }
        }

    }
}
