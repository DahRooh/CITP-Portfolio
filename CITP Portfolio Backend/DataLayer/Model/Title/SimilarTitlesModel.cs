using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class SimilarTitlesModel
    {
        public string Url { get; set; }
        public string TitleId { get; set; }
        public string _Title { get; set; }
        public int AmountOfSimilarGenres { get; set; }
    }
}
