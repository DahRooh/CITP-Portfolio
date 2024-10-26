using DataLayer.DomainObjects;

namespace DataLayer;

public interface IPersonDataService
{
    IList<Person> GetPeople();
}