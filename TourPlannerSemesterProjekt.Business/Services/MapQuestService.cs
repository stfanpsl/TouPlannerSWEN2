using System;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TourPlannerSemesterProjekt.Business.Services
{
    public class MapQuestService
    {
        private readonly string _baseUrl;
        private readonly HttpClient _client;
        private readonly string _apiKey;
        private readonly string _filePath;
        private JObject _routeData;

        public MapQuestService(string fromLocation, string toLocation)
        {
            _baseUrl = "https://www.mapquestapi.com";
            _client = new HttpClient();
            _apiKey = ConfigurationManager.AppSettings["MapQuestKey"];
            _filePath = ConfigurationManager.AppSettings["ImagePath"];
            _routeData = SaveRouteInformation(fromLocation, toLocation);
        }

        private JObject SaveRouteInformation(string fromLocation, string toLocation)
        {
            if (DoesLocationExist(fromLocation) && DoesLocationExist(toLocation))
            {
                var url = _baseUrl + "/directions/v2/route?key=" + _apiKey + "&from=" + fromLocation + "&to=" + toLocation + "&unit=k";
                using (WebClient client = new WebClient())
                {
                    JObject jSonResponse = JObject.Parse(client.DownloadString(url));
                    return jSonResponse;
                }
            }

            return null;
        }

        public double GetRouteDistance()
        {
            if (_routeData != null)
            {
                return (double)_routeData["route"]["distance"];
            }

            return -1;
        }

        public string LoadImage()
        {
            if (_routeData != null)
            {
                string session = (string)_routeData["route"]["sessionId"];
                string lrLng = (string)_routeData["route"]["boundingBox"]["lr"]["lng"];
                string lrLat = (string)_routeData["route"]["boundingBox"]["lr"]["lat"];
                string ulLng = (string)_routeData["route"]["boundingBox"]["ul"]["lng"];
                string ulLat = (string)_routeData["route"]["boundingBox"]["ul"]["lat"];

                var url = _baseUrl + "/staticmap/v5/map?key=" + _apiKey + "&size=600,600" + "&session=" + session + "&boundingBox=" + ulLat + "," + ulLng + "," + lrLat + "," + lrLng;
                var fileName = System.IO.Path.GetRandomFileName() + ".jpg";
                var fullFilePath = _filePath + fileName;
                using (WebClient client = new WebClient())
                {
                    var data = client.DownloadData(url);
                    using (var ms = new MemoryStream(data))
                    {
                        using (var image = Image.FromStream(ms))
                        {
                            var i2 = new Bitmap(image);
                            i2.Save(fullFilePath, ImageFormat.Jpeg);
                        }
                    }
                }
                return fullFilePath;
            }
            return "";

        }


        public bool DoesLocationExist(string location)
        {
            var task = Task.Run(() => _client.GetAsync(_baseUrl + "/geocoding/v1/address?key=" + _apiKey + "&location=" + location));
            task.Wait();

            var stringJsonResponse = task.Result.Content.ReadAsStringAsync().Result;

            JObject jSonResponse = JObject.Parse(stringJsonResponse);

            if (jSonResponse["results"]?[0]?["locations"]?.Count() > 1)
            {
                return true;
            }

            return false;
        }
    }
}
