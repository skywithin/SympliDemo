using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class RatingService : IRatingService
    {
        ISearchScraper _searchScraper;


        public RatingService(ISearchScraper searchScraper)
        {
            _searchScraper = searchScraper;
        }

        /// <summary>
        /// Find all positions of a search item 
        /// </summary>
        /// <param name="keyWords"></param>
        /// <param name="searchItem"></param>
        /// <param name="maxSearchResults"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> GetRatings(
            IEnumerable<string> keyWords,
            string searchItem,
            int maxSearchResults = 100)
        {
            var searchResultItems = await _searchScraper.Search(keyWords, maxSearchResults);
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
