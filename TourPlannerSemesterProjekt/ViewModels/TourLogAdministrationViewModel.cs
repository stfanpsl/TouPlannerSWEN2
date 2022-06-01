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
    public class TourLogAdministrationViewModel : BaseViewModel
    {
            private static ILoggerWrapper logger = LoggerFactory.GetLogger();

            private Window _window;
            private MainWindowViewModel _mainView;

            private TourLogObjekt currentItem = new TourLogObjekt(DateTime.Now, "Tour-Log Comment", "Easy", "Tour-Log Total-Time", 1);
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

            //public ICommand CloseWindowCommand { get; set; }
            
            public TourLogAdministrationViewModel(Window window, MainWindowViewModel mainView, int tourID)
            {
                _window = window;
                _mainView = mainView;
                _tourservice = TourPlannerFactory.GetInstance();

                currentItem.l_tour = tourID;
                //RaisePropertyChanged(nameof(CurrentItem));

                SaveCommand = new RelayCommand(o => {
                    //var newTourItem = CurrentItem;
                    InsertTourLog(currentItem);
                });
            }

            public TourLogAdministrationViewModel(Window window, MainWindowViewModel mainView, TourLogObjekt tourlog)
            {
                _window = window;
                _mainView = mainView;

                currentItem = tourlog;

                _tourservice = TourPlannerFactory.GetInstance();

                //RaisePropertyChanged(nameof(CurrentItem));

                SaveCommand = new RelayCommand(o => {
                    UpdateTourLog(currentItem);
                });
            }

            private void InsertTourLog(TourLogObjekt newTourLog)
            {
                logger.Debug("New Tour-Log: '" + newTourLog.l_date.ToString() + "' created.");

                    _tourservice.AddNewTourLog(newTourLog);
                    _mainView.RefreshTourLogs();
                    _window.Close();
            }

            private void UpdateTourLog(TourLogObjekt newTourLog)
            {
                _tourservice.EditTourLog(newTourLog);
                _mainView.RefreshTourLogs();
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
