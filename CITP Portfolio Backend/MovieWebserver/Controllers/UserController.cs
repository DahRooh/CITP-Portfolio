using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using DataLayer.IDataServices;

namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : BaseController
{
    IUserDataService _ds;
    public UserController(IUserDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
    }


}
