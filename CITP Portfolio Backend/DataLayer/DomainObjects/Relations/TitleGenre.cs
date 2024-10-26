using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Relations
{
    public class TitleGenre
    {
        public string TitleId { get; set; }
        public Title Title { get; set; }
            
        public string GenreName { get; set; }
        public Genre Genre { get; set; }
    }
}
