﻿namespace MovieWebserver.Model.Title
{
    public class MovieModel
    { 
        public string Id { get; set; }
        public string Url { get; set; }
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
    }
}
