using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt.Models
{
    internal class TourObjekt
    {
        string name { get; set; }
        string tourDescription { get; set; }
        string to { get; set; }
        string from { get; set; }
        string transportType { get; set; }
        string routeInformation { get; set; }
        double tourDistance { get; set; }
        DateTime estimatedTime { get; set; }
        int id { get; set; }

        public TourObjekt(string name,
                          string tourDescription,
                          string to,
                          string from,
                          string transportType,
                          string routeInformation,
                          double tourDistance,
                          DateTime estimatedTime,
                          int id)
        {
            this.name = name;
            this.tourDescription = tourDescription;
            this.to = to;
            this.from = from;
            this.transportType = transportType;
            this.routeInformation = routeInformation;
            this.tourDistance = tourDistance;
            this.estimatedTime = estimatedTime;
            this.id = id;
        }

        public TourObjekt()
        {

        }
    }
}
