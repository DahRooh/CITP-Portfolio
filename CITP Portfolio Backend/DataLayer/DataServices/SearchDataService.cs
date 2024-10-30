﻿using DataLayer.DomainObjects.Relations;
using DataLayer.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DataServices
{
    public class SearchDataService : ISearchDataService
    {
        private MVContext db;
        public IList<SearchResult> Search(string keyword, int take, int skip = 0, int userId = -1)
        {
            db = new MVContext();
            var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();

            if (user != null)
            {
                db.Database.ExecuteSqlRaw("call insert_search({0}, {1}, {2})", keyword, userId, null);
            }

            var results = db.SearchResults.
                FromSqlRaw("select * from make_search({0}, {1}, {2})", keyword, take, skip).ToList();

            return results;
        }
    }
}

// keyword, user_id, new_search_id