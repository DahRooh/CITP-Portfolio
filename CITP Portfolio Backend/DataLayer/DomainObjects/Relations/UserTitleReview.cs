using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataLayer.DomainObjects;

public class UserTitleReview
{
    public int UserId { get; set; }
    public string TitleId { get; set; }
    public int ReviewId { get; set; }
    public double Rating { get; set; }
    
    
    public User User { get; set; }
    public Title Title { get; set; }
    public Review Review { get; set; }
}
