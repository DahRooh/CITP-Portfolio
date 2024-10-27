using DataLayer.DomainObjects;

namespace DataLayer;

public interface IPersonDataService
{
    IList<Person> GetPeople();
    IList<Person> GetActors();
    Person AddNewPerson(Person person);
    bool UpdatePerson(Person person);


}