using Ratings.Interfaces;
using System;

namespace Ratings.Services
{
    public class SearchScraperFactory : ISearchScraperFactory
    {
        private IDownloadService _downloadService;

        public SearchScraperFactory(IDownloadService downloadService)
        {
            _downloadService = downloadService;
        }

        public ISearchScraper GetSearchScraperInstance(SearchEngineType type)
        {
            switch (type)
            {
                case SearchEngineType.Google:
                    return new GoogleSearchScraper(_downloadService);

                case SearchEngineType.Bing:
                    return new BingSearchScraper(_downloadService);

                case SearchEngineType.Unknown:
                    throw new ApplicationException(
                        string.Format(
                            $"{type} is not a valid scraper type "));
                default:
                    throw new NotImplementedException(
                        string.Format(
                            $"Scraper of type {type} has not been implented"));
            }
        }
    }
}
