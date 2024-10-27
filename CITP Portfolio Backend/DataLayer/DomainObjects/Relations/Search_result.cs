using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DomainObjects.Relations
{
    public class Search_result
    {
        public string SearchId { get; set; }
        public string PersonId { get; set; }
        public string TitleId { get; set; }
        public string Keyword { get; set; }

        public DateTime SearchedAt { get; set; }


        public Search Search { get; set; }
        public Person Person { get; set; }
        public Title Title { get; set; }


    }
}
