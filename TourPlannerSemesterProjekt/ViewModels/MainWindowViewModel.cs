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

        private ICommand _addCommand;
        public ICommand AddCommand => _addCommand ??= new RelayCommand(AddTour);

        private ICommand _deleteCommand;
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(DeleteTour);

        private ICommand _editCommand;
        public ICommand EditCommand => _editCommand ??= new RelayCommand(UpdateTour);

        private ICommand _pdfCommand;
        public ICommand PDFCommand => _pdfCommand ??= new RelayCommand(PrintPdf);


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
            MessageBox.Show("Your report can be found in the root folder of your installation.", "PDF Generation done");
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
