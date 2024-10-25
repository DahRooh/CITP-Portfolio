namespace DataLayer;

public class PersonInvolvedIn
{
    public Person? Person { get; set; }
    public string PersonId { get; set; }

    public Title? Title { get; set; }
    public string TitleId { get; set; }

    public string Character { get; set; }
    public string Job { get; set; }
}