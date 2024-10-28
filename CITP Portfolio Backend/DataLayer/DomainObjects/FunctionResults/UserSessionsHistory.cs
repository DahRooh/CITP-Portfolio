using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.FunctionResults
{
    public class UserSessionsHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime? SessionEnd { get; set; }
        public string? Expiration { get; set; }

        public override string? ToString()
        {
            return $"Session ID: {Id}, User ID: {UserId}, " +
                   $"Session Start: {SessionStart}, " +
                   $"Session End: {SessionEnd?.ToString() ?? "N/A"}, " +
                   $"Expiration: {Expiration}, ";
        }
    }
}
