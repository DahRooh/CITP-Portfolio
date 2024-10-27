using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/titles")]
public class TitleController : ControllerBase
{
    private readonly ITitleDataService _ds; 
    private readonly GetUrl _getUrl;

    public TitleController(ITitleDataService ds, GetUrl getUrl)
    {
        _ds = ds;
        _getUrl = getUrl;
    }

    [HttpGet]
    public IActionResult GetTitles() 
    {
        var titles = _ds.GetTitles().Select(x => CreateTitleModel(x)).ToList();

        return Ok(titles);
    }

    [HttpGet("{id}", Name = nameof(GetTitles))]
    public IActionResult GetTitle(string id)
    {
        var title = _ds.GetTitles().FirstOrDefault(x => x.Id == id);
        if (title == null)
        {
            return NotFound();
        }
        return Ok(CreateTitleModel(title));
    }

    private TitleModel? CreateTitleModel(Title? title)
    {
        if (title == null || title._Title == "")
        {
            return null;
        }

        var newTitle = title.Adapt<TitleModel>();
        newTitle.Url = _getUrl.GetWebpageUrl(HttpContext, title.Id, nameof(GetTitles));
        return newTitle;
    }


}
