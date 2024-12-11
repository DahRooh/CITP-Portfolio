using MovieWebserver.Model.Title;

namespace DataLayer.Model.Search;

public class SearchResultModel
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string PersonId { get; set; }
    public double? PersonRating { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string PersonName { get; set; }
    public string? Poster { get; set; }
    public double? Rating { get; set; }
    // (topcast) 
}