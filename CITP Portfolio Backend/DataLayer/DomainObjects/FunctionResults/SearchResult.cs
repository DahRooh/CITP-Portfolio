using DataLayer.DomainObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Relations
{
    public class SearchResult
    {
        public double Relevance { get; set; }
        public string WebpageId { get; set; }
        
    }
}

