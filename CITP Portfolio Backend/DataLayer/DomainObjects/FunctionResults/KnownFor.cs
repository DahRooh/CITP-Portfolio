using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.FunctionResults
{
    public class KnownFor
    {
        public string KnownForTitle { get; set; }
        public string KnownForId { get; set; }

        public Title Title { get; set; }

    }
}
