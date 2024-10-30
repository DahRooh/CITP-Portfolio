using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Entities;

namespace DataLayer.IDataServices;

public interface ISearchDataService
{
    public IList<SearchResult> Search(string keyword, int take, int skip = 0, int userId = -1);
    public Webpage GetWebpage(string webpageId);
}
