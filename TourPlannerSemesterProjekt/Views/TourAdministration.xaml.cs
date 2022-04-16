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
using TourPlannerSemesterProjekt.ViewModels;
using TourPlannerSemesterProjekt.Models;

namespace TourPlannerSemesterProjekt
{
    /// <summary>
    /// Interaktionslogik für TourAdministration.xaml
    /// </summary>
    public partial class TourAdministration : Window
    {
        public TourAdministration(MainWindowViewModel mainView)
        {
            InitializeComponent();
            this.DataContext = new TourAdministrationViewModel(this, mainView);
        }

        public TourAdministration(MainWindowViewModel mainView, TourObjekt tour)
        {
            InitializeComponent();
            this.DataContext = new TourAdministrationViewModel(this, mainView, tour);
        }

        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
