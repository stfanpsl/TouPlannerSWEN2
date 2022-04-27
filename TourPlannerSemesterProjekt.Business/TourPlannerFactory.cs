using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt.Business
{
    public static class TourPlannerFactory
    {
        private static ITourPlannerFactory _instance;
        public static ITourPlannerFactory GetInstance()
        {
            if (_instance == null)
            {
                _instance = new TourPlannerFactoryImpl();
            }
            return _instance;
        }
    }
}
