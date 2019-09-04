using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface IRatingService
    {
        Task<IEnumerable<int>> GetRatings(
            IEnumerable<string> keyWords,
            string searchItem,
            SearchEngineType type,
            int maxSearchResults = 100);
    }
}
