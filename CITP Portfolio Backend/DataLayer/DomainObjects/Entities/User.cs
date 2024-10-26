namespace DataLayer.DomainObjects;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }

    public List<UserReview> Reviews { get; set; }
}