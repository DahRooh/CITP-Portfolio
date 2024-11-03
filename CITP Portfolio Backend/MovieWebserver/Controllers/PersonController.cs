using Microsoft.AspNetCore.Mvc;
using MovieWebserver.Model;
using DataLayer;
using MovieWebserver.Model.Person;
using Mapster;
using DataLayer.DomainObjects;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieWebserver.Model.Title;
using DataLayer.DomainObjects.FunctionResults;
using DataLayer.Model.Person;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace MovieWebserver.Controllers;


[ApiController]
[Route("api/person")]
public class PersonController : BaseController
{
    private readonly IPersonDataService _ds;
    private readonly LinkGenerator _linkGenerator;

    public PersonController(IPersonDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
        _linkGenerator = linkGenerator;
    }

    [HttpGet(Name = nameof(GetPeople))]
    public IActionResult GetPeople(int page = 1, int pageSize = 20)
    {
        var people = _ds.GetPeople(page, pageSize).Select(x => CreatePersonModel(x)).ToList();
        var numberOfItems = _ds.NumberOfPeople();

        object result = CreatePaging(
            nameof(GetPeople),
            "Person",
            page,
            pageSize,
            numberOfItems,
            people);

        return Ok(result);
    }

    [HttpGet("{id}", Name = nameof(GetPerson))]
    public IActionResult GetPerson(string id)
    {
        var person = _ds.GetPerson(id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(CreatePersonModel(person));
    }



    [HttpGet("actor", Name = nameof(GetActors))] // maybe move to title? get actors from title (get cast)
                                                 // maybe use this for finding the highest rated actors on site to display? 
    public IActionResult GetActors([FromQuery] int page = 1, [FromQuery] int pageSize = 20) 
    {
        var actors = _ds.GetActors(page, pageSize).Select(x => CreatePersonModel(x)).ToList();
        var numberOfItems = _ds.NumberOfActors();
        object result = CreatePaging(
            nameof(GetPeople),
            "Person",
            page,
            pageSize,
            numberOfItems,
            actors);
        return Ok(actors);
    }


    [HttpPost]
    public IActionResult CreatePerson(PersonModel personModel)
    {
        if (personModel == null)
        {
            return NotFound();
        }

        var person = new Person
        {
            Name = personModel.Name,
            BirthYear = personModel.BirthYear,
            DeathYear = personModel.DeathYear,

        };

        var newPerson = _ds.AddNewPerson(person);
        return Created(nameof(GetPerson), CreatePersonModel(newPerson));
    }


    [HttpPut("{id}")]
    public IActionResult UpdatePerson(string id, PersonModel personModel)
    {
        if (personModel == null)
        {
            return NotFound();
        }


        var existingPerson = _ds.GetPerson(id);
        if (existingPerson == null)
        {
            return NotFound();
        }

        var UpdateThePerson = new Person
        {
            Id = id,
            Name = personModel.Name,
            BirthYear = personModel.BirthYear,
            DeathYear = personModel.DeathYear,
        };

        var updatingPerson = _ds.UpdatePerson(UpdateThePerson);
        if (!updatingPerson)
        {
            return NotFound();
        }

        return Ok(CreatePersonModel(existingPerson));
    }

    [HttpGet("coactors", Name = nameof(GetCoActors))] // maybe make person/id/coactors? then id not from query
    public IActionResult GetCoActors([FromQuery] string id, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var coActors = _ds.GetCoActors(id, page, pageSize).Select(x => CreateCoActorModel(x)).ToList();

        var count = _ds.NumberOfCoActors(id);
        var results = CreatePaging(nameof(GetCoActors), "Person", page, pageSize, count, coActors, id);
        
        return Ok(results);
    }


    private PersonModel? CreatePersonModel(Person? person)
    {
        if (person == null)
        {
            return null;
        }

        var model = person.Adapt<PersonModel>();
        model.Url = GetWebpageUrl(nameof(GetPerson), "Person", new { id = person.Id }); // Initializing the Url property
        return model;
    }


    private CoActorModel? CreateCoActorModel(CoActor coActor)
    {
        var model = coActor.Adapt<CoActorModel>();
        var url = GetWebpageUrl(nameof(GetPerson), "Person", new { id = coActor.PersonId });
        model.Url = url;
        model.PersonId = coActor.PersonId;
        model.CoActors = coActor._CoActor;
        model.Title = coActor.TitleName;
        model.PersonRating = coActor.PersonRating;

        return model;
    }
}

