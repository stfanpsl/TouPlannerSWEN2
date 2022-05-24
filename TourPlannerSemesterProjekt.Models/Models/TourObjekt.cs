using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt.Models
{
    public class TourObjekt
    {

        public int id { get; set; }
        public string name { get; set; }
        public string tourDescription { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string transportType { get; set; }
        public string routeInformation { get; set; }
        public double tourDistance { get; set; }
        public List<TourLogObjekt> tourlogs { get; set; }

        public string imagePath { get; set; }
        public string estimatedTime { get; set; }

        public TourObjekt(int id,
                          string name,
                          string tourDescription,
                          string from,
                          string to,
                          string transportType,
                          string routeInformation,
                          double tourDistance,
                          string estimatedTime,
                          string imagePath)
        {
            this.name = name;
            this.tourDescription = tourDescription;
            this.from = from;
            this.to = to;
            this.transportType = transportType;
            this.routeInformation = routeInformation;
            this.tourDistance = tourDistance;
            this.estimatedTime = estimatedTime;
            this.id = id;
            this.imagePath = imagePath;
        }

        public TourObjekt(string name,
                          string tourDescription,
                          string from,
                          string to,
                          string transportType,
                          string routeInformation)
        {
            this.name = name;
            this.tourDescription = tourDescription;
            this.from = from;
            this.to = to;
            this.transportType = transportType;
            this.routeInformation = routeInformation;
        }

        public TourObjekt()
        {

        }

    }
}
