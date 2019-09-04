using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class GoogleSearchScraper : ISearchScraper
    {
        private const int SearchResultsPerPage = 10;
        private const string GoogleResultsParserRegex = "<div\\sclass=\\\"r\\\">(.*?)</div>";
        private IDownloadService _downloadService;

        public GoogleSearchScraper(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        /// <summary>
        /// Search Google for required key words
        /// </summary>
        /// <param name="keyWords">List of key words</param>
        /// <param name="maxSearchResults">Total number of search results to get</param>
        /// <returns>Pieces of html representing search results</returns>
        public async Task<IEnumerable<string>> Search(
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
            var numberOfSearchRequests = GetNumberOfSearchRequests(maxSearchResults);
            var keyPhrase = GetSearchPhrase(keyWords);

            for (int requestIndex = 0; requestIndex < numberOfSearchRequests; requestIndex++)
            {
                int start = requestIndex * SearchResultsPerPage;
                output.Add(string.Format($"https://www.google.com.au/search?q={keyPhrase}&start={start}"));
            }

            return output;
        }

        /// <summary>
        /// Calculate number of requests required to get required number of search results
        /// </summary>
        /// <param name="maxSearchResults"></param>
        /// <returns>Number of required searches. Minimum of 1 request is required</returns>
        private int GetNumberOfSearchRequests(int maxSearchResults)
        {
            var numberOfSearchRequests = maxSearchResults / SearchResultsPerPage;

            // Make sure there is at least one request
            numberOfSearchRequests = numberOfSearchRequests > 0 ? numberOfSearchRequests : 1;

            return numberOfSearchRequests;
        }

        /// <summary>
        /// Assemble search phrase from key words. All words must be separated by '+' char
        /// </summary>
        /// <param name="keyWords"></param>
        /// <returns></returns>
        private string GetSearchPhrase(IEnumerable<string> keyWords)
        {
            var sb = new StringBuilder();

            foreach (var keyWord in keyWords)
            {
                sb.Append(keyWord);
                sb.Append("+");
            }

            if (sb.Length > 0)
            {
                // Remove trailing "+"
                sb.Remove(sb.Length - 1, 1);
            }

            return sb.ToString();
        }

        public IEnumerable<string> GetAllSearchResultItems(IEnumerable<string> websitesHtmlContent)
        {
            var output = new List<string>();

            foreach (var content in websitesHtmlContent)
            {
                output.AddRange(ParseGoogleSearchPageHtml(content));
            }

            return output;
        }

        /// <summary>
        /// Find search result divs in website html content
        /// </summary>
        /// <param name="pageContent"></param>
        /// <returns>Collection of search results. </returns>
        private IEnumerable<string> ParseGoogleSearchPageHtml(string pageContent)
        {
            var output = new List<string>();
            var matches = Regex.Matches(pageContent, GoogleResultsParserRegex);

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
