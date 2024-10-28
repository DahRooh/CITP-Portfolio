using MovieWebserver.Model.User;

namespace MovieWebserver.Model;

public class SessionModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserModel User { get; set; }
    public DateTime SessionStart { get; set; }
    public DateTime? SessionEnd { get; set; }
    public string? Expiration { get; set; }
}
