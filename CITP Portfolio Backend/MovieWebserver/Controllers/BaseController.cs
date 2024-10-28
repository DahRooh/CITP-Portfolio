using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Npgsql;
namespace MovieWebserver.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    private readonly LinkGenerator _linkGenerator;

    public BaseController(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    [NonAction]
    public string? GetWebpageUrl(string entityName, object args) // Update order here
    {
        return _linkGenerator.GetUriByName(HttpContext, entityName, args);
    }

    [NonAction]
    public string? GetPageLink(string entityName, object args)
    {
        return GetWebpageUrl(entityName, args);
    }

    [NonAction]
    public object CreatePaging<T>(string entityName, int page, int pageSize, int total, IEnumerable<T?> items)
    {
        const int MaxItemPerPage = 20;
        pageSize = pageSize > MaxItemPerPage ? MaxItemPerPage : pageSize;
        int TotalNumberOfPages = (int)Math.Ceiling((double)total / pageSize);
        int nextPageNumber = page + 1;
        int previousPageNumber = page - 1;
        
        var CurrentPage = GetPageLink(entityName, new { page, pageSize });
        var PreviousPage = previousPageNumber < 0 ? null : GetPageLink(entityName, new { previousPageNumber, pageSize });
        var NextPage = nextPageNumber > TotalNumberOfPages ? null : GetPageLink(entityName, new { nextPageNumber, pageSize });

        var result = new
        {
            CurrentPage = CurrentPage,
            PreviousPage = PreviousPage,
            NextPage = NextPage,
            Items = items,
            TotalNumberOfPages = TotalNumberOfPages,
            MaxItemPerPage = MaxItemPerPage
        };
        return result;
    }


}