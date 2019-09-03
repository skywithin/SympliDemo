using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ratings.Interfaces
{
    public interface IDownloadService
    {
        Task<IEnumerable<string>> DownloadWebsitesParallelAsync(IEnumerable<string> urls);
    }
}
