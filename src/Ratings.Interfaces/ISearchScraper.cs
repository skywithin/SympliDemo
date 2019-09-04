using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface ISearchScraper
    {
        /// <summary>
        /// Query search engine for required key words
        /// </summary>
        /// <param name="keyWords">List of key words</param>
        /// <param name="maxSearchResults">Total number of search results to get</param>
        /// <returns>Pieces of html representing search results</returns>
        Task<IEnumerable<string>> Search(IEnumerable<string> keyWords, int maxSearchResults = 100);
    }
}
