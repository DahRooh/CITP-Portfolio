using System.Net;
using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using DataLayer.DomainObjects.Entities;
using DataLayer.IDataServices;
using DataLayer.DomainObjects.FunctionResults;
using MovieWebserver.Model;
using MovieWebserver.Model.User;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.Search;
using DataLayer.Model.User;
using Microsoft.AspNetCore.Authorization;
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
        if (decodedToken != null)
        {
            var claim = decodedToken.Claims.FirstOrDefault();
            if (claim != null) {
                username = claim.Value;
                items = _ds.Search(keyword, page, pageSize, username)
                            .Select(x => CreateSearchResultModel(x)).ToList();
            }

        } else
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
