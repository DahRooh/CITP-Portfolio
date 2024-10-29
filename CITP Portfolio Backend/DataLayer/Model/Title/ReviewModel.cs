using DataLayer.DomainObjects.Relations;
using DataLayer.DomainObjects;
using MovieWebserver.Model.User;

namespace MovieWebserver.Model.Title;

public class ReviewModel
{
    public string Text { get; set; }
    public int Liked { get; set; } 

    public TitleModel Title {  get; set; }
    public string TitleId {  get; set; } 
    public IList<UserModel> UserLikes { get; set; }

    public UserModel CreatedBy { get; set; }
}


