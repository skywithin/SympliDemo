using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface ISearchScraper
    {
        Task<IEnumerable<string>> Search(IEnumerable<string> keyWords, int maxSearchResults = 100);
    }
}
