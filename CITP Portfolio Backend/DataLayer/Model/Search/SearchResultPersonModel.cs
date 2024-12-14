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
        public string Text { get; set; }
        public double? Rating { get; set; }
        public string Type { get; set; }
    }
}
