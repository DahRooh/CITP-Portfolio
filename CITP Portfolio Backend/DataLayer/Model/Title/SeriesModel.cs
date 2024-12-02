using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model.Title
{
    public class SeriesModel
    {
        public string Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Plot { get; set; }
        public double? Rating { get; set; } = 0;
        public string Type { get; set; }
        public bool? IsAdult { get; set; }
        public string? Released { get; set; }
        public string? Language { get; set; }
        public string? Country { get; set; }
        public int? RunTime { get; set; }
        public string? Poster { get; set; }
        public string Titletype { get; set; }
        public int? SeasonNum { get; set; }
        public int? EpisodeNum { get; set; }
    }
}
