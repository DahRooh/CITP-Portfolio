using Microsoft.AspNetCore.Mvc;
using DataLayer.IDataServices;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.Search;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/search")]
public class SearchController : BaseController
{
    ISearchDataService _ds;
    public SearchController(ISearchDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
    }

    [HttpGet(Name = nameof(GetSearches))]
    public IActionResult GetSearches([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 25)
    {
        // if logged in
        var username = string.Empty;
        var items = new List<SearchResultModel>();
        var decodedToken = GetDecodedToken();
        if (decodedToken != null) // store result for people logged in
        {
            var claim = decodedToken.Claims.FirstOrDefault();
            if (claim != null) {
                username = claim.Value;
                items = _ds.Search(keyword, page, pageSize, username)
                            .Select(x => CreateSearchResultModel(x)).ToList();
            }

        } else // dont store result for anonymous users
        {
            items = _ds.Search(keyword, page, pageSize, username)
                        .Select(x => CreateSearchResultModel(x)).ToList();
        }





        var count = _ds.SearchCount(keyword);

        var results = CreatePaging(nameof(GetSearches), "Search", page, pageSize, count, items, keyword);
        return Ok(results);
    }
    
    [NonAction]
    public SearchResultModel CreateSearchResultModel(SearchResult searchResult)
    {
        
          var webpage = _ds.GetWebpage(searchResult.WebpageId);
          var result = new SearchResultModel
          {
              Url = GetWebpageUrl(nameof(TitleController.GetTitle), "Title", new { tId = webpage.TitleId }),
              Title = webpage.Title._Title,
              Poster = webpage.Title.Poster,
              Rating = webpage.Title.Rating
          };
        return result;
    }
}
