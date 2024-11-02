namespace DataLayer.DomainObjects;

public class Movie 
{
    public string Id { get; set; }
    public string TitleId { get; set; }

    public Title Title { get; set; }

}

