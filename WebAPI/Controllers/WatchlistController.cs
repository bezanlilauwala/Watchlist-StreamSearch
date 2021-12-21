using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI3.Models;
using WebAPI3.ViewModels;
using Newtonsoft.Json.Linq;
using WebAPI3.Utility;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace WebAPI3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WatchlistController : ControllerBase
    {
        private readonly WatchlistDBContext _context;

        public WatchlistController(WatchlistDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { test = "this is the test string" });
        }
        
        [HttpDelete]
        [Authorize]
        [Route("DeleteTitle")]
        public async Task<IActionResult> DeleteTitle(int watchlistId, int titleId)
        {
            List<WatchlistTitle> watchlistTitles = await _context.WatchlistTitles.Where(wt => wt.WatchlistId == watchlistId && wt.TitleId == titleId).ToListAsync();
            _context.WatchlistTitles.Remove(watchlistTitles[0]);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpDelete]
        [Authorize]
        [Route("Delete")]
        public async Task<IActionResult> DeleteWatchlist(int watchlistId)
        {
            List<Watchlist> watchlists = await _context.Watchlists.Where(w => w.Id == watchlistId).ToListAsync();
            _context.Watchlists.Remove(watchlists[0]);

            //Delete all WatchlistTitle entries with Id == watchlistId
            List<WatchlistTitle> watchlistTitles = await _context.WatchlistTitles.Where(wt => wt.WatchlistId == watchlistId).ToListAsync();
            _context.WatchlistTitles.RemoveRange(watchlistTitles);

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut]
        [Authorize]
        [Route("Checked")]
        public async Task<IActionResult> PutWatched(int watchlistId, int titleId)
        {
            List<WatchlistTitle> watchlistTitle =  await _context.WatchlistTitles.Where(wt => wt.WatchlistId == watchlistId && wt.TitleId == titleId).ToListAsync();
            watchlistTitle[0].Watched = !watchlistTitle[0].Watched;
            _context.WatchlistTitles.Update(watchlistTitle[0]);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        [Authorize]
        [Route("WatchlistTitles")]
        public async Task<List<FullTitleViewModel>> GetTitles(int watchlistId)
        {
            List<FullTitleViewModel> fullTitleInfoList = new List<FullTitleViewModel>();
            List<WatchlistTitle> watchlistTitles = await _context.WatchlistTitles.Where(wt => wt.WatchlistId == watchlistId).ToListAsync();

            for (int i = 0; i < watchlistTitles.Count; i++) {
                List<Title> titles = await _context.Titles.Where(t => t.Id == watchlistTitles[i].TitleId).ToListAsync();
                Title title = titles[0];

                FullTitleViewModel fullTitle = new FullTitleViewModel();
                fullTitle.Id = title.Id.ToString();
                fullTitle.ImdbId = title.ImdbId;
                fullTitle.Name = title.Name;
                fullTitle.ImdbRating = title.ImdbRating;
                fullTitle.ReleaseDate = title.ReleaseDate;
                fullTitle.Runtime = title.Runtime;
                fullTitle.RuntimeStr = title.RuntimeStr;
                fullTitle.Genres = title.Genres;
                fullTitle.Type = title.Type;
                fullTitle.Image = title.Image;
                fullTitle.Plot = title.Plot;
                List<WatchlistTitle> watchlistTitle = await _context.WatchlistTitles.Where(wt => wt.WatchlistId == watchlistId && wt.TitleId == title.Id).ToListAsync();
                fullTitle.Watched = watchlistTitle[0].Watched;
                fullTitleInfoList.Add(fullTitle);
            }

            return fullTitleInfoList;
        }


        [HttpPost]
        [Authorize]
        [Route("Title")]
        //Adds the new title to the WatchlistTitle table and to the Title table if it has not yet been added to it
        public async Task<IActionResult> Post(string imdbId, string watchlistId)
        {
            //Add the title to the Title table
            if (_context.Titles.Where(t => t.ImdbId == imdbId).Count() < 1) {
                ImdbRequest request = new ImdbRequest();
                JObject json = request.GetRequest("Title", imdbId);
                Title title = new Title();
                title.ImdbId = json["id"].ToString();
                title.Name = json["title"].ToString();
                title.Type = json["type"].ToString();
                title.Runtime = json["runtimeMins"].ToString();
                title.RuntimeStr = json["runtimeStr"].ToString();
                title.ReleaseDate = json["releaseDate"].ToString();
                title.Genres = json["genres"].ToString();
                title.ImdbRating = json["imDbRating"].ToString();
                title.Image = json["image"].ToString();
                title.Plot = json["plot"].ToString();
                _context.Titles.Add(title);
                _context.SaveChanges();
            }

            //Add the title to the WatchlistTitle table
            WatchlistTitle watchlistTitle = new WatchlistTitle();
            watchlistTitle.WatchlistId = Int32.Parse(watchlistId);
            List<Title> titles = await _context.Titles.Where(t => t.ImdbId == imdbId).ToListAsync();
            watchlistTitle.TitleId = titles[0].Id;
            await _context.WatchlistTitles.AddAsync(watchlistTitle);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                suceeded = true
            });
        }
        

        [HttpGet]
        [Authorize]
        [Route("Search")]
        public ShortTitleViewModel[] Get(string searchString)
        {
            ImdbRequest request = new ImdbRequest();
            JObject json = request.GetRequest("Search", searchString);

            JArray items = (JArray)json["results"];
            ShortTitleViewModel[] infoArray = new ShortTitleViewModel[items.Count];

            for (int i = 0; i < items.Count; i++)
            {
                ShortTitleViewModel info = new ShortTitleViewModel();
                info.ImdbId = items[i]["id"].ToString();
                info.Image = items[i]["image"].ToString();
                info.Title = items[i]["title"].ToString();
                info.Description = items[i]["description"].ToString();
                infoArray[i] = info;
            }
            
            return infoArray;
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<List<Watchlist>> Get(int id)
        {
            IQueryable<Watchlist> watchlistRows = _context.Watchlists.Where(w => w.UserId == id);
            System.Diagnostics.Debug.WriteLine(watchlistRows);
            return await watchlistRows.ToListAsync();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] WatchlistViewModel watchlist)
        {
            Watchlist newWatchlist = new Watchlist();
            newWatchlist.Name = watchlist.Name;
            newWatchlist.UserId = Int32.Parse(watchlist.Id);
            await _context.Watchlists.AddAsync(newWatchlist);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                suceeded = true
            });
        }

    }
}

