namespace DataLayer;

public class Profession
{
    public string Name { get; set; }
    public IList<Person>? Persons { get; set; }
}