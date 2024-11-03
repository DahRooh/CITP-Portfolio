using DataLayer.DomainObjects.Entities;
using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class Bookmark
{
    public string Id { get; set; }
    public DateTime CreatedAt { get; set; }


    [JsonIgnore]
    public UserBookmark BookmarkedBy { get; set; }
    public WebpageBookmark WebpageBookmark { get; set; }

}
