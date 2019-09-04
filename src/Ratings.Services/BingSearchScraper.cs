using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class BingSearchScraper : ISearchScraper
    {
        private IDownloadService _downloadService;

        public BingSearchScraper(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        /// <summary>
        /// Search Bing for required key words
        /// </summary>
        /// <param name="keyWords">List of key words</param>
        /// <param name="maxSearchResults">Total number of search results to get</param>
        /// <returns>Pieces of html representing search results</returns>
        public async Task<IEnumerable<string>> Search(
            IEnumerable<string> keyWords,
            int maxSearchResults = 100)
        {
            //TODO: Implement Bing Scraper
            throw new NotImplementedException();
        }
    }
}
