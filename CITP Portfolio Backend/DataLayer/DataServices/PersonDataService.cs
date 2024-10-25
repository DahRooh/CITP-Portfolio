namespace DataLayer;

public class PersonDataService : IPersonDataService
{
    private MVContext db;
    
    public IList<Person> GetPeople()
    {
        db = new MVContext();
        var people = db.People.Take(5).ToList();

        if (people == null || people.Count == 0)
        {
            return null;
        }
        
        return people;
    }
}