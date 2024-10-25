using Microsoft.AspNetCore.Mvc;
using MovieWebserver.Model;
using DataLayer;
using MovieWebserver.Model.Person;

namespace MovieWebserver.Controllers;


[ApiController]
[Route("api/person")]
public class PersonController : ControllerBase
{
    IPersonDataService _dataService;
    LinkGenerator _linkGenerator;
    public PersonController(IPersonDataService dataService, LinkGenerator linkGenerator)
    {
        _dataService = dataService;
        _linkGenerator = linkGenerator;
    }

    [HttpGet]
    public IActionResult GetPeople()
    {
        var people = _dataService.GetPeople().Select(x => CreatePersonModel(x)).ToList();
        return Ok(people);
    }

    private PersonModel CreatePersonModel(Person x)
    {
        return new PersonModel()
        {
            Name = x.Name
        };
    }
}
