using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Relations;

namespace DataLayer.DomainObjects;

public class Review
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int Likes { get; set; }
    public IList<UserLikesReview> UserLikes { get; set; }
    public UserTitleReview createdBy { get; set; }

}
