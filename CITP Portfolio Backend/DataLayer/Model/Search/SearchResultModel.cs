using MovieWebserver.Model.Title;

namespace DataLayer.Model.Search;

public class SearchResultModel
{
    public string Title { get; set; }
    public string Url { get; set; }
    public string? Poster { get; set; }
    public double? Rating { get; set; }
    // (topcast) 
}