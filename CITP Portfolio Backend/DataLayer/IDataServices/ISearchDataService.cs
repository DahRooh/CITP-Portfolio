using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DataLayer.IDataServices;

public interface ISearchDataService
{
    public IList<SearchResult> Search(string keyword, int page, int pageSize, int userId = -1);
    public int SearchCount(string keyword);
    public Webpage GetWebpage(string webpageId);
}
