using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt.Models
{
    public class TourObjekt
    {
        private string v1;
        private string v2;
        private string v3;
        private string v4;
        private string v5;
        private string v6;
        private int v7;
        private object p;

        public int id { get; set; }
        public string name { get; set; }
        public string tourDescription { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public string transportType { get; set; }
        public string routeInformation { get; set; }
        public double tourDistance { get; set; }

        public string imagePath { get; set; }
        public DateTime estimatedTime { get; set; }

        public TourObjekt(int id,
                          string name,
                          string tourDescription,
                          string to,
                          string from,
                          string transportType,
                          string routeInformation,
                          double tourDistance,
                          DateTime estimatedTime,
                          string imagePath)
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
            this.imagePath = imagePath;
        }

        public TourObjekt(string name,
                          string tourDescription,
                          string to,
                          string from,
                          string transportType,
                          string routeInformation)
        {
            this.name = name;
            this.tourDescription = tourDescription;
            this.to = to;
            this.from = from;
            this.transportType = transportType;
            this.routeInformation = routeInformation;
        }

        public TourObjekt()
        {

        }

    }
}
