using DataLayer.DomainObjects;
using DataLayer.DomainObjects.Relations;
using DataLayer.DomainObjects.FunctionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MovieWebserver.Model.Title;
using MovieWebserver.Model.User;
using DataLayer.Model.User;

namespace DataLayer.IDataServices;

public interface IUserDataService
{


    public List<User> GetUsers();
    public User GetUser(int userId);
    public User GetUser(string username);
    public bool IsEmailUsed(string email);

    public List<UserSearch> GetHistory(int userId);
    public List<Bookmark> GetBookmarks(int userId);
    public IList<UserLikesReview> GetLikes(int userId);
    public User CreateUser(CreateUserModel userModel);

    public List<UserTitleReview> GetReviews(int userId);
    public UserTitleReview GetReview(int reviewId);
    public UserSessionsHistory GetCurrentSession(int userId);
    public List<UserSessionsHistory> GetSessions(int userId);
    public List<(Title, int)> GetLikeHistory(int userId);


    public bool LikeReview(int reviewId, int userId, int like); // like can be -1, 0 or 1.
    public bool UpdateEmail(string username, string email);
    public bool UpdatePassword(int userId, string password);

    public bool DeleteUser(int userId);

    public bool DeleteReview(int reviewId);
    public bool DeleteBookmark(string bookmarkId);
    bool DeleteSearch(string searchId);
    bool ClearHistory(int searchId);

}
