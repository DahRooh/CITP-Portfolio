namespace DataLayer.DomainObjects.Relations;

public class UserLikesReview
{
    public int UserId { get; set; }
    public int ReviewId { get; set; }
    public int Liked { get; set; } = 0;
    
    
    public User User { get; set; }
    public Review Review { get; set; }
}