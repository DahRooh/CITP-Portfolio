using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class PersonDataService : IPersonDataService
{
    private MVContext db;
    
    public IList<Person> GetPeople()
    {
        db = new MVContext();
        var people = db.People
            .Include(p => p.Profession)
            .Include(p => p.InvolvedIn)
            .ThenInclude(x => x.Title)
            .OrderBy(p => p.Id).Take(5).ToList();

        if (people == null || people.Count == 0)
        {
            return null;
        }
        
        return people;
    }
}