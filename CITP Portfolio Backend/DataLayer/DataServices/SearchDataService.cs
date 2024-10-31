﻿using DataLayer.DomainObjects.Relations;
using DataLayer.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Entities;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class SearchDataService : ISearchDataService
    {
        private MVContext db;
        public IList<SearchResult> Search(string keyword, int page, int pageSize, int userId = -1)
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            
            var skip = pageSize*page;

            if (user != null)
            {
                db.Database.ExecuteSqlRaw("call insert_search({0}, {1}, {2})", keyword, userId, null);
            }

            var results = db.SearchResults.
                FromSqlRaw("select * from make_search({0}, {1}, {2})", keyword, pageSize, skip).ToList();

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
