using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class UserSearch
{
    public User User { get; set; }
    public Search Search { get; set; }
}
