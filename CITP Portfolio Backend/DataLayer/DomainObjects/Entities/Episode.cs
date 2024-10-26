namespace DataLayer.DomainObjects;

public class Episode : Title
{
    public int SeasonNumber { get; set; }
    public int EpisodeNumber { get; set; }
}

