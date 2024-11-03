using DataLayer.DomainObjects;
using DataLayer.DomainObjects.Entities;
using MovieWebserver.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.User
{
    public class SearchModel
    {
        public string SearchId { get; set; }

        public string Keyword { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
