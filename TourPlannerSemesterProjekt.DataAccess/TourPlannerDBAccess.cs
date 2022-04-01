using System;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using TourPlannerSemesterProjekt.Models;
using System.Data.Common;

namespace TourPlannerSemesterProjekt.DataAccess
{
    public class TourPlannerDBAccess : ITourPlannerDBAccess
    {
        //TBD: Add to App.Config
        private string _connectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;


        private const string SQL_GET_ALL_TOURS = "SELECT * FROM tour";

        private const string SQL_INSERT_TOUR = "INSERT INTO tour (\"name\", \"tourDescription\", \"toAddress\", \"fromAddress\", \"transportType\", \"routeInformation\", \"tourDistance\", \"estimatedArrTime\") VALUES (@nname, @ntourDescription, @ntoAddress, @nfromAddress, @ntransportType, @nrouteInformation, @ntourDistance, @nestimatedArrTime)";

        private static Lazy<TourPlannerDBAccess> _instance; // Singleton pattern

        public static TourPlannerDBAccess GetInstance()
        {
            _instance ??= new Lazy<TourPlannerDBAccess>(() => new TourPlannerDBAccess());

            return _instance.Value;
        }

        public List<TourObjekt> GetAllTours()
        {
            var Tours = new List<TourObjekt>();

            var conn = CreateOpenConnection();

            using var cmd = new NpgsqlCommand(SQL_GET_ALL_TOURS, conn);

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        
                        var id = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        string tourdescription = reader.GetString(2);
                        string to = reader.GetString(3);
                        string from = reader.GetString(4);
                        string transporttype = reader.GetString(5);
                        string routeinformation = reader.GetString(6);
                        double tourdistance = reader.GetDouble(7);
                        DateTime estArrival = reader.GetDateTime(8);
                        var tour = new TourObjekt(id, name, tourdescription, to, from, transporttype, routeinformation, tourdistance, estArrival);
                        Tours.Add(tour);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
            return Tours;

        }

        public void AddNewTour(TourObjekt newtour)
        {
            //var Tour = new TourObjekt();

            var conn = CreateOpenConnection();

            using var insertCommand = new NpgsqlCommand(SQL_INSERT_TOUR, conn);
            

            try
            {
                //name, tourDescription, to, from, transportType, routeInformation, tourDistance, estimatedArrTime
                insertCommand.Parameters.AddWithValue("nname", newtour.name);
                insertCommand.Parameters.AddWithValue("ntourDescription", newtour.tourDescription);
                insertCommand.Parameters.AddWithValue("ntoAddress", newtour.to);
                insertCommand.Parameters.AddWithValue("nfromAddress", newtour.from);
                insertCommand.Parameters.AddWithValue("ntransportType", newtour.transportType);
                insertCommand.Parameters.AddWithValue("nrouteInformation", newtour.routeInformation);
                insertCommand.Parameters.AddWithValue("ntourDistance", newtour.tourDistance);
                insertCommand.Parameters.AddWithValue("nestimatedArrTime", newtour.estimatedTime);

                insertCommand.Prepare();

                insertCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                Debug.WriteLine("NpgsqlException Error Message ex.Message: " + ex.Message);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }


        //Helper Functions

        private NpgsqlConnection CreateOpenConnection()
        {
            NpgsqlConnection connection = new NpgsqlConnection(this._connectionString);
            connection.Open();

            return connection;
        }
    }
}
