using Microsoft.AspNetCore.Mvc;
using MovieWebserver.Model;
using DataLayer;
using MovieWebserver.Model.Person;
using Mapster;
using DataLayer.DomainObjects;
using Microsoft.AspNetCore.Mvc.RazorPages;

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

    [HttpGet (Name = nameof(GetPeople))]
    public IActionResult GetPeople(int page, int pageSize)
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

        return Ok(people);
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



    [HttpGet("actor", Name = nameof(GetActors))]
    public IActionResult GetActors(int page, int pageSize)
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



    private PersonModel? CreatePersonModel(Person? person)
    {
        if (person == null)
        {
            return null;
        }

        var model = person.Adapt<PersonModel>();
        model.Url = GetWebpageUrl(nameof(GetPeople), "Person", person.Id); // Initializing the Url property
        return model;
    }


}
