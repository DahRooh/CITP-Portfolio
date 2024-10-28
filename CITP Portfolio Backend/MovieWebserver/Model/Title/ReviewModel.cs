using DataLayer.DomainObjects.Relations;
using DataLayer.DomainObjects;
using MovieWebserver.Model.User;

namespace MovieWebserver.Model.Title
{
    public class ReviewModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Likes { get; set; }

        public IList<UserModel> UserLikes { get; set; }

        public UserModel createdBy { get; set; }
    }
}
