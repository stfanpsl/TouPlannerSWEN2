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


namespace TourPlannerSemesterProjekt
{
    class MainWindowViewModel : BaseViewModel
    {



        public MainWindowViewModel()
        {
            Firstname = "Hallo";
            Lastname = "Welt!";
        }

        //public string firstname { get; set; }

        string firstname;
        public string Firstname
        {
            get => firstname;
            set
            {
                if (firstname != value)
                {
                    firstname = value;

                    this.RaisePropertyChanged();

                    this.RaisePropertyChanged(nameof(Fullname));
                }
            }
        }

        string lastname;
        public string Lastname
        {
            get => lastname;
            set
            {
                if (lastname != value)
                {
                    lastname = value;

                    this.RaisePropertyChanged();

                    this.RaisePropertyChanged(nameof(Fullname));
                }
            }
        }

        //public string lastname { get; set; }

        public string Fullname => $"{Firstname} {Lastname}";



    }
}
