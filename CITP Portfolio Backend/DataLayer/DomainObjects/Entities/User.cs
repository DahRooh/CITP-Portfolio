using DataLayer.DomainObjects.Relations;

namespace DataLayer.DomainObjects;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    
    public List<UserTitleReview> Reviews { get; set; }
    public IList<UserLikesReview> UserLikes { get; set; } = new List<UserLikesReview>();
    public IList<UserSession>? UserSessions { get; set; }
}