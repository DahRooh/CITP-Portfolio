using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;

namespace DataLayer;

public interface IPersonDataService
{
    IList<Person> GetPeople(int page, int pageSize);
    public Person GetPerson(string id);
    IList<Person> GetActors(int page, int pageSize);
    int NumberOfActors();
    int NumberOfPeople();
    Person AddNewPerson(Person person);
    bool UpdatePerson(Person person);
    IList <CoActor>GetCoActors(string id);


}