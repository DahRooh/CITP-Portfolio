using DataLayer.DomainObjects;
using DataLayer.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.HelperMethods;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using DataLayer.DomainObjects.FunctionResults;
using Microsoft.AspNetCore.Http;

namespace DataLayer.DataServices
{
    public class UserDataService : IUserDataService
    {
        private MVContext db;

        public bool CreateBookmark(string titleId, int userId)
        {
            db = new MVContext();
            throw new NotImplementedException();
        }

        public bool CreateReview(string titleId, string ReviewText)
        {
            throw new NotImplementedException();
        }



        public bool LikeReview(int reviewId, int userId, int like)
        {
            throw new NotImplementedException();
        }


        public List<Search> GetHistory(int userId)
        {
            throw new NotImplementedException();
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

        public List<User> GetUsers()
        {
            db = new MVContext();
            var users = db.Users.ToList();
            return users;
        }

        public User GetUser(int userId)
        {
            db = new MVContext();
            Console.WriteLine("userid" + userId);
            var users = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            return users;
        }

        public List<Bookmark> GetBookmarks(int userId)
        {
            db = new MVContext();
            return db.Bookmarks.Where(x => x.Id == userId).ToList();

        }

        public UserSessionsHistory GetCurrentSession(int userId)
        {
            db = new MVContext();

            var session = db.SessionHistory.FromSqlInterpolated($"select * from get_session({userId}) limit 1").First();


            return session;
        }

        public Review GetReview(int reviewId)
        {
            db = new MVContext();
            var review = db.Reviews.Where(x => x.Id == reviewId).FirstOrDefault();

            if (review == null)
            {
                return null;
            }

            return review;
        }


        public List<Review> GetReviews(string titleId)
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
        public bool CreateUser(string username, string email, string password)
        {
            throw new NotImplementedException();

            db = new MVContext();

            db.Database.ExecuteSqlInterpolated($"call signup({username},{password},{email})");
        }


    }
}
