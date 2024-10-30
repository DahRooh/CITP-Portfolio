using DataLayer.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieWebserver.Model.Title;

public class CastModel
{
    public string Url { get; set; }
    public string TitleName { get; set; }
    public string Person { get; set; }
    public string Character { get; set; }

}
