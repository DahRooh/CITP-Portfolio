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
    public string PersonId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    // public int Age { get; set; }
    public double? Rating { get; set; } = 0;

    public IList<PersonProfession> Professions { get; set; } = new List<PersonProfession>();

    public IList<PersonInvolvedIn> InvolvedIn { get; set; } = new List<PersonInvolvedIn>();

    
}




