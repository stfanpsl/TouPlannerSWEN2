using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            Firstname = "Bester";
            Lastname = "Mensch";
        }

        //public string firstname { get; set; }

        string firstname;
        public string Firstname{ get => firstname;
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (!string.IsNullOrEmpty(propertyName))
            {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
