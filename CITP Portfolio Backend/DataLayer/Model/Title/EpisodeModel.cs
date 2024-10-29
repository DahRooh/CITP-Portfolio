﻿using DataLayer;
using MovieWebserver.Model.Person;

namespace MovieWebserver.Model.Title;


public class EpisodeModel
{
    public string Url { get; set; }
    public int? SeasonNumber { get; set; }
    public int? EpisodeNumber { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Plot { get; set; }
    public double? Rating { get; set; } = 0;
    public bool? IsAdult { get; set; }
    public string? Released { get; set; }
    public string? Language { get; set; }
    public string? Country { get; set; }
    public int? RunTime { get; set; }
    public string? Poster { get; set; }



}