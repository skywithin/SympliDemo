using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ratings.Interfaces;

namespace Ratings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<int>>> GetRatings(
            [FromQuery] string[] keyWords,
            [FromQuery] string searchItem,
            [FromQuery] string searchEngine,
            [FromQuery] int maxSearchResults = 100)
        {
            var searchEngineType = Enum.Parse<SearchEngineType>(
                searchEngine, 
                ignoreCase: true);

            var output = await _ratingService.GetRatings(
                keyWords,
                searchItem,
                searchEngineType,
                maxSearchResults);

            return Ok(output);
        }


        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Index()
        //{
        //    var output = new List<string>();
        //    output.Add(string.Format($"Now: {DateTime.Now.ToLongTimeString()}"));
        //    output.Add("Try this URL: http://localhost:5000/api/ratings?keywords=e-settlements&searchItem=www.sympli.com.au&searchEngine=Google&maxSearchResults=100");
        //    return Ok(output);
        //}
    }
}
