using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI3.Utility;

namespace WebAPI3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StreamController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public string Get(string Country, string Services, string Type, string OrderBy, string MinYear, string MaxYear, string Page, string Genres, string GenresRelation, string Language, string MinRating, string MaxRating)
        {
            //Required Parameters
            string query = $"country={Country}&services={Services}&type={Type}&order_by={OrderBy}&page={Page}";
            //Optional Parameters
            if (MinYear != "") query += $"&year_min={MinYear}";
            if (MaxYear != "") query += $"&year_max={MaxYear}";
            if (Genres != "") query += $"&genres={Genres}";
            if (GenresRelation != "") query += $"&genres_relation={GenresRelation}";
            if (Language != "") query += $"&genres_relation={GenresRelation}";
            if (MinRating != "") query += $"&min_imdb_rating={MinRating}";
            if (MaxRating != "") query += $"&max_imdb_rating={MaxRating}";

            string result = StreamingAvailabilityRequest.GetSearchResultsAsync(query).ToString();
            return result;
        }
    }
}
