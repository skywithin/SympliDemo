using Ratings.Interfaces;
using Ratings.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Application started");

            try
            {
                IDownloadService ds = new DownloadService();
                ISearchScraperFactory ssf = new SearchScraperFactory(ds);
                IRatingService rs = new RatingService(ssf);

                var keyWords = new string[] { "e-settlements" };
                var searchItem = "www.sympli.com.au";
                var searchEngineType = SearchEngineType.Google;
                var maxSearchResults = 30;

                var result = Task.Run(() => rs.GetRatings(
                    keyWords, 
                    searchItem, 
                    searchEngineType, 
                    maxSearchResults)).Result;

                foreach(var item in result)
                {
                    Console.WriteLine($"position: {item}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
