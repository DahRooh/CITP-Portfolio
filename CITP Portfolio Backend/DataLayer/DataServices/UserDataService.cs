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

        public List<Session> GetSessions(int userId)
        {
            Console.WriteLine("diin");
            var result = new List<Session>();
            db = new MVContext();
            var connection = (NpgsqlConnection)db.Database.GetDbConnection();

            var results = Helpers.ExecuteFunctionSQL(connection, $"select * from get_session({userId})");

            Console.WriteLine("WOWO");
            foreach (var resultitem in results)
            {
                var session = new Session()
                {
                    Id = (int)resultitem["session_id"],
                    UserId = (int)resultitem["user_id"],

                };
                result.Add(session);
            }
            return result;

        }

        public List<User> GetUser()
        {
            throw new NotImplementedException();
        }

        public User GetUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Bookmark> GetBookmarks(int userId)
        {
            throw new NotImplementedException();
        }

        public Session GetCurrentSession(int userId)
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

            //            db = new MVContext();
            //           var sql = "select * from {}"
            //         Helpers.ExecuteFunctionSQL(db, sql);
        }
    }
}
