using NSubstitute;
using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ratings.Services.Tests
{
    public class GoogleSearchScraperTests
    {
        private readonly IDownloadService _downloadService = Substitute.For<IDownloadService>();

        private readonly string testHtml =
            "<!doctype html>" +
            "  <div id=\"search\">" +
            "    <div class=\"bkWMgd\">" +
            "      <div class=\"rc\">" +
            "        <div class=\"r\"><a href=\"https://www.sympli.com.au/\"><h3 class=\"LC20lb\"><div class=\"ellip\">Sympli: Making e-Settlements Simple</div></h3><br><div class=\"TbwUpd\"><cite class=\"iUh30\">https://www.sympli.com.au</cite></div></a></div>" +
            "        <div class=\"s\"><span class=\"st\">Meet Sympli - the new <em>Electronic Settlements</em> provider built by users for users. We're here to make <em>e</em>-<em>Settlements</em> simple. Find out more.</span></div>" +
            "      </div>" +
            "      <div class=\"rc\">" +
            "        <div class=\"r\"><a href=\"https://www.infotrack.com.au/news-and-insights/the-evolution-of-e-settlements/\"><h3 class=\"LC20lb\"><div class=\"ellip\">The evolution of e-settlements | InfoTrack</div></h3><br><div class=\"TbwUpd\"><cite class=\"iUh30 bc\">https://www.infotrack.com.au › News &amp; Insights</cite></div></a></div> " +
            "        <div class=\"s\"></div>" +
            "      </div>" +
            "      <div class=\"rc\">" +
            "        <div class=\"r\"><a href=\"https://www.sympli.com.au/\"><h3 class=\"LC20lb\"><div class=\"ellip\">Sympli: Making e-Settlements Simple</div></h3><br><div class=\"TbwUpd\"><cite class=\"iUh30\">https://www.sympli.com.au</cite></div></a></div>" +
            "        <div class=\"s\">test</div>" +
            "      </div>" +
            "      <div class=\"rc\">" +
            "        <div class=\"r\"></div>" +
            "        <div class=\"s\"></div>" +
            "      </div>" +
            "    </div>" +
            "  </div>" +
            "</html>";

        private readonly string notFoundTestHtml =
            "<!doctype html>" +
            "  <div class=\"mnr-c\"><div class=\"med card-section\"><p>Your search - <em>vcfgvjvhkgdr</em> - did not match any documents.</p></div></div>" +
            "  <div id=\"search\"></div>" +
            "</html>";

        [Fact]
        public void GoogleSearchScraper_CanGenerateSearchUrls()
        {
            // ASSERT:
            // Scraper can generate correct list of urls required to run a search

            var SUT = new GoogleSearchScraper(downloadService: null);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;

            var requestUrls = SUT.GetSearchRequestUrls(keyWords, maxSearchResults).ToList();

            Assert.NotNull(requestUrls);
            Assert.True(requestUrls.Count == 10);
            Assert.True(requestUrls[0] == "https://www.google.com.au/search?q=KEYWORD1+KEYWORD2&start=0");
            Assert.True(requestUrls[1] == "https://www.google.com.au/search?q=KEYWORD1+KEYWORD2&start=10");
            Assert.True(requestUrls[9] == "https://www.google.com.au/search?q=KEYWORD1+KEYWORD2&start=90");
        }

        [Fact]
        public async Task GoogleSearchScraper_Search_ReturnsValidSearchResults()
        {
            // ASSERT:
            // Scraper is able to process multiple search downloads.
            // One of the results contains expected substring.

            _downloadService
                .DownloadWebsitesParallelAsync(urls: Arg.Any<IEnumerable<string>>())
                .Returns(new List<string> { testHtml, testHtml, testHtml });

            var SUT = new GoogleSearchScraper(downloadService: _downloadService);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;
            var expectedSubstring = "www.sympli.com.au";

            var searchResults = await SUT.Search(keyWords, maxSearchResults) as List<string>;

            Assert.NotNull(searchResults);
            Assert.True(searchResults.Any(r => r.Contains(expectedSubstring)));
        }

        [Fact]
        public async Task GoogleSearchScraper_BadSearch_ReturnsEmptyList()
        {
            // ASSERT:
            // Scraper is able to handle responses with no search results.

            _downloadService
                .DownloadWebsitesParallelAsync(urls: Arg.Any<IEnumerable<string>>())
                .Returns(new List<string> { notFoundTestHtml });

            var SUT = new GoogleSearchScraper(downloadService: _downloadService);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;

            var searchResults = await SUT.Search(keyWords, maxSearchResults) as List<string>;

            Assert.NotNull(searchResults);
            Assert.False(searchResults.Any());
        }

        [Fact]
        public void GoogleSearchScraper_CanParsePageContent()
        {
            var SUT = new GoogleSearchScraper(downloadService: null);
            var parsedItems = SUT.GetAllSearchResultItems(new string[] { testHtml }).ToList();

            Assert.NotNull(parsedItems);
            Assert.True(parsedItems.Count == 4);
        }

        [Fact]
        public void GoogleSearchScraper_CanParseMultiplePages()
        {
            var SUT = new GoogleSearchScraper(downloadService: null);
            var parsedItems = SUT.GetAllSearchResultItems(new string[] { testHtml, testHtml }).ToList();

            Assert.NotNull(parsedItems);
            Assert.True(parsedItems.Count == 8);
        }
    }
}
