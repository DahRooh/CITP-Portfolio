using Microsoft.AspNetCore.Mvc;
using DataLayer.IDataServices;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.Search;
using DataLayer.DomainObjects;
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

    [HttpGet("titles", Name = nameof(GetSearchesTitle))]
    public IActionResult GetSearchesTitle([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
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

        var results = CreatePaging(nameof(GetSearchesTitle), "Search", page, pageSize, count, items, keyword);
        return Ok(results);
    }

    [HttpGet("people", Name = nameof(GetSearchesPeople))]
    public IActionResult GetSearchesPeople([FromQuery] string keyword, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        // if logged in
        var username = string.Empty;
        var items = new List<SearchResultPersonModel>();
        var decodedToken = GetDecodedToken();

        if (decodedToken != null) // store result for people logged in
        {
            var claim = decodedToken.Claims.FirstOrDefault();
            if (claim != null)
            {
                username = claim.Value;
                items = _ds.SearchPerson(keyword, page, pageSize, username)
                            .Select(x => CreateSearchResultPersonModel(x)).ToList();

            }

        }
        else // dont store result for anonymous users
        {

            items = _ds.SearchPerson(keyword, page, pageSize, username)
                        .Select(x => CreateSearchResultPersonModel(x)).ToList();
        }

        var countPerson = _ds.SearchPersonCount(keyword);

        var results = CreatePaging(nameof(GetSearchesPeople), "Search", page, pageSize, countPerson, items, keyword);
       
        return Ok(results);
    }

    [NonAction]
    public SearchResultModel CreateSearchResultModel(SearchResult searchResult)
    {
        
        var url = string.Empty;


        var result = new SearchResultModel
        {
            Id = searchResult.Id,
            Text = searchResult.Text,
            Poster = searchResult.Poster,
            Rating = searchResult.Rating,
            Type = (searchResult.Type == "series") ? searchResult.Type : "title"
        };
        return result;
    }

    [NonAction]
    public SearchResultPersonModel CreateSearchResultPersonModel(SearchResultPerson searchResultPerson)
    {

        var webpage = _ds.GetWebpage(searchResultPerson.WebpageId);
        var url = GetWebpageUrl(nameof(PersonController.GetPerson), "Person", new { pId = webpage.PersonId});

        var person = webpage.Person;

        var result = new SearchResultPersonModel
        {
            Url = url,
            Id = person.Id,
            Text = person.Name,
            Rating = person.PersonRating,
            Type = "person"
        };

        return result;
    }


}
