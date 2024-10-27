using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/titles")]
public class TitleController : BaseController
{
    private readonly ITitleDataService _ds; 
    private readonly LinkGenerator _linkGenerator;

    public TitleController(ITitleDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
        _linkGenerator = linkGenerator;
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
        newTitle.Url = GetWebpageUrl(nameof(GetTitles), title.Id);
        return newTitle;
    }


}
