# SympliDemo

## Ratings

* Ratings web application consists of project libraries (.Net Standard 2.0), API (.Net Core 2.2), and ASP.NET MVC Web Site (.Net Framework 4.6.1).

* Google search scraper runs multiple queries in parallel (10 results per page). This can be improved by increasing the number of search results per page. 

* Basic caching has only been implemented in Ratings.UI project. Caching should also be implemented in Ratings.API. A proper cache manager needs to be thread-safe.

* Bing search engine scraper is currently work-in-progress.

* User input validation needs to be implemented both on Ratings.UI and Ratings.API.

* Example of API request: http://localhost:5000/api/ratings?keywords=e-settlements&searchItem=www.sympli.com.au&searchEngine=Google&maxSearchResults=100