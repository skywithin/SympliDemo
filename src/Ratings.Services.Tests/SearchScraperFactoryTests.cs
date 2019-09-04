using NSubstitute;
using Ratings.Interfaces;
using System;
using Xunit;

namespace Ratings.Services.Tests
{
    public class SearchScraperFactoryTests
    {
        private readonly IDownloadService _downloadService = Substitute.For<IDownloadService>();

        [Fact]
        public void SearchScraperFactory_ReturnsValidClassInstance()
        {
            // ASSERT:
            // Factory can generate an in instance of a required class

            var SUT = new SearchScraperFactory(_downloadService);
            var requiredType = SearchEngineType.Google;

            var scraper = SUT.GetSearchScraperInstance(requiredType);

            Assert.NotNull(scraper);
            Assert.IsType<GoogleSearchScraper>(scraper);
        }

        [Fact]
        public void SearchScraperFactory_WithInvalidType_ThrowsApplicationException()
        {
            // ASSERT:
            // Factory will throw an exception for an Unknown type 

            var SUT = new SearchScraperFactory(_downloadService);
            var invalidType = SearchEngineType.Unknown;

            var ex = Assert.Throws<ApplicationException>(() => SUT.GetSearchScraperInstance(invalidType));
        }

        
    }
}
