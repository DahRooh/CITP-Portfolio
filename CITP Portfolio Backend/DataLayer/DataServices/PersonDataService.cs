using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataLayer;

public class PersonDataService : IPersonDataService
{
    private MVContext db;


    public int NumberOfPeople()
    {
        db = new MVContext();
        var count = db.People.Count();
        return count;
    }

    public IList<Person> GetPeople(int page, int pageSize)
    {
        db = new MVContext();
        var people = db.People
            .Include(p => p.Professions)
            .Include(p => p.InvolvedIn)
            .ThenInclude(x => x.Title)
            .AsSplitQuery()
            .OrderByDescending(p => p.PersonRating)
            .Skip(page * pageSize).Take(pageSize)
            .ToList();


        if (people == null || people.Count == 0)
        {
            return null;
        }

        return people;
    }


    public Person GetPerson(string id)
    {
        db = new MVContext();
        var person = db.People
            .Include(p => p.Professions)
            .Include(p => p.InvolvedIn)
            .ThenInclude(x => x.Title)
            .AsSplitQuery()
            .FirstOrDefault(x => x.Id == id);

        if (person == null)
        {
            return null;
        }

        return person;



    }




    public int NumberOfActors()
    {
        db = new MVContext();
        var count = db.People
            .Include(p => p.Professions)
            .Where(p => p.Professions.Any(pr => pr.ProfessionName == "actor"))
            .Count();
        return count;
    }


    public IList<Person> GetActors(int page, int pageSize)
    {
        db = new MVContext();
        var actors = db.People
                        .Include(p => p.Professions)
                        .Where(p => p.Professions.Any(pr => pr.ProfessionName == "actor"))
                        .OrderByDescending(p => p.PersonRating)
                        .Skip(page * pageSize).Take(pageSize)
                        .ToList();

        if (actors == null || actors.Count == 0)
        {
            return null;
        }

        return actors;
    }



    public Person AddNewPerson(Person person)
    {
        db = new MVContext();
        string maxId = db.People.Max(x => x.Id);

        string prefix = maxId.Substring(0, 2); // Result: nm
        string numberPart = maxId.Substring(2); // Result: 9993710

        int number = int.Parse(numberPart);
        string newId;

        do // Loop to find a new ID that is not already in use
        {
            number++;
            newId = $"{prefix}{number}"; // String concatenation -> giving a new ID

        } while (db.People.Any(x => x.Id == newId));


        var NewPerson = new Person
        {
            Id = newId,
            Name = person.Name,
            BirthYear = person.BirthYear,
            DeathYear = person.DeathYear,
        };

        db.People.Add(NewPerson);
        db.SaveChanges();
        return NewPerson;

    }

    public bool UpdatePerson(Person person)
    {
        db = new MVContext();
        var dbPerson = db.People.Find(person.Id);

        if (dbPerson == null)
        {
            return false;
        }

        dbPerson.Name = person.Name;
        dbPerson.BirthYear = person.BirthYear;
        dbPerson.DeathYear = person.DeathYear;

        db.SaveChanges();
        return true;
    }

    public int NumberOfCoActors(string id)
    {
        db = new MVContext();
        return db.CoActors.FromSqlRaw("select * from find_coactors({0})", id).Count();
    }

    public IList<CoActor> GetCoActors(string id, int page, int pageSize)
    {
        db = new MVContext();
        var coActors = db.CoActors.FromSqlRaw("select * from find_coactors_with_skip({0}, {1}, {2})", id, page, pageSize)
            .OrderByDescending(a => a.PersonRating).ToList();
        return coActors;
    }


    
    public int NumberOfKnownFor(string id)
    {
        db = new MVContext();
        return db.KnownFor.FromSqlRaw("select * from person_known_for({0})", id).Count();
    }

    public IList<KnownFor> GetKnownFor(string id, int page, int pageSize)
    {
        db = new MVContext();
        var knownForList = db.KnownFor.FromSqlRaw("select * from person_known_for_with_skip({0}, {1}, {2})", id, page, pageSize).ToList();
        return knownForList;
    }
    






    
    /*
    public int NumberOfKnownFor(string id)
    {
        db = new MVContext();
        return db.PersonInvolvedIn
            .Include(p => p.Title)
            .Where(p => p.Id == id)
            .Count();

    }

    public IList<Title> GetKnownFor(string id, int page, int pageSize)
    {
        db = new MVContext();
        var knownFor = db.PersonInvolvedIn
            .Include(p => p.Title)
            .Where(p => p.Id == id)
            .OrderByDescending(p => p.Title.Rating)
            .Select(p => p.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return knownFor;

    }
    */
    
  
}