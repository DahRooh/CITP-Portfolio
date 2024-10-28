using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.IDataServices
{
    public interface IUserDataService
    {
        public bool CreateReview(string titleId, string ReviewText);


        public List<User> GetUsers();
        public User GetUser(int userId);
        public List<Search> GetHistory(int userId);
        public List<Bookmark> GetBookmarks(int userId);
        public Review GetReview(int reviewId);
        public List<Review> GetReviews(string titleId);
        public UserSessionsHistory GetCurrentSession(int userId);
        public List<UserSessionsHistory> GetSessions(int userId);
        public List<(Title, int)> GetLikeHistory(int userId);

        public bool CreateBookmark(string titleId, int userId);

        public bool LikeReview(int reviewId, int userId, int like); // like can be -1, 0 or 1.
        public bool UpdateEmail(int userId, string email);


        public bool DeleteUser(int userId);

        // authentication
        public bool CreateUser(string username, string email, string password);
        public bool LogIn(string username, string password);
        public bool LogOff(string username);




    }
}
