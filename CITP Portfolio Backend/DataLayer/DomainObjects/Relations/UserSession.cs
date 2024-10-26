using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLayer.DomainObjects.Relations;

public class UserSession
{
    public int UserId { get; set; }
    public int SessionId { get; set; }


    public User User { get; set; }
    public Session Session { get; set; }
}