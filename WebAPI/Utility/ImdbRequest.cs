using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI3.Utility
{
    public class ImdbRequest
    {
        public ImdbRequest()
        { }

        public JObject GetRequest(string type, string searchString)
        {
            string url = "https://imdb-api.com/en/API/" + type + "/k_7q1n2xyl/" + searchString;
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = request.GetResponse();
            string result;

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }

            return JObject.Parse(result);
        }
    }
}
