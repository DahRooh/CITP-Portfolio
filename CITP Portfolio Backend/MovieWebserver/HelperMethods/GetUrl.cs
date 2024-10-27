using Microsoft.AspNetCore.Mvc;

namespace MovieWebserver.Controllers;


public class GetUrl
{
    private readonly LinkGenerator _linkGenerator;

    public GetUrl(LinkGenerator linkGenerator)
    {
        _linkGenerator = linkGenerator;
    }

    public string? GetWebpageUrl(HttpContext httpContext, string id, string methodName) // Update order here
    {
        return _linkGenerator.GetUriByName(httpContext, methodName, new { id });
    }
}

