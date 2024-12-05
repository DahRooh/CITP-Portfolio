using DataLayer.DomainObjects.Relations;
using DataLayer.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using DataLayer.DomainObjects;

namespace DataLayer.DataServices
{
    public class SearchDataService : ISearchDataService
    {
        private MVContext db;
        public IList<SearchResult> Search(string keyword, int page, int pageSize, string username = "")
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Username == username).FirstOrDefault();
            
            var skip = pageSize*page;

            if (user != null)
            {
                db.Database.ExecuteSqlRaw("call insert_search({0}, {1}, {2})", keyword, user.Id, username);
            }

            var results = db.SearchResults.
                FromSqlRaw("select * from make_search({0}, {1}, {2})", keyword, pageSize, skip).OrderByDescending(r => r.Relevance).ToList();

            return results;
        }


        public int SearchCount(string keyword)
        {
            return db.SearchResults.FromSqlRaw("select * from make_search({0})", keyword).Count();
        }

        public Webpage GetWebpage(string webpageId)
        {
            db = new MVContext();
            var webpage = db.Webpages.Include(x => x.Title)
                .Where(x => x.Id.Equals(webpageId)).FirstOrDefault();

            if (webpage == null)
            {
                return null;
            }
            return webpage;
        }
    }
}

