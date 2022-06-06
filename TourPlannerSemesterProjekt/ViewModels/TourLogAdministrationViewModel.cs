using System;
using System.Windows;
using System.Windows.Input;
using TourPlannerSemesterProjekt.Business;
using TourPlannerSemesterProjekt.Logging;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt.ViewModels
{
    public class TourLogAdministrationViewModel : BaseViewModel
    {
        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        private Window _window;
        private MainWindowViewModel _mainView;

        private TourLogObjekt currentItem = new TourLogObjekt(DateTime.Now, "Tour-Log Comment", "Easy", "HH:MM:SS", 1);
        public TourLogObjekt CurrentItem
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


        public TourLogAdministrationViewModel(Window window, MainWindowViewModel mainView, int tourID)
        {
            _window = window;
            _mainView = mainView;
            _tourservice = TourPlannerFactory.GetInstance();

            currentItem.l_tour = tourID;

            SaveCommand = new RelayCommand(o =>
            {
                InsertTourLog(currentItem);
            }, canExecuteSaveCommand);
        }

        public TourLogAdministrationViewModel(Window window, MainWindowViewModel mainView, TourLogObjekt tourlog)
        {
            _window = window;
            _mainView = mainView;

            currentItem = tourlog;

            _tourservice = TourPlannerFactory.GetInstance();


            SaveCommand = new RelayCommand(o =>
            {
                UpdateTourLog(currentItem);
            }, canExecuteSaveCommand);
        }

        private void InsertTourLog(TourLogObjekt newTourLog)
        {
            if (TimeSpan.TryParse(newTourLog.l_totaltime, out var dummyOutput))
            {

                logger.Debug("New Tour-Log: '" + newTourLog.l_date.ToString() + "' created.");

                _tourservice.AddNewTourLog(newTourLog);
                _mainView.RefreshTourLogs();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Please check if the 'Total Time' field is filled in using the 'HH:MM:SS' time-format.", "Wrong Time Format!");
            }
        }

        private void UpdateTourLog(TourLogObjekt newTourLog)
        {
            if (TimeSpan.TryParse(newTourLog.l_totaltime, out var dummyOutput))
            {
                _tourservice.EditTourLog(newTourLog);
                _mainView.RefreshTourLogs();
                _window.Close();
            }
            else
            {
                MessageBox.Show("Please check if the 'Total Time' field is filled in using the 'HH:MM:SS' time-format.", "Wrong Time Format!");
            }
        }

        private bool canExecuteSaveCommand(object commandParameter)
        {
            if (string.IsNullOrEmpty(currentItem.l_comment) ||
                string.IsNullOrEmpty(currentItem.l_totaltime))
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
