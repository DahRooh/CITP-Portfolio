using DataLayer.DomainObjects.Relations;
using DataLayer.DomainObjects;
using MovieWebserver.Model.User;

namespace MovieWebserver.Model.Title;

public class ReviewModel
{
    public string Id { get; set; }
    public string TitleId {  get; set; } 
    public string Username { get; set; }
    public string Text { get; set; }
    public int Rating { get; set; }
    public int Liked { get; set; } 


}


