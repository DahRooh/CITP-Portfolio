using Microsoft.AspNetCore.Mvc;
using MovieWebserver.Model;
using DataLayer;
using MovieWebserver.Model.Person;
using Mapster;
using DataLayer.DomainObjects;

namespace MovieWebserver.Controllers;


[ApiController]
[Route("api/person")]
public class PersonController : BaseController
{
    IPersonDataService _dataService;
    private readonly LinkGenerator _linkGenerator;
    public PersonController(IPersonDataService dataService, LinkGenerator linkGenerator) : base(linkGenerator)
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

    [HttpGet("{id}", Name = nameof(GetPeople))]
    public IActionResult GetPerson(string id)
    {
        var person = _dataService.GetPeople().FirstOrDefault(x => x.Id == id);
        if (person == null)
        {
            return NotFound();
        }
        return Ok(CreatePersonModel(person));
    }


    [HttpGet("actor")]
    public IActionResult GetActors()
    {
        var actors = _dataService.GetActors().Select(x => CreatePersonModel(x)).ToList();
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

        var newPerson = _dataService.AddNewPerson(person);
        return Created(nameof(GetPerson), CreatePersonModel(newPerson));
    }


    [HttpPut("{id}")]
    public IActionResult UpdatePerson(string id, PersonModel personModel)
    {
        if (personModel == null)
        {
            return NotFound();
        }


        var existingPerson = _dataService.GetPeople().FirstOrDefault(x => x.Id == id);
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

        var updatingPerson = _dataService.UpdatePerson(UpdateThePerson);
        if (!updatingPerson)
        {
            return NotFound();
        }

        return Ok(CreatePersonModel(existingPerson));
    }



    private PersonModel? CreatePersonModel(Person? person)
    {
        if (person == null)
        {
            return null;
        }

        var model = person.Adapt<PersonModel>();
        model.Url = GetWebpageUrl(nameof(GetPeople), person.Id); // Initializing the Url property
        return model;
    }


}
