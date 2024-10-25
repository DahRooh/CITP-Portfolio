namespace DataLayer;

public class PersonInvolvedIn
{
    public string PersonId { get; set; }
    public string TitleId { get; set; }
    public string Character { get; set; }
    public Person? Person { get; set; }
}