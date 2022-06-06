using System.Windows;
using System.Windows.Input;
using TourPlannerSemesterProjekt.Business;
using TourPlannerSemesterProjekt.Logging;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.ViewModels
{
    public class TourAdministrationViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        private Window _window;
        private MainWindowViewModel _mainView;

        private TourObjekt currentItem = new TourObjekt("New Tour", "Tour Description", "From", "To", "By Car", "Route Information");
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


        public TourAdministrationViewModel(Window window, MainWindowViewModel mainView)
        {
            _window = window;
            _mainView = mainView;
            _tourservice = TourPlannerFactory.GetInstance();


            SaveCommand = new RelayCommand(o =>
            {
                InsertTour(currentItem);
            }, canExecuteSaveCommand);
        }

        public TourAdministrationViewModel(Window window, MainWindowViewModel mainView, TourObjekt tour)
        {
            _window = window;
            _mainView = mainView;

            currentItem = tour;

            var initFrom = currentItem.from;
            var initTo = currentItem.to;

            _tourservice = TourPlannerFactory.GetInstance();


            SaveCommand = new RelayCommand(o =>
            {
                if(currentItem.from != initFrom || currentItem.to != initTo)
                {
                    UpdateTour(currentItem, true);
                }
                else
                {
                    UpdateTour(currentItem, false);
                }
            }, canExecuteSaveCommand);
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

        private void UpdateTour(TourObjekt newTour, bool newRoute)
        {
            if (newRoute) {
                if (_tourservice.CheckTour(newTour))
                {
                    _tourservice.EditTour(newTour, true);
                    _mainView.GetTours();
                    _window.Close();
                }
                else
                {
                    MessageBox.Show("Please check if the 'To' and 'From' fields have been correctly filled in.'", "Route could not be found!");
                }
            }
            else { 
                _tourservice.EditTour(newTour);
                _mainView.GetTours();
                _window.Close();
            }
        }

        private bool canExecuteSaveCommand(object commandParameter)
        {
            if (string.IsNullOrEmpty(currentItem.name) ||
                string.IsNullOrEmpty(currentItem.from) ||
                string.IsNullOrEmpty(currentItem.to) ||
                string.IsNullOrEmpty(currentItem.routeInformation) ||
                string.IsNullOrEmpty(currentItem.tourDescription))
            {
                return false;
            }
            else
            {
                return true;
            }
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
