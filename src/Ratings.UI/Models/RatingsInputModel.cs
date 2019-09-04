using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ratings.UI.Models
{
    public class RatingsInputModel
    {
        public string KeyWord { get; set; }
        public string SearchItem { get; set; }
        public string SearchEngine { get; set; }
        public int MaxSearchResults { get; set; }

        public string GetCacheItemKey()
        {
            return $"{this.KeyWord}+{this.SearchItem}+{this.SearchEngine}+{this.MaxSearchResults}";
        }
    }
}