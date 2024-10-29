using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Entities
{
    public class Webpage
    {
        public int Id { get; set; }

        public Title Title { get; set; }
        public Person Person { get; set; }

        public string RelationId { get; set; } // person or title, dont matter
        public string? Url { get; set; } = null;

        public int ViewCount { get; set; } = 0;
    }
}
