namespace DataLayer.DomainObjects;

public class Profession
{
    public string Name { get; set; }
    public IList<PersonProfession>? Persons { get; set; }
}