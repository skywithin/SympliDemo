using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface IRatingService
    {
        /// <summary>
        /// Find all positions of a search item
        /// </summary>
        /// <param name="keyWords">List of key words to search for</param>
        /// <param name="searchItem">Search criteria (e.g. website URL)</param>
        /// <param name="type">Search engine type</param>
        /// <param name="maxSearchResults"></param>
        /// <returns>An array of numbers represnting positions of serch results matching your criteria</returns>
        Task<IEnumerable<int>> GetRatings(
            IEnumerable<string> keyWords,
            string searchItem,
            SearchEngineType type,
            int maxSearchResults = 100);
    }
}
