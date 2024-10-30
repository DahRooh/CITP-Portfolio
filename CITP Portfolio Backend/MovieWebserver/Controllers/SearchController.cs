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
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/find")]
public class SearchController : BaseController
{
    ISearchDataService _ds;
    public SearchController(ISearchDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
    }

    [HttpGet(Name = nameof(GetSearches))]
    public IActionResult GetSearches([FromQuery] string keyword)
    {
        Console.WriteLine(keyword);
        var results = _ds.Search(keyword, 10)
            .Select(x => CreateSearchResultModel(x)).ToList();
        
        return Ok(results);
    }
    
    [NonAction]
    public SearchResultModel CreateSearchResultModel(SearchResult searchResult)
    {
        
          var webpage = _ds.GetWebpage(searchResult.WebpageId);
          var result = new SearchResultModel
          {
              Title = webpage.Title._Title,
              Url = GetWebpageUrl(nameof(GetSearches), new { webpage.TitleId }),
              Poster = webpage.Title.Poster,
              Rating = webpage.Title.Rating
          };
        return result;
    }
}
