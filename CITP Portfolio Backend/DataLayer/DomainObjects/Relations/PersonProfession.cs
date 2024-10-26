using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataLayer.DomainObjects;

public class PersonProfession
{
    public string PersonId {  get; set; }
    public Person Person {  get; set; }
    public string ProfessionName {  get; set; }
    public Profession Profession {  get; set; }

}
