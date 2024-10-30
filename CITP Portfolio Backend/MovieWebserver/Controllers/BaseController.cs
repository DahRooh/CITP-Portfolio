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
    public string? GetWebpageUrl(string entityName, string controllerName, object args) // Update order here
    {
        var url = _linkGenerator.GetUriByAction(
                    action: entityName,
                    controller: controllerName,
                    values: args,
                    httpContext: HttpContext);
        
        return url;
    }

    [NonAction]
    public string? GetWebpageUrlByAction(string nameOf, string id, string controller) 
    {
        var url = _linkGenerator.GetUriByAction(
                    action: nameOf,
                    controller: controller,
                    values: new { id },
                    httpContext: HttpContext
                    );

        return url;
    }

    [NonAction]
    public string? GetPageLink(string entityName, string controller, object args)
    {
        return GetWebpageUrl(entityName, controller, args);
    }

    [NonAction]
    public object CreatePaging<T>(string entityName, string controller, int page, int pageSize, int total, IEnumerable<T?> items, string args = null)
    {
        const int MaxItemPerPage = 20;
        pageSize = pageSize > MaxItemPerPage ? MaxItemPerPage : pageSize;
        int TotalNumberOfPages = (int)Math.Ceiling((double)total / pageSize);
        int nextPageNumber = page + 1;
        int previousPageNumber = page - 1;


        var CurrentPage = GetPageLink(entityName, controller, new { keywords = args, page, pageSize });
        var PreviousPage = previousPageNumber < 1 ? null : GetPageLink(entityName, controller, new { keywords = args, page = previousPageNumber, pageSize });
        var NextPage = nextPageNumber > TotalNumberOfPages ? null : GetPageLink(entityName, controller, new { keywords = args, page = nextPageNumber, pageSize });

        Console.WriteLine("entity name " + entityName);
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