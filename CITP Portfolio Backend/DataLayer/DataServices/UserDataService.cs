using DataLayer.DomainObjects;
using DataLayer.IDataServices;
using Microsoft.EntityFrameworkCore;
using DataLayer.DomainObjects.FunctionResults;
using MovieWebserver.Model.User;
using MovieWebserver.Model.Title;


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


        public bool CreateBookmark(string titleId, int userId)
        {
            db = new MVContext();
            throw new NotImplementedException();
        }


        
        public User CreateUser(UserModel user)
        {
            db = new MVContext();

            var doesUserExist = db.Users.Where(x => x.Username == user.Username).FirstOrDefault();

            Console.WriteLine(doesUserExist);
            if (doesUserExist == null)
            {
                db.Database.ExecuteSqlRaw("call signup({0}, {1}, {2}, {3})", user.Username, user.Password, user.Email, null);
                var newUser = db.Users.Where(x => x.Username == user.Username).First();
                return newUser;
            } 
            else
            {
                return null;
            }
            
            
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


        public List<UserSearch> GetHistory(int userId)
        {
            db = new MVContext();
            User user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            if (user == null)
            {
                return null;
            }
            return db.UserSearches.Include(x => x.Search).Where(x => x.UserId == userId).ToList();
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
            return db.Bookmarks.Where(x => x.UserId == userId).ToList();

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
            var reviews = db.UserReviews.Include(x => x.User).Include(x => x.Title).Include(x => x.Review).ThenInclude(x => x.UserLikes).ToList();
            return reviews;
        }
        
        public Bookmark CreateBookmark()
        {
            throw new NotImplementedException();
        }

        public bool DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEmail(int userId, string email)
        {
            throw new NotImplementedException();

        }


        // authentication

        public bool LogIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool LogOff(string username)
        {
            throw new NotImplementedException();
        }



    }
}
