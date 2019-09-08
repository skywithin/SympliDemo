using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public abstract class BaseSearchScraper : ISearchScraper
    {
        public abstract Task<IEnumerable<string>> Search(
            IEnumerable<string> keyWords, 
            int maxSearchResults = 100);

        /// <summary>
        /// Calculate number of requests required to get required number of search results
        /// </summary>
        /// <param name="searchResultsPerPage">Number of search results displayed on a page</param>
        /// <param name="maxSearchResults">Total number of search results required</param>
        /// <returns>Number of required searches. Minimum of 1 request is required</returns>
        protected int GetNumberOfSearchRequests(int searchResultsPerPage, int maxSearchResults)
        {
            var numberOfSearchRequests = maxSearchResults / searchResultsPerPage;

            // Make sure there is at least one request
            numberOfSearchRequests = numberOfSearchRequests > 0 ? numberOfSearchRequests : 1;

            return numberOfSearchRequests;
        }

        /// <summary>
        /// Assemble search phrase from key words. All words must be separated by '+' char
        /// </summary>
        /// <param name="keyWords">Collection of key words</param>
        /// <returns>Concatenated key words ready to search</returns>
        protected string GetSearchPhrase(IEnumerable<string> keyWords)
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
    }
}
