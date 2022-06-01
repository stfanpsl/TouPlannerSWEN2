using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerSemesterProjekt.Models
{
    public class TourLogObjekt
    {

        public int l_id { get; set; }
        public DateTime l_date { get; set; }
        public string l_comment { get; set; }
        public string l_difficulty { get; set; }
        public string l_totaltime { get; set; }
        public int l_rating { get; set; }
        public int l_tour { get; set; }

        public TourLogObjekt(int l_id,
                          DateTime l_date,
                          string l_comment,
                          string l_difficulty,
                          string l_totaltime,
                          int l_rating,
                          int l_tour)
        {
            this.l_id = l_id;
            this.l_date = l_date;
            this.l_comment = l_comment;
            this.l_difficulty = l_difficulty;
            this.l_totaltime = l_totaltime;
            this.l_rating = l_rating;
            this.l_tour = l_tour;
        }

        public TourLogObjekt(DateTime l_date,
                          string l_comment,
                          string l_difficulty,
                          string l_totaltime,
                          int l_rating)
        {
            this.l_date = l_date;
            this.l_comment = l_comment;
            this.l_difficulty = l_difficulty;
            this.l_totaltime = l_totaltime;
            this.l_rating = l_rating;
        }

        public TourLogObjekt(DateTime l_date,
                  string l_comment,
                  string l_difficulty,
                  string l_totaltime,
                  int l_rating,
                  int l_tour)
        {
            this.l_date = l_date;
            this.l_comment = l_comment;
            this.l_difficulty = l_difficulty;
            this.l_totaltime = l_totaltime;
            this.l_rating = l_rating;
            this.l_tour = l_tour;
        }

        public TourLogObjekt()
        {

        }

    }
}
