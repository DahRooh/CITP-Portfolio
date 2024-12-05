using DataLayer.DomainObjects;
using DataLayer.IDataServices;
using Microsoft.EntityFrameworkCore;
using DataLayer.DomainObjects.FunctionResults;
using MovieWebserver.Model.User;
using MovieWebserver.Model.Title;
using DataLayer.Model.User;
using System.Linq;
using DataLayer.DomainObjects.Relations;
using DataLayer.HelperMethods;


namespace DataLayer.DataServices
{
    public class UserDataService : IUserDataService
    {
        private MVContext db;
        private Hashing _hashing;
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





        //public User CreateUser(CreateUserModel user, string salt)
        public User CreateUser(CreateUserModel user)
        {
            db = new MVContext();
            _hashing = new Hashing();

            (var hashedPassword, var salt) = _hashing.Hash(user.Password);
            user.Password = hashedPassword;

            var newId = db.Users.FirstOrDefault() == null ? 1 : db.Users.Max(x => x.Id) + 1;

            db.Database.ExecuteSqlRaw("call signup({0}, {1}, {2}, {3}, {4})", newId, user.Username, user.Password,
                user.Email, salt);
            var newUser = db.Users.Where(x => x.Username == user.Username).First();

            if (newUser == null) return null;

            return newUser;

        }

        public bool LikeReview(int reviewId, int userId, int like)
        {
            throw new NotImplementedException();
        }

        public IList<UserLikesReview> GetLikes(int userId)
        {
            db = new MVContext();
            var likedList = db.UserLikesReviews
                .Include(x => x.Review).ThenInclude(x => x.createdBy).ThenInclude(x => x.Title)
                .Include(x => x.User)
                .AsSplitQuery()
                .Where(x => x.UserId == userId).ToList();

            return likedList;
        }
        public List<UserSearch> GetHistory(int userId)
        {
            db = new MVContext();
            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            return db.UserSearches.Include(x => x.User)
                .Include(x => x.Search)
                .AsSplitQuery()
                .Where(x => x.UserId == userId).ToList();

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

        public List<Bookmark> GetBookmarks(int userId)
        {
            db = new MVContext();
            return db.Bookmarks
                .Include(x => x.WebpageBookmark).ThenInclude(x => x.Webpage)
                .ThenInclude(x => x.Title)
                .Include(x => x.BookmarkedBy)
                .AsSplitQuery()

                .Where(x => x.BookmarkedBy.UserId == userId).ToList();

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
                .AsSplitQuery()

                .Where(x => x.UserId == userId)
                .ToList();
            return reviews;
        }


        public bool DeleteUser(int userId)
        {
            db = new MVContext();

            var bookmark_ids = db.UserBookmarks.Where(x => x.UserId == userId).Select(x => x.BookmarkId).ToList();
            var review_ids = db.UserReviews.Where(x => x.UserId == userId).Select(x => x.ReviewId).ToList();
            var search_ids = db.UserSearches.Where(x => x.UserId == userId).Select(x => x.SearchId).ToList();


            db.Bookmarks.RemoveRange(db.Bookmarks.Where(x => bookmark_ids.Contains(x.Id)));
            db.Reviews.RemoveRange(db.Reviews.Where(x => review_ids.Contains(x.Id)));
            db.Searches.RemoveRange(db.Searches.Where(x => search_ids.Contains(x.Id)));
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


        public bool DeleteBookmark(string bookmarkId)
        {
            db = new MVContext();

            db.Bookmarks.Remove(db.Bookmarks.Single(x => x.Id == bookmarkId));
            var dbChanged = db.SaveChanges() > 0;

            return dbChanged;
        }



        public bool UpdateEmail(string username, string email)
        {
            db = new MVContext();

            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();
            user.Email = email;
            var updatedEmail = db.SaveChanges() > 0;

            return updatedEmail;
        }

        public bool UpdatePassword(int userId, string password)
        {
            db = new MVContext();

            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            
            if (user.Password != password) // Password variable should already be hashed, if no input
            {
                _hashing = new Hashing();
                var hashedPassword = _hashing.Hash(password); 
                user.Password = hashedPassword.hash;
                user.Salt = hashedPassword.salt;

            }
 
            return db.SaveChanges() > 0;


        }


    }
}
