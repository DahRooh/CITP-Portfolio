using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public class Title
{
    public string Id { get; set; }
    public string _Title { get; set; } = string.Empty;
    public string? Plot { get; set; }
    public double? Rating { get; set; } = 0;
    public string Type { get; set; }
    public bool? IsAdult { get; set; }
    public string? Released { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public int? RunTime { get; set; }
    public string? Poster { get; set; }


    // from relations

    public List<Genre> Genres { get; set; }
    public List<PersonInvolvedIn> PeopleInvolved { get; set; }
}
