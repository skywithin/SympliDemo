using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class RatingService : IRatingService
    {
        ISearchScraperFactory _searchScraperFactory;


        public RatingService(ISearchScraperFactory searchScraperFactory)
        {
            _searchScraperFactory = searchScraperFactory;
        }

        /// <summary>
        /// Find all positions of a search item
        /// </summary>
        /// <param name="keyWords">List of key words to search for</param>
        /// <param name="searchItem">Search criteria (e.g. website URL)</param>
        /// <param name="type">Search engine type</param>
        /// <param name="maxSearchResults"></param>
        /// <returns>An array of numbers represnting positions of serch results matching your criteria</returns>
        public async Task<IEnumerable<int>> GetRatings(
            IEnumerable<string> keyWords,
            string searchItem,
            SearchEngineType type,
            int maxSearchResults = 100)
        {
            var searchScraper = _searchScraperFactory.GetSearchScraperInstance(type);
            var searchResultItems = await searchScraper.Search(keyWords, maxSearchResults);
            return GetSearchItemPositions(searchResultItems, searchItem);
        }

        private IEnumerable<int> GetSearchItemPositions(
            IEnumerable<string> searchResultItems, 
            string searchItem)
        {
            var output = new List<int>();
            int position = 0;

            foreach (var item in searchResultItems)
            {
                position++;

                if (item.Contains(searchItem))
                {
                    output.Add(position);
                }
            }

            return output;
        }
    }
}
