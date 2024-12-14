using MovieWebserver.Model.Title;

namespace DataLayer.Model.Search;

public class SearchResultModel
{
    public string Id { get; set; }
    public string Type { get; set; }
    public string Text { get; set; }
    public string? Poster { get; set; }
    public double? Rating { get; set; }
}