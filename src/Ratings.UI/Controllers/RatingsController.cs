using Ratings.Interfaces;
using Ratings.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;

namespace Ratings.UI.Controllers
{
    public class RatingsController : Controller
    {
        private IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        // GET: Ratings
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(RatingsInputModel model)
        {
            SearchEngineType searchEngineType;

            Enum.TryParse<SearchEngineType>(
                model.SearchEngine,
                ignoreCase: true,
                result: out searchEngineType);

            var cachedOutput = HttpContext.Cache.Get(model.GetCacheItemKey());

            if (cachedOutput != null)
            {
                return View(cachedOutput);
            }

            var output = await _ratingService.GetRatings(
                new string[] { model.KeyWord },
                model.SearchItem,
                searchEngineType,
                model.MaxSearchResults);
            
            //TODO: Implement proper cache manager
            HttpContext.Cache.Insert(
                model.GetCacheItemKey(), 
                output, 
                null, 
                DateTime.UtcNow.AddHours(1), 
                Cache.NoSlidingExpiration);

            return View(output);
        }

        
    }
}