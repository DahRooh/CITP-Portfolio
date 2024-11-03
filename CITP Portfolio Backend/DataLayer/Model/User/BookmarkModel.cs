using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using MovieWebserver.Model.Title;

namespace MovieWebserver.Model.User
{
    public class BookmarkModel
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
