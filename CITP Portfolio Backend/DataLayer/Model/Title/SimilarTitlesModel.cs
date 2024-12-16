using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class SimilarTitlesModel
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string _Title { get; set; }
        public int CommonGenres { get; set; }
        public double? Rating { get; set; } = 0;
    }
}
