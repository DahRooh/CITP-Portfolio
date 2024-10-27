using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class Search
{
    public string Id { get; set; }
    public string Keyword { get; set; }
    public DateTime CreatedAt { get; set; }
    

    public UserSearch UserSearch { get; set; }
}
