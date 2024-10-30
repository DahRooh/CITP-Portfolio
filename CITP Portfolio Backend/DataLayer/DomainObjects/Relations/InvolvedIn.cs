using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer.DomainObjects;

public class InvolvedIn
{
    public string PersonId { get; set; }
    public string TitleId { get; set; }
    public string? Character { get; set; }
    public string Job { get; set; }


    public Person? Person { get; set; }
    public Title? Title { get; set; }
}