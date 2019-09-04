namespace Ratings.Interfaces
{
    public interface ISearchScraperFactory
    {
        ISearchScraper GetSearchScraperInstance(SearchEngineType type);
    }
}
