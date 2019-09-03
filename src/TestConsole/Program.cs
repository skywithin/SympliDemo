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
                ISearchScraper ss = new GoogleSearchScraper(ds);
                IRatingService rs = new RatingService(ss);


                var keyWords = new string[] { "e-settlements" };
                var searchItem = "www.sympli.com.au";
                var maxSearchResults = 30;

                var result = Task.Run(() => rs.GetRatings(keyWords, searchItem, maxSearchResults)).Result;

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
