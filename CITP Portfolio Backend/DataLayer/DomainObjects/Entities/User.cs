using DataLayer.DomainObjects.Relations;

namespace DataLayer.DomainObjects;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }

    
    public IList<UserTitleReview> Reviews { get; set; }
    public IList<UserSearch> Searches { get; set; }
    public IList<UserLikesReview> UserLikes { get; set; } = new List<UserLikesReview>();
    public IList<UserSession>? UserSessions { get; set; }
    public IList<UserBookmark> UserBookmarks { get; internal set; }
}