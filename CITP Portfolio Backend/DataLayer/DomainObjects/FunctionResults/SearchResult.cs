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
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
        public string? Poster { get; set; }
        public double? Rating { get; set; }

    }
}

