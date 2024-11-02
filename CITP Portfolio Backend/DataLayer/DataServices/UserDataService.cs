using DataLayer.DomainObjects;
using DataLayer.IDataServices;
using Microsoft.EntityFrameworkCore;
using DataLayer.DomainObjects.FunctionResults;
using MovieWebserver.Model.User;
using MovieWebserver.Model.Title;
using DataLayer.Model.User;
using System.Linq;
using DataLayer.DomainObjects.Relations;


namespace DataLayer.DataServices
{
    public class UserDataService : IUserDataService
    {
        private MVContext db;

        public List<User> GetUsers()
        {
            db = new MVContext();
            var users = db.Users.ToList();
            return users;
        }

        public User GetUser(int userId)
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public User GetUser(string username)
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public bool IsEmailUsed(string email)
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Email == email).FirstOrDefault();
            if (user == null) return false;
            return true;
        }

        public bool CreateBookmark(string titleId, int userId)
        {
            db = new MVContext();
            throw new NotImplementedException();
        }



        //public User CreateUser(CreateUserModel user, string salt)
        public User CreateUser(CreateUserModel user, string salt)
        {
            db = new MVContext();

            var newId = db.Users.FirstOrDefault() == null ? 1 : db.Users.Max(x => x.Id) + 1;

            db.Database.ExecuteSqlRaw("call signup({0}, {1}, {2}, {3}, {4})", newId, user.Username, user.Password,
                user.Email, salt);
            var newUser = db.Users.Where(x => x.Username == user.Username).First();

            if (newUser == null) return null;

            return newUser;

        }



        // this to the title data service, a user can create a review on the title, not from the user route
        //public UserTitleReview CreateReview(ReviewModel review)
        //{
        //   db = new MVContext();

//            db.Database.ExecuteSqlRaw("call rate({0}, {1}, {2}, {3})", review.TitleId, review.CreatedBy.Id, review.Liked, review.Text);
        //           return db.UserReviews.Where(x => x.TitleId == review.TitleId & x.UserId == review.CreatedBy.Id).First();
        //     }


        // also to title?
        public bool LikeReview(int reviewId, int userId, int like)
        {
            throw new NotImplementedException();
        }

        public IList<UserLikesReview> GetLikes(int userId)
        {
            db = new MVContext();
            User user = db.Users.Include(x => x.UserLikes)
                .ThenInclude(x => x.Review)
                .ThenInclude(x => x.createdBy)
                .ThenInclude(x => x.Title)
                .Where(x => x.Id == userId).FirstOrDefault();

            if (user == null) return null;

            return user.UserLikes.ToList();
        }

        public List<UserSearch> GetHistory(int userId)
        {
            db = new MVContext();
            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return db.UserSearches.Include(x => x.User).Include(x => x.Search).Where(x => x.UserId == userId).ToList();

        }


        public List<(Title, int)> GetLikeHistory(int userId)
        {
            throw new NotImplementedException();
        }

        public List<UserSessionsHistory> GetSessions(int userId)
        {
            db = new MVContext();

            var results = db.SessionHistory.FromSqlInterpolated($"select * from get_session({userId})").ToList();


            return results;
        }



        public List<UserBookmark> GetBookmarks(int userId)
        {
            db = new MVContext();
            return db.UserBookmarks
                .Include(x => x.User)
                .Include(x => x.Bookmark).ThenInclude(x => x.Webpage)
                .Where(x => x.UserId == userId).ToList();

        }

        public UserSessionsHistory GetCurrentSession(int userId)
        {
            db = new MVContext();

            var session = db.SessionHistory.FromSqlInterpolated($"select * from get_session({userId}) limit 1").First();


            return session;
        }

        public UserTitleReview GetReview(int reviewId)
        {
            db = new MVContext();
            var review = db.UserReviews.Where(x => x.ReviewId == reviewId).FirstOrDefault();

            if (review == null)
            {
                return null;
            }

            return review;
        }

        public List<UserTitleReview> GetReviews(int userId)
        {
            db = new MVContext();
            var reviews = db.UserReviews
                .Include(x => x.User)
                .Include(x => x.Title)
                .Include(x => x.Review)
                .ThenInclude(x => x.UserLikes)
                .Where(x => x.UserId == userId)
                .ToList();
            return reviews;
        }

        public Bookmark CreateBookmark()
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId)
        {
            db = new MVContext();

            db.Users.Remove(db.Users.Single(x => x.Id == userId));
            var dbChanged = db.SaveChanges() > 0;

            return dbChanged;
        }

        public bool DeleteReview(int reviewId)
        {
            db = new MVContext();

            db.Reviews.Remove(db.Reviews.Single(x => x.Id == reviewId));
            var dbChanged = db.SaveChanges() > 0;

            return dbChanged;
        }

<<<<<<< Updated upstream
        public bool DeleteBookmark(string bookmarkId)
        {
            db = new MVContext();

            db.Bookmarks.Remove(db.Bookmarks.Single(x => x.Id == bookmarkId));
            var dbChanged = db.SaveChanges() > 0;

            return dbChanged;
        }


        public bool UpdateEmail(int userId, string email)
=======
        public bool UpdateEmail(string username, string email)
>>>>>>> Stashed changes
        {
            db = new MVContext();

            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();
            user.Email = email;
            var updatedEmail = db.SaveChanges() > 0;

            return updatedEmail;
        }
    }
}
