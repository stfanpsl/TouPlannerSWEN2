using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Drawing;
using System.Net;
using TourPlannerSemesterProjekt.DataAccess;
using TourPlannerSemesterProjekt.Logging;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class MapQuestService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _filePath;
        private JObject _routeData;

        private static ILoggerWrapper logger = LoggerFactory.GetLogger();


        public MapQuestService(string fromLocation, string toLocation)
        {
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", false, true)
            .Build();
            _baseUrl = "https://www.mapquestapi.com";
            _client = new HttpClient();
            _apiKey = config["MapQuestKey"];
            _filePath = config["ImagePath"];
            _routeData = GetRoute(fromLocation, toLocation);
        }

        private JObject GetRoute(string fromLocation, string toLocation)
        {
            var fromLocationFormatted = fromLocation.Replace(" ", "%");
            var toLocationFormatted = toLocation.Replace(" ", "%");
            var url = _baseUrl + "/directions/v2/route?key="
                    + _apiKey
                    + "&from="
                    + fromLocationFormatted
                    + "&to="
                    + toLocationFormatted
                    + "&unit=k";

            HttpClient client = new HttpClient();
            JObject jSonResponse;
            using (HttpResponseMessage response = client.GetAsync(url).Result)
            {
                using (HttpContent content = response.Content)
                {
                    jSonResponse = JObject.Parse(content.ReadAsStringAsync().Result);
                }
                return jSonResponse;
            }
            return null;
        }

        public bool CheckRoute()
        {
            if (_routeData != null)
            {
                if ((string)_routeData["route"]["formattedTime"] == "00:00:00" || (string)_routeData["route"]["routeError"]["errorCode"] == "2")
                {
                    logger.Error("Route could not be found for Session ID: " + _routeData["route"]["sessionId"]);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                logger.Error("Route could not be found for Session ID: " + _routeData["route"]["sessionId"]);
                return false;
            }
        }

        public double GetRouteDistance()
        {
            if (_routeData != null)
            {
                return (double)_routeData["route"]["distance"];
            }

            return -1;
        }

        public string GetArrivalTime()
        {
            if (_routeData != null)
            {
                return (string)_routeData["route"]["formattedTime"];
            }

            return "";
        }

        //Connect to FileAccess DAL
        public string GetImage()
        {
            if (_routeData != null)
            {
                var fileAcess = new TourPlannerFileAccess();

                string session = (string)_routeData["route"]["sessionId"];
                string lrLng = (string)_routeData["route"]["boundingBox"]["lr"]["lng"];
                string lrLat = (string)_routeData["route"]["boundingBox"]["lr"]["lat"];
                string ulLng = (string)_routeData["route"]["boundingBox"]["ul"]["lng"];
                string ulLat = (string)_routeData["route"]["boundingBox"]["ul"]["lat"];

                var url = _baseUrl + "/staticmap/v5/map?key=" + _apiKey + "&size=600,600" + "&session=" + session + "&boundingBox=" + ulLat + "," + ulLng + "," + lrLat + "," + lrLng;
                var savedFilePath = "/img/placeholder.png";
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        var data = client.DownloadData(url);
                        using (var ms = new MemoryStream(data))
                        {
                            using (var image = Image.FromStream(ms))
                            {
                                //image.Save(fullFilePath, ImageFormat.Jpeg);
                                savedFilePath = fileAcess.SaveImage(image);
                                logger.Debug("Image saved under: " + savedFilePath);
                            }
                        }
                    }
                    catch (System.Net.WebException ex)
                    {
                        logger.Error("Image could not be saved.");
                    }

                }
                return savedFilePath;
            }
            return "";

        }
    }
}
