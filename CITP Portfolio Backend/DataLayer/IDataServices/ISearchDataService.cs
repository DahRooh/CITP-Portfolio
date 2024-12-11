using DataLayer.DomainObjects.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace DataLayer.IDataServices;

public interface ISearchDataService
{
    public IList<SearchResult> Search(string keyword, int page, int pageSize, string userId = "");
    public IList<SearchResultPerson> SearchPerson(string keyword, int page, int pageSize, string username = "");
    public int SearchCount(string keyword);
    public int SearchPersonCount(string keyword);
    public Webpage GetWebpage(string webpageId);
}
