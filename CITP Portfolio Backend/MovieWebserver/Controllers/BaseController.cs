using DataLayer.DataServices;
using DataLayer.DomainObjects;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MovieWebserver.Model.User;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
    public User AuthorizeUser(int userId)
    {
        var _ds = new UserDataService();
        JwtSecurityToken token = GetDecodedToken();
        User user = _ds.GetUser(token.Claims.FirstOrDefault().Value);
        if (user == null || userId != user.Id) return null;
        return user;
    }

    [NonAction]
    public User GetUserLoggedIn()
    {
        var _ds = new UserDataService();
        JwtSecurityToken token = GetDecodedToken();
        User user = _ds.GetUser(token.Claims.FirstOrDefault().Value);
        if (user == null) return null;
        return user;
    }

    [NonAction]
    public JwtSecurityToken CreateToken(User user, IConfiguration _configuration)
    {
        // create claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        // fetch secret from configuration
        var secret = _configuration.GetSection("Auth:Secret").Value;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds

            );
        return token;
    }

    [NonAction]
    public string? GetWebpageUrl(string entityName, string controllerName, object args)
    {
        var url = _linkGenerator.GetUriByAction(
                    action: entityName,
                    controller: controllerName,
                    values: args,
                    httpContext: HttpContext);
        
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


        var CurrentPage = GetPageLink(entityName, controller, new { keyword = args, page, pageSize });

        var PreviousPage = previousPageNumber < 1 ? null : GetPageLink(entityName, controller, new { keyword = args, page = previousPageNumber, pageSize });
        var NextPage = nextPageNumber > TotalNumberOfPages ? null : GetPageLink(entityName, controller, new { keyword = args, page = nextPageNumber, pageSize });

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



    [NonAction]
    public JwtSecurityToken GetDecodedToken()
    {
        var encodedToken = HttpContext.Request.Headers.Authorization.FirstOrDefault();
        if (encodedToken == null) return null;
        var handler = new JwtSecurityTokenHandler();

        var trimmedEncodedToken = encodedToken.Replace("Bearer ", "");

        var token = handler.ReadJwtToken(trimmedEncodedToken);

        return token;
    }
}