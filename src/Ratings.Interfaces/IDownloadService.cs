using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface IDownloadService
    {
        /// <summary>
        /// Download multiple pages in parallel
        /// </summary>
        /// <param name="urls">List of URLs to download</param>
        /// <returns>Collection of HTML contents of requested URLs</returns>
        Task<IEnumerable<string>> DownloadWebsitesParallelAsync(IEnumerable<string> urls);
    }
}
