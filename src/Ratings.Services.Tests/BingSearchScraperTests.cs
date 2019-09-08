using NSubstitute;
using Ratings.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ratings.Services.Tests
{
    public class BingSearchScraperTests
    {
        private readonly IDownloadService _downloadService = Substitute.For<IDownloadService>();

        private readonly string testHtml =
            "<!doctype html>" +
            "  <div id=\"b_content\">" +
            "    <main aria-label=\"Search Results\">" +
            "      <ol id=\"b_results\">" +
            "        <li class=\"b_ad\" data-bm=\"8\">" +
            "          THIS IS A TEST" +
            "        </li>" +
            "        <li class=\"b_algo\" data-bm=\"9\">" +
            "          <a href=\"https://www.pexa.com.au/\">Home | PEXA</a>" +
            "        </li>" +
            "        <li class=\"b_algo\" data-bm=\"10\">" +
            "          <a href=\"https://www.sympli.com.au/\">Making e-Settlements Simple | Sympli</a>" +
            "        </li>" +
            "        <li class=\"b_algo\" data-bm=\"11\">" +
            "          <a href=\"http://www.isettlements.com.au/\">iSettlements</a>" +
            "        </li>" +
            "        <li class=\"b_algo\" data-bm=\"12\">" +
            "          <a href=\"https://www.sympli.com.au/\">Making e-Settlements Simple | Sympli</a>" +
            "        </li>" +
            "      </ol>" +
            "    </main>" +
            "  </div>" +
            "</html>";

        private readonly string notFoundTestHtml =
            "<!doctype html>" +
            "  <div id=\"b_content\">" +
            "    <main aria-label=\"Search Results\">" +
            "      <ol id=\"b_results\">" +
            "        <li class=\"b_no\" data-bm=\"4\">" +
            "          There are no results for..." +
            "        </li>" +
            "      </ol>" +
            "    </main>" +
            "  </div>" +
            "</html>";

        [Fact]
        public void BingSearchScraper_CanGenerateSearchUrls()
        {
            // ASSERT:
            // Scraper can generate correct list of urls required to run a search

            var SUT = new BingSearchScraper(downloadService: null);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;

            var requestUrls = SUT.GetSearchRequestUrls(keyWords, maxSearchResults).ToList();

            Assert.NotNull(requestUrls);
            Assert.True(requestUrls.Count == 10);
            Assert.True(requestUrls[0] == "https://www.bing.com/search?q=KEYWORD1+KEYWORD2&first=1");
            Assert.True(requestUrls[1] == "https://www.bing.com/search?q=KEYWORD1+KEYWORD2&first=11");
            Assert.True(requestUrls[9] == "https://www.bing.com/search?q=KEYWORD1+KEYWORD2&first=91");
        }

        [Fact]
        public async Task BingSearchScraper_Search_ReturnsValidSearchResults()
        {
            // ASSERT:
            // Scraper is able to process multiple search downloads.
            // One of the results contains expected substring.

            _downloadService
                .DownloadWebsitesParallelAsync(urls: Arg.Any<IEnumerable<string>>())
                .Returns(new List<string> { testHtml, testHtml, testHtml });

            var SUT = new BingSearchScraper(downloadService: _downloadService);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;
            var expectedSubstring = "www.sympli.com.au";

            var searchResults = await SUT.Search(keyWords, maxSearchResults) as List<string>;

            Assert.NotNull(searchResults);
            Assert.True(searchResults.Any(r => r.Contains(expectedSubstring)));
        }

        [Fact]
        public async Task BingSearchScraper_BadSearch_ReturnsEmptyList()
        {
            // ASSERT:
            // Scraper is able to handle responses with no search results.

            _downloadService
                .DownloadWebsitesParallelAsync(urls: Arg.Any<IEnumerable<string>>())
                .Returns(new List<string> { notFoundTestHtml });

            var SUT = new BingSearchScraper(downloadService: _downloadService);
            var keyWords = new string[] { "KEYWORD1", "KEYWORD2" };
            var maxSearchResults = 100;

            var searchResults = await SUT.Search(keyWords, maxSearchResults) as List<string>;

            Assert.NotNull(searchResults);
            Assert.False(searchResults.Any());
        }

        [Fact]
        public void BingSearchScraper_CanParsePageContent()
        {
            var SUT = new BingSearchScraper(downloadService: null);
            var parsedItems = SUT.GetAllSearchResultItems(new string[] { testHtml }).ToList();

            Assert.NotNull(parsedItems);
            Assert.True(parsedItems.Count == 4);
        }

        [Fact]
        public void BingSearchScraper_CanParseMultiplePages()
        {
            var SUT = new BingSearchScraper(downloadService: null);
            var parsedItems = SUT.GetAllSearchResultItems(new string[] { testHtml, testHtml }).ToList();

            Assert.NotNull(parsedItems);
            Assert.True(parsedItems.Count == 8);
        }
    }
}
