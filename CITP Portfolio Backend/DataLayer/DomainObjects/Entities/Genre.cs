using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects;

public class Genre
{
    [JsonIgnore]
    public List<TitleGenre> Titles { get; set; }
    public string _Genre { get; set; }
}
