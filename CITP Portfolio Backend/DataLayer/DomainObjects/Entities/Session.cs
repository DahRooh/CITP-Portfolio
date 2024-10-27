using DataLayer.DomainObjects.Relations;

namespace DataLayer.DomainObjects;

public class Session
{
    public int Id { get; set; }
    public DateTime SessionStart { get; set; }
    public DateTime SessionEnd { get; set; }
    public string Expiration { get; set; }
    public UserSession? UserSessions { get; set; }
}