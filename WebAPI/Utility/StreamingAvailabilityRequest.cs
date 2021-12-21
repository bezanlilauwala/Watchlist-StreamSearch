using RestSharp;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPI3.Utility
{
    public class StreamingAvailabilityRequest
    {
        public static string GetSearchResultsAsync(string query)
        {
            var client = new RestClient("https://streaming-availability.p.rapidapi.com/search/ultra?country=us&services=netflix&type=movie&order_by=imdb_rating&year_min=2000&year_max=2020&page=1&desc=false&min_imdb_rating=70&max_imdb_rating=90");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "streaming-availability.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "a412f8ce24msh8a549cea097c93ap1609c6jsn64d13db83b0d");
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
