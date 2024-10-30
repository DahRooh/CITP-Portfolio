using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Entities
{
    public class Webpage
    {
        public string Id { get; set; }


        public Title Title { get; set; }
        public Person Person { get; set; }

        public string TitleId { get; set; } // person or title, dont matter
        public string PersonId { get; set; } // person or title, dont matter



        public IList<SearchResult> Searches;
        public IList<WebpageBookmark> Bookmarks;
        public string? Url { get; set; } = null;

        public int ViewCount { get; set; } = 0;
    }
}
