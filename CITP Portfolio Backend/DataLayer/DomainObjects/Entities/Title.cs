﻿using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class Title 
{
    public string Id { get; set; }
    public string _Title { get; set; } = string.Empty;
    public string? Plot { get; set; }
    public double Rating { get; set; } = 0;
    public string Type { get; set; }
    public string Titletype { get; set; }
    public bool? IsAdult { get; set; }
    public string? Released { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public int? RunTime { get; set; }
    public string? Poster { get; set; }


    // from relations
    [JsonIgnore]
    public IList<TitleGenre> Genres { get; set; } = new List<TitleGenre>();
    public IList<InvolvedIn>? PeopleInvolved { get; set; } = new List<InvolvedIn>();
    public IList<UserTitleReview>? Reviews { get; set; }

   // public Title MovieOrTitle { get; set; }

}
