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

        private ITourPlannerFactory _tourservice;

        //Should probably create a Command Class now that there are this many...
        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ??= new RelayCommand(AddTour);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(DeleteTour);

        private ICommand _editCommand;
        public ICommand EditCommand => _editCommand ??= new RelayCommand(UpdateTour);

        private ICommand _pdfCommand;
        public ICommand PDFCommand => _pdfCommand ??= new RelayCommand(PrintPdf);

        private ICommand _exportCommand;
        public ICommand ExportCommand => _exportCommand ??= new RelayCommand(ExportTour);

        private ICommand _importCommand;
        public ICommand ImportCommand => _importCommand ??= new RelayCommand(ImportTour);


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
                }
            }
        }

        public MainWindowViewModel()
        {
            _tourservice = TourPlannerFactory.GetInstance();

            GetAllTours();
        }

        public void GetAllTours()
        {
            TourItems = new ObservableCollection<TourObjekt>(_tourservice.GetAllTours());
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
            _tourservice.ExportTour(CurrentTour);
            MessageBox.Show("Your exported report can be found in the root folder of your installation.", "Export done");
        }

        private void ImportTour(object commandParameter)
        {
            _tourservice.ImportTour();
            GetAllTours();
            MessageBox.Show("Your tour was imported.", "Import done");
        }

        private void DeleteTour(object commandParameter)
        {

            if (CurrentTour != null)
            {
                _tourservice.DeleteTour(CurrentTour);
                TourItems.Remove(CurrentTour);

                CurrentTour = null;
            }
        }

    }
}
