using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using MovieWebserver.Model.Title;

namespace MovieWebserver.Model.User
{
    public class BookmarkModel
    {
        public string Title { get; set; }
        public string WebpageId { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
