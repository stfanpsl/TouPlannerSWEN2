using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TourPlannerSemesterProjekt.ViewModels;
using System.Web;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.DataAccess;
using System.Collections.ObjectModel;
using TourPlannerSemesterProjekt.Business;
using System.Windows.Input;
using Microsoft.Win32;
using TourPlannerSemesterProjekt.Views;

namespace TourPlannerSemesterProjekt.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {

        private ObservableCollection<TourObjekt> _tourItems;
        public ObservableCollection<TourObjekt> TourItems
        {
            get => _tourItems;
            set
            {
                if (_tourItems != value)
                {
                    _tourItems = value;
                    RaisePropertyChanged(nameof(TourItems));
                }
            }
        }

        private ObservableCollection<TourLogObjekt> _tourLogItems;
        public ObservableCollection<TourLogObjekt> TourLogItems
        {
            get => _tourLogItems;
            set
            {
                if (_tourLogItems != value)
                {
                    _tourLogItems = value;
                    RaisePropertyChanged(nameof(TourLogItems));
                }
            }
        }

        private ITourPlannerFactory _tourservice;

        //Should probably create a Command Class now that there are this many...
        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ??= new RelayCommand(AddTour);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(DeleteTour);

        private ICommand _editCommand;
        public ICommand EditCommand => _editCommand ??= new RelayCommand(UpdateTour);

        private ICommand _addLogCommand;
        public ICommand AddLogCommand => _addLogCommand ??= new RelayCommand(AddTourLog);

        private ICommand _deleteLogCommand;
        public ICommand DeleteLogCommand => _deleteLogCommand ??= new RelayCommand(DeleteTourLog);

        private ICommand _editLogCommand;
        public ICommand EditLogCommand => _editLogCommand ??= new RelayCommand(UpdateTourLog);

        private ICommand _pdfCommand;
        public ICommand PDFCommand => _pdfCommand ??= new RelayCommand(PrintPdf);

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ??= new RelayCommand(ExportTour);

        private ICommand _importCommand;
        public ICommand ImportCommand => _importCommand ??= new RelayCommand(ImportTour);

        private ICommand _searchCommand;
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(SearchTours);

        private ICommand _searchLogsCommand;
        public ICommand SearchLogsCommand => _searchLogsCommand ??= new RelayCommand(SearchTourLogs);


        private TourObjekt _currentTour;

        public TourObjekt CurrentTour
        {
            get => _currentTour;
            set
            {
                if (_currentTour != value)
                {
                    _currentTour = value;
                    RaisePropertyChanged(nameof(CurrentTour));
                    OnPropertyChanged();
                }
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if(CurrentTour != null)
            {
                GetTourLogs(CurrentTour);
            }
            else
            {
                TourLogItems.Clear();
            }
        }

        private TourLogObjekt _currentTourLog;

        public TourLogObjekt CurrentTourLog
        {
            get => _currentTourLog;
            set
            {
                if (_currentTourLog != value)
                {
                    _currentTourLog = value;
                    RaisePropertyChanged(nameof(CurrentTourLog));
                }
            }
        }


        private string _searchText;

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    RaisePropertyChanged(nameof(SearchText));
                }
            }
        }

        private string _searchTextLog;

        public string SearchTextLog
        {
            get => _searchTextLog;
            set
            {
                if (_searchTextLog != value)
                {
                    _searchTextLog = value;
                    RaisePropertyChanged(nameof(SearchTextLog));
                }
            }
        }

        public MainWindowViewModel()
        {
            _tourservice = TourPlannerFactory.GetInstance();

            GetTours();
        }

        public void GetTours()
        {
            TourItems = new ObservableCollection<TourObjekt>(_tourservice.GetTours());
        }

        public void GetTourLogs(TourObjekt tour)
        {
            CurrentTour.tourlogs = _tourservice.GetTourLogs(tour);
            TourLogItems = new ObservableCollection<TourLogObjekt>(_tourservice.GetTourLogs(tour));
        }

        public void RefreshTourLogs()
        {
            CurrentTour.tourlogs = _tourservice.GetTourLogs(CurrentTour);
            TourLogItems = new ObservableCollection<TourLogObjekt>(_tourservice.GetTourLogs(CurrentTour));
        }

        public void SearchTours(object commandParameter)
        {
            TourItems.Clear();
            TourItems = new ObservableCollection<TourObjekt>(_tourservice.GetTours(SearchText));
        }

        public void SearchTourLogs(object commandParameter)
        {
            CurrentTour.tourlogs = _tourservice.GetTourLogs(CurrentTour);
            TourLogItems = new ObservableCollection<TourLogObjekt>(_tourservice.GetTourLogs(CurrentTour, SearchTextLog));
        }


        private void AddTour(object commandParameter)
        {
            TourAdministration addTourWindow = new TourAdministration(this);
            addTourWindow.Show();
        }

        private void UpdateTour(object commandParameter)
        {
            TourAdministration addTourWindow = new TourAdministration(this, CurrentTour);
            addTourWindow.Show();
        }


        private void PrintPdf(object commandParameter)
        {
            _tourservice.GeneratePdf(CurrentTour);
            MessageBox.Show("Your printed report can be found in the root folder of your installation.", "PDF Generation done");
        }

        private void ExportTour(object commandParameter)
        {
            if(CurrentTour != null)
            {
                _tourservice.ExportTour(CurrentTour);
                MessageBox.Show("Your exported report can be found in the root folder of your installation.", "Export done");
            }
        }

        //Add File Upload Control to View
        private void ImportTour(object commandParameter)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                _tourservice.ImportTour(openFileDialog.FileName);
                GetTours();
                MessageBox.Show("Your tour was imported.", "Import done");
            }
        }

        private void DeleteTour(object commandParameter)
        {
            if (CurrentTour != null)
            {
                var delTour = CurrentTour;
                TourItems.Remove(CurrentTour);
                _tourservice.DeleteTour(delTour);

                CurrentTour = null;
            }
        }

        private void AddTourLog(object commandParameter)
        {
            TourLogAdministration addTourWindow = new TourLogAdministration(this, CurrentTour.id);
            addTourWindow.Show();
        }

        private void UpdateTourLog(object commandParameter)
        {
            TourLogAdministration addTourWindow = new TourLogAdministration(this, CurrentTourLog);
            addTourWindow.Show();
        }

        private void DeleteTourLog(object commandParameter)
        {
            if (CurrentTourLog != null)
            {
                var delTourLog = CurrentTourLog;
                TourLogItems.Remove(CurrentTourLog);
                _tourservice.DeleteTourLog(delTourLog);

                CurrentTourLog = null;
            }
        }

    }
}
