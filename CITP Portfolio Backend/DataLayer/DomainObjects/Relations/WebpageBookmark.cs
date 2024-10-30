using DataLayer.DomainObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Relations
{
    public class WebpageBookmark
    {
        public Webpage Webpage;
        public Bookmark Bookmark;

        public string WebpageId;
        public string BookmarkId;
    }
}
