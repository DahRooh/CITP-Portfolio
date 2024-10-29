using DataLayer.DomainObjects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Relations
{
    public class WebpageSearch
    {
        public Webpage Webpage { get; set; }
        public Search Search { get; set; }

        public double Frequency { get; set; }

        public string WebpageId { get; set; }
        public string SearchId { get; set; }

    }
}
