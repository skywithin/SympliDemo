using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class BingSearchScraper : BaseSearchScraper
    {
        private const int SearchResultsPerPage = 10;
        private const string BingResultsParserRegex = "<li\\sclass=\\\"b_algo\\\"[^>]*>(.*?)</li>";
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
        public override async Task<IEnumerable<string>> Search(
            IEnumerable<string> keyWords,
            int maxSearchResults = 100)
        {
            //TODO: Validate key words

            var searchUrls = GetSearchRequestUrls(keyWords, maxSearchResults);
            var websitesHtmlContent = await _downloadService.DownloadWebsitesParallelAsync(searchUrls);
            return GetAllSearchResultItems(websitesHtmlContent);
        }

        /// <summary>
        /// Generate search request URLs for required key words
        /// </summary>
        /// <param name="keyWords">List of key words to search</param>
        /// <param name="maxSearchResults">Max number of results</param>
        /// <returns></returns>
        public IEnumerable<string> GetSearchRequestUrls(
            IEnumerable<string> keyWords,
            int maxSearchResults = 100)
        {
            var output = new List<string>();
            var numberOfSearchRequests = GetNumberOfSearchRequests(SearchResultsPerPage, maxSearchResults);
            var keyPhrase = GetSearchPhrase(keyWords);

            for (int requestIndex = 0; requestIndex < numberOfSearchRequests; requestIndex++)
            {
                int first = requestIndex * SearchResultsPerPage + 1;
                output.Add(string.Format($"https://www.bing.com/search?q={keyPhrase}&first={first}"));
            }

            return output;
        }

        public IEnumerable<string> GetAllSearchResultItems(IEnumerable<string> websitesHtmlContent)
        {
            var output = new List<string>();

            foreach (var content in websitesHtmlContent)
            {
                output.AddRange(ParseBingSearchPageHtml(content));
            }

            return output;
        }

        /// <summary>
        /// Find search result divs in website html content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns>Collection of search results. </returns>
        private IEnumerable<string> ParseBingSearchPageHtml(string pageContent)
        {
            var output = new List<string>();
            var matches = Regex.Matches(pageContent, BingResultsParserRegex);

            foreach (Match match in matches)
            {
                foreach (Capture capture in match.Captures)
                {
                    output.Add(capture.Value);
                }
            }

            return output;
        }
    }
}
