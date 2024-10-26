using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DomainObjects;

public class PersonInvolvedIn
{
    public string PersonId { get; set; }

    public string TitleId { get; set; }

    public string Character { get; set; }
    public string Job { get; set; }

    public Person? Person { get; set; }
    public Title? Title { get; set; }
}