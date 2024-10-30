using DataLayer.DomainObjects.Relations;
using DataLayer.IDataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.DataServices
{
    public class SearchDataService : ISearchDataService
    {
        private MVContext db;
        public IList<SearchResult> Search(string keyword)
        {
            db = new MVContext();

            throw new NotImplementedException();
        }
    }
}
