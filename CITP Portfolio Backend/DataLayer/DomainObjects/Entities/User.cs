using DataLayer.DomainObjects.Relations;

namespace DataLayer.DomainObjects;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Salt { get; set; }


    public IList<UserTitleReview> Reviews { get; set; } = new List<UserTitleReview>();
    public IList<UserSearch> Searches { get; set; } = new List<UserSearch>();
    public IList<UserLikesReview> UserLikes { get; set; } = new List<UserLikesReview>();
    public IList<UserSession>? UserSessions { get; set; } = new List<UserSession>();
    public IList<UserBookmark> UserBookmarks { get; internal set; } = new List<UserBookmark>();
}