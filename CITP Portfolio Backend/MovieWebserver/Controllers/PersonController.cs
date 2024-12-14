using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Person;
using Mapster;
using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;
using DataLayer.Model.Person;

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
    public IActionResult GetPeople(int page = 1, int pageSize = 5)
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

    [HttpGet("{pId}", Name = nameof(GetPerson))]
    public IActionResult GetPerson(string pId)
    {
        var person = _ds.GetPerson(pId);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(CreatePersonModel(person));
    }



    [HttpGet("actor", Name = nameof(GetActors))] // maybe move to title? get actors from title (get cast)
                                                 // maybe use this for finding the highest rated actors on site to display? 
    public IActionResult GetActors([FromQuery] int page = 1, [FromQuery] int pageSize = 5) 
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


    [HttpGet("{pId}/coactors", Name = nameof(GetCoActors))] // maybe make person/id/coactors? then id not from query
    public IActionResult GetCoActors(string pId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        var coActors = _ds.GetCoActors(pId, page, pageSize).Select(x => CreateCoActorModel(x)).ToList();

        var count = _ds.NumberOfCoActors(pId);
        var results = CreatePaging(nameof(GetCoActors), "Person", page, pageSize, count, coActors);
        
        return Ok(results);
    }



    [HttpGet("{pId}/knownfor", Name = nameof(GetKnownFor))]
    public IActionResult GetKnownFor(string pId, [FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        var knownFor = _ds.GetKnownFor(pId, page, pageSize).Select(x => CreateKnownForModel(x)).ToList();

        var count = _ds.NumberOfKnownFor(pId);
    
        var results = CreatePaging(nameof(GetKnownFor), "Person", page, pageSize, count, knownFor);

        return Ok(results);
    }





    // models
    private PersonModel? CreatePersonModel(Person? person)
    {
        if (person == null)
        {
            return null;
        }

        var model = person.Adapt<PersonModel>();
        model.PersonRating = person.PersonRating;
        model.Url = GetWebpageUrl(nameof(GetPerson), "Person", new { pId = person.Id }); // Initializing the Url property

        return model;
    }


    private CoActorModel? CreateCoActorModel(CoActor coActor)
    {
        var model = coActor.Adapt<CoActorModel>();
        var url = GetWebpageUrl(nameof(GetPerson), "Person", new { pId = coActor.PersonId });
        model.Url = url;
        model.Id = coActor.PersonId;

        return model;
    }

    private KnownForModel? CreateKnownForModel(KnownFor knownFor)
    {
        var model = knownFor.Adapt<KnownForModel>();
        var url = GetWebpageUrl(nameof(GetPerson), "Person", new { pId = knownFor.KnownForId });
        model.Url = url;
        model.KnownForId = knownFor.KnownForId;
        model.KnownForTitle = knownFor.KnownForTitle;
        
        return model;
    }




}
