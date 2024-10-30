using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class UserBookmark
{

    public int UserId;
    public string BookmarkId;

    public Bookmark Bookmark { get; set; }
    public User User { get; set; }
}
