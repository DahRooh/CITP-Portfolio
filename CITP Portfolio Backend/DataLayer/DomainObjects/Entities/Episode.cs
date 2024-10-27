namespace DataLayer.DomainObjects;

public class Episode
{
    public string Id { get; set; }
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }


    public string TitleId { get; set; }

    public Title Title { get; set; }
}

