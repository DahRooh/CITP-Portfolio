using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using MovieWebserver.Model.Title;
using System.Reflection.Metadata;

namespace DataLayer.Model.User
{
    public class BookmarkModel
    {
        public string Id { get; set; }
        public string TitleId { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }
        public string Poster { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
