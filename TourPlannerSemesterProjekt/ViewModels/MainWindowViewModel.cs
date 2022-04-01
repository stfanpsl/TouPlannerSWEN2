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
using TourPlannerSemesterProjekt.Business.Services;

namespace TourPlannerSemesterProjekt
{
    class MainWindowViewModel : BaseViewModel
    {

        public ObservableCollection<TourObjekt> TourItems { get; set; }

        private ITourPlannerService _tourservice;

        public MainWindowViewModel()
        {
            var repository = TourPlannerDBAccess.GetInstance();
            _tourservice = new TourPlannerService(repository);
            TourItems = new ObservableCollection<TourObjekt>(_tourservice.GetAllTours());
        }

    }
}
