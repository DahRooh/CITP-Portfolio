using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataLayer.DomainObjects;

public class Person
{
    [Column("p_id")]
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    public double? PersonRating { get; set; }

    public IList<PersonProfession> Professions { get; set; } = new List<PersonProfession>();

    public IList<InvolvedIn> InvolvedIn { get; set; } = new List<InvolvedIn>();

    
}




