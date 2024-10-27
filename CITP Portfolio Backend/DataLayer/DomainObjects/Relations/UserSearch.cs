using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class UserSearch
{
    public int UserId { get; set; }
    public int SearchId { get; set; }

    public User User { get; set; }
    public Search Search { get; set; }
}
