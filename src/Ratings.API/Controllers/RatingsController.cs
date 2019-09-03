using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ratings.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<int>> GeRatings()
        {
            return Ok(new int[] { 1, 2, 3 });
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> GetValue(int id)
        {
            var value = string.Format($"Your value is {id}");
            return Ok(value);
        }
    }
}
