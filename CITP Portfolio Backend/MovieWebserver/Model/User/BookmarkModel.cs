using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using MovieWebserver.Model.Title;

namespace MovieWebserver.Model.User
{
    public class BookmarkModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public string Name { get; set; }
        public string Url { get; set; }



        public UserModel BookmarkedBy { get; set; }
    }
}
