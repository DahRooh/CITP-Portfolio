using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Search
{
    public class SearchResultPersonModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public double? PersonRating { get; set; }
        public string Type { get; set; }
    }
}
