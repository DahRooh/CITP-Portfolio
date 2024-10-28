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

namespace DataLayer.IDataServices;

public interface IUserDataService
{


    public List<User> GetUsers();
    public User GetUser(int userId);
    public List<Search> GetHistory(int userId);
    public List<Bookmark> GetBookmarks(int userId);
    public Bookmark CreateBookmark();
    public UserTitleReview CreateReview(ReviewModel reviewModel);
    public User CreateUser(UserModel userModel);

    public List<UserTitleReview> GetReviews(int userId);
    public UserTitleReview GetReview(int reviewId);
    public UserSessionsHistory GetCurrentSession(int userId);
    public List<UserSessionsHistory> GetSessions(int userId);
    public List<(Title, int)> GetLikeHistory(int userId);

    public bool CreateBookmark(string titleId, int userId);

    public bool LikeReview(int reviewId, int userId, int like); // like can be -1, 0 or 1.
    public bool UpdateEmail(int userId, string email);


    public bool DeleteUser(int userId);

    // authentication
    public bool LogIn(string username, string password);
    public bool LogOff(string username);




}
