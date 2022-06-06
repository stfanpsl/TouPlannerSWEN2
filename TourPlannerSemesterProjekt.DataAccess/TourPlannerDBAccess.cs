using Microsoft.Extensions.Configuration;
using Npgsql;
using TourPlannerSemesterProjekt.Logging;
using TourPlannerSemesterProjekt.Models;


namespace TourPlannerSemesterProjekt.DataAccess
{
    public class TourPlannerDBAccess : ITourPlannerDBAccess
    {
        private const string SQL_GET_ALL_TOURS = "SELECT * FROM tour";

        private const string SQL_SEARCH_TOURS = "SELECT * FROM tour WHERE LOWER(\"name\") LIKE LOWER(@nname)";

        private const string SQL_GET_TOURLOGS = "SELECT * FROM tourlogs WHERE \"l_tour\" = @l_tour";

        private const string SQL_SEARCH_TOURLOGS = "SELECT * FROM tourlogs WHERE LOWER(\"l_comment\") LIKE LOWER(@nl_comment)";

        private const string SQL_INSERT_TOUR = "INSERT INTO tour (\"name\", \"tourDescription\", \"toAddress\", \"fromAddress\", \"transportType\", \"routeInformation\", \"tourDistance\", \"estimatedArrTime\", \"filePath\", \"caloriefuel\") VALUES (@nname, @ntourDescription, @ntoAddress, @nfromAddress, @ntransportType, @nrouteInformation, @ntourDistance, @nestimatedArrTime, @nfilePath, @ncaloriefuel) RETURNING id";

        private const string SQL_UPDATE_TOUR = "UPDATE tour SET \"name\" = @nname, \"tourDescription\" = @ntourDescription, \"toAddress\" = @ntoAddress, \"fromAddress\" = @nfromAddress, \"transportType\" = @ntransportType, \"routeInformation\" = @nrouteInformation, \"tourDistance\" =@ntourDistance , \"estimatedArrTime\" = @nestimatedArrTime, \"caloriefuel\" = @ncaloriefuel WHERE id = @id";

        private const string SQL_DELETE_TOUR = "DELETE FROM tour WHERE \"id\" = @id";

        private const string SQL_INSERT_TOURLOG = "INSERT INTO tourlogs (\"l_date\", \"l_comment\", \"l_difficulty\", \"l_totaltime\", \"l_rating\", \"l_tour\") VALUES (@nl_date, @nl_comment, @nl_difficulty, @nl_totaltime, @nl_rating, @nl_tour)";

        private const string SQL_UPDATE_TOURLOG = "UPDATE tourlogs SET \"l_date\" = @nl_date, \"l_comment\" = @nl_comment, \"l_difficulty\" = @nl_difficulty, \"l_totaltime\" = @nl_totaltime, \"l_rating\" = @nl_rating WHERE l_id = @l_id";

        private const string SQL_DELETE_TOURLOG = "DELETE FROM tourlogs WHERE \"l_id\" = @l_id";


        private static Lazy<TourPlannerDBAccess> _instance;

        private static ILoggerWrapper logger = LoggerFactory.GetLogger();

        public static TourPlannerDBAccess GetInstance()
        {

            _instance ??= new Lazy<TourPlannerDBAccess>(() => new TourPlannerDBAccess());

            return _instance.Value;


        }

        public List<TourObjekt> GetTours(string searchText = "")
        {
            var Tours = new List<TourObjekt>();

            var conn = CreateOpenConnection();

            using var cmd = new NpgsqlCommand(SQL_GET_ALL_TOURS, conn);

            if (searchText != "")
            {
                cmd.CommandText = SQL_SEARCH_TOURS;
                cmd.Parameters.AddWithValue("nname", ("%" + searchText + "%"));
            }

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
                        string from = reader.GetString(3);
                        string to = reader.GetString(4);
                        string transporttype = reader.GetString(5);
                        string routeinformation = reader.GetString(6);
                        double tourdistance = reader.GetDouble(7);
                        string filepath = reader.GetString(8);
                        string estArrival = reader.GetString(9);
                        double caloriefuel = reader.GetDouble(10);
                        var tour = new TourObjekt(id, name, tourdescription, to, from, transporttype, routeinformation, tourdistance, estArrival, filepath);
                        tour.caloriefuel = caloriefuel;
                        Tours.Add(tour);
                    }
                }
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tours could not be loaded from DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
            return Tours;

        }

        public List<TourLogObjekt> GetTourLogs(TourObjekt tour, string searchText = "")
        {
            var TourLogs = new List<TourLogObjekt>();

            var conn = CreateOpenConnection();

            using var cmd = new NpgsqlCommand(SQL_GET_TOURLOGS, conn);

            cmd.Parameters.AddWithValue("l_tour", tour.id);

            if (searchText != "")
            {
                cmd.CommandText = SQL_SEARCH_TOURLOGS;
                cmd.Parameters.AddWithValue("nl_comment", ("%" + searchText + "%"));
            }

            try
            {
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var id = reader.GetInt32(0);
                        DateTime date = reader.GetDateTime(1);
                        string comment = reader.GetString(2);
                        string difficulty = reader.GetString(3);
                        string totaltime = reader.GetString(4);
                        int rating = reader.GetInt32(5);
                        int tourreference = reader.GetInt32(6);
                        var tourlog = new TourLogObjekt(id, date, comment, difficulty, totaltime, rating, tourreference);
                        TourLogs.Add(tourlog);
                    }
                }
            }

            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour-Logs could not be loaded from DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
            return TourLogs;

        }


        public int AddNewTour(TourObjekt newtour)
        {

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
                insertCommand.Parameters.AddWithValue("nfilePath", newtour.imagePath);
                insertCommand.Parameters.AddWithValue("ncaloriefuel", newtour.caloriefuel);


                insertCommand.Prepare();

                int res = (int)insertCommand.ExecuteScalar();
                return res;
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtour.name + "'could not be inserted into DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }

        public void EditTour(TourObjekt newtour)
        {

            var conn = CreateOpenConnection();

            using var updateCommand = new NpgsqlCommand(SQL_UPDATE_TOUR, conn);


            try
            {
                //name, tourDescription, to, from, transportType, routeInformation, tourDistance, estimatedArrTime
                updateCommand.Parameters.AddWithValue("nname", newtour.name);
                updateCommand.Parameters.AddWithValue("ntourDescription", newtour.tourDescription);
                updateCommand.Parameters.AddWithValue("ntoAddress", newtour.to);
                updateCommand.Parameters.AddWithValue("nfromAddress", newtour.from);
                updateCommand.Parameters.AddWithValue("ntransportType", newtour.transportType);
                updateCommand.Parameters.AddWithValue("nrouteInformation", newtour.routeInformation);
                updateCommand.Parameters.AddWithValue("ntourDistance", newtour.tourDistance);
                updateCommand.Parameters.AddWithValue("nestimatedArrTime", newtour.estimatedTime);
                updateCommand.Parameters.AddWithValue("ncaloriefuel", newtour.caloriefuel);

                updateCommand.Parameters.AddWithValue("id", newtour.id);

                updateCommand.Prepare();

                updateCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtour.name + "'could not be edited in DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }


        public void DeleteTour(TourObjekt newtour)
        {

            var conn = CreateOpenConnection();

            using var removeCommand = new NpgsqlCommand(SQL_DELETE_TOUR, conn);


            try
            {
                //name, tourDescription, to, from, transportType, routeInformation, tourDistance, estimatedArrTime
                removeCommand.Parameters.AddWithValue("id", newtour.id);

                removeCommand.Prepare();

                removeCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtour.name + "'could not be deleted in DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }

        public void AddNewTourLog(TourLogObjekt newtourlog)
        {

            var conn = CreateOpenConnection();

            using var insertCommand = new NpgsqlCommand(SQL_INSERT_TOURLOG, conn);


            try
            {
                insertCommand.Parameters.AddWithValue("nl_date", newtourlog.l_date);
                insertCommand.Parameters.AddWithValue("nl_comment", newtourlog.l_comment);
                insertCommand.Parameters.AddWithValue("nl_difficulty", newtourlog.l_difficulty);
                insertCommand.Parameters.AddWithValue("nl_rating", newtourlog.l_rating);
                insertCommand.Parameters.AddWithValue("nl_totaltime", newtourlog.l_totaltime);
                insertCommand.Parameters.AddWithValue("nl_tour", newtourlog.l_tour);

                insertCommand.Prepare();

                insertCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtourlog.l_date.ToShortDateString() + "'could not be inserted into DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }

        public void EditTourLog(TourLogObjekt newtourlog)
        {

            var conn = CreateOpenConnection();

            using var updateCommand = new NpgsqlCommand(SQL_UPDATE_TOURLOG, conn);


            try
            {
                updateCommand.Parameters.AddWithValue("nl_date", newtourlog.l_date);
                updateCommand.Parameters.AddWithValue("nl_comment", newtourlog.l_comment);
                updateCommand.Parameters.AddWithValue("nl_difficulty", newtourlog.l_difficulty);
                updateCommand.Parameters.AddWithValue("nl_rating", newtourlog.l_rating);
                updateCommand.Parameters.AddWithValue("nl_totaltime", newtourlog.l_totaltime);

                updateCommand.Parameters.AddWithValue("l_id", newtourlog.l_id);

                updateCommand.Prepare();

                updateCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtourlog.l_date.ToShortDateString() + "'could not be edited in DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }


        public void DeleteTourLog(TourLogObjekt newtourlog)
        {

            var conn = CreateOpenConnection();

            using var removeCommand = new NpgsqlCommand(SQL_DELETE_TOURLOG, conn);


            try
            {
                removeCommand.Parameters.AddWithValue("id", newtourlog.l_id);

                removeCommand.Prepare();

                removeCommand.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                logger.Fatal("Tour: '" + newtourlog.l_date.ToShortDateString() + "'could not be deleted in DB: " + ex);
                throw new NpgsqlException("Error in database occurred.", ex);
            }

            conn.Close();
        }


        //Helper Functions

        private NpgsqlConnection CreateOpenConnection()
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();

            string connectionString = config["connectionString"];

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            connection.Open();

            return connection;
        }
    }
}
