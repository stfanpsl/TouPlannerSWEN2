using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Business.Services;
using System.Windows.Input;
using System.Windows;

namespace TourPlannerSemesterProjekt.ViewModels
{
    class TourAdministrationViewModel : BaseViewModel
    {

        private TourObjekt currentItem = new TourObjekt("New Tour", "Tour Description", "To", "From", "Transport-type", "Route Information", 0, DateTime.Today);
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

        private ITourPlannerService _tourservice;

        public ICommand CreateCommand { get; set; }

        //public ICommand CloseWindowCommand { get; set; }

        public TourAdministrationViewModel()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _tourservice = new TourPlannerService(repository);

            //RaisePropertyChanged(nameof(CurrentItem));

            CreateCommand = new RelayCommand(o => {
                var newTourItem = CurrentItem;
                InsertTour(newTourItem);
            });
        }
        
        private void InsertTour(TourObjekt newTour)
        {
            _tourservice.AddNewTour(newTour);
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
