using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text.Json;
using System.Runtime.Serialization.Json;
using System.Net;
using System.IO;

namespace DNWS
{
    class PM25Plugin : IPlugin
    {
        private static readonly HttpClient client = new HttpClient();
        protected static PM25Reading pm25reading = null;

        public PM25Plugin()
        {
            if (pm25reading == null) {
                RequestAqiCn();
            }
        }

        public void RequestAqiCn()
        {
            HttpWebRequest http = (HttpWebRequest) WebRequest.Create("http://api.waqi.info/feed/shanghai/?token=demo");
            HttpWebResponse response = (HttpWebResponse) http.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            string content = sr.ReadToEnd();
            pm25reading = JsonSerializer.Deserialize<PM25Reading>(content);
        }
        
        public void PreProcessing(HTTPRequest request)
        {
            throw new NotImplementedException();
        }

        public HTTPResponse GetResponse(HTTPRequest request)
        {
            if (pm25reading == null) {
                RequestAqiCn();
            } else {
                DateTime now = DateTime.Now;
                TimeSpan diff = now.Subtract(pm25reading.data.time.iso.DateTime);
                if (diff >= new TimeSpan(1, 0, 0)) {
                    RequestAqiCn();
                }
            }

            Console.WriteLine(pm25reading.data.city.name);
            Console.WriteLine(pm25reading.data.time.iso);
            Console.WriteLine(pm25reading.data.iaqi.pm25.v);
            Console.WriteLine(pm25reading.data.iaqi.pm10.v);


            
            HTTPResponse response = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("<html><body>");
            sb.Append("<header><meta http-equiv='Content-Type' content='text/html; charset=utf-8'/></hearder>");
            sb.Append("Location : " + pm25reading.data.city.name + "<br />");
            sb.Append("Date/Time : " + pm25reading.data.time.iso + "<br />");
            sb.Append("PM25 : " + pm25reading.data.iaqi.pm25.v + "<br />");
            sb.Append("PM10 : " + pm25reading.data.iaqi.pm10.v + "<br />");
            sb.Append("</body></html>");
            
            //Here HW3
            // Encode the json.
            String reading = JsonSerializer.Serialize(pm25reading);    
            response = new HTTPResponse(200);
            response.body = Encoding.UTF8.GetBytes(reading);
            return response;
        }

        public HTTPResponse PostProcessing(HTTPResponse response)
        {
            throw new NotImplementedException();
        }
    }
}
