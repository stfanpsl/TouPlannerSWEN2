using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TourPlannerSemesterProjekt.Models;
using TourPlannerSemesterProjekt.ViewModels;

namespace TourPlannerSemesterProjekt.Views
{
    /// <summary>
    /// Interaktionslogik für TourLogAdministration.xaml
    /// </summary>
    public partial class TourLogAdministration : Window
    {
        public TourLogAdministration(MainWindowViewModel mainView, int tourID)
        {
            InitializeComponent();
            this.DataContext = new TourLogAdministrationViewModel(this, mainView, tourID);
        }

        public TourLogAdministration(MainWindowViewModel mainView, TourLogObjekt tourlog)
        {
            InitializeComponent();
            this.DataContext = new TourLogAdministrationViewModel(this, mainView, tourlog);
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
