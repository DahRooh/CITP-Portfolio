using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/titles")]
public class TitleController : ControllerBase
{
    readonly LinkGenerator _linkGenerator;
    readonly ITitleDataService _ds; 

    public TitleController(ITitleDataService ds, LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
        _ds = ds;
    }

    [HttpGet]
    public IActionResult GetTitles() 
    {
        var titles = _ds.GetTitles().Select(x => CreateTitleModel(x)).ToList();

        return Ok(titles);
    }


    private TitleModel? CreateTitleModel(Title? title)
    {
        if (title == null || title._Title == "")
        {
            return null;
        }

        var newTitle = new TitleModel { Title = title._Title };
        return newTitle;
    }

}
