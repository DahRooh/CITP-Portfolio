using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataLayer.DomainObjects;

public class UserReview
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int ReviewId { get; set; }
    public Review Review { get; set; }
}
