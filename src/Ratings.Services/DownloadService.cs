﻿using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ratings.Services
{
    public class DownloadService : IDownloadService
    {
        public const string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/76.0.3809.100 Safari/537.36";
        
        /// <summary>
        /// Download multiple pages in parallel
        /// </summary>
        /// <param name="urls"></param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> DownloadWebsitesParallelAsync(IEnumerable<string> urls)
        {
            //TODO: Hadle bad search requests

            var tasks = new List<Task<string>>();

            foreach (var url in urls)
            {
                tasks.Add(DownloadWebsiteAsync(url));
            }

            return await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Download content of a web page
        /// </summary>
        /// <param name="url"></param>
        /// <returns>HTML content of requested URL</returns>
        private async Task<string> DownloadWebsiteAsync(string url)
        {
            //TODO: Handle web exceptions

            using (WebClient client = new WebClient())
            {
                client.Headers["User-Agent"] = userAgent;
                return await client.DownloadStringTaskAsync(url);
            }
        }
    }
}
