using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using DataLayer.IDataServices;
using DataLayer.DomainObjects.FunctionResults;
using MovieWebserver.Model;
using MovieWebserver.Model.User;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.User;
using DataLayer.HelperMethods;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : BaseController
{
    IUserDataService _ds;
    private readonly IConfiguration _configuration;
    private readonly Hashing _hashing;

    public UserController(
        IUserDataService ds,
        LinkGenerator linkGenerator,
        IConfiguration configuration,
        Hashing hashing) : base(linkGenerator)
    {
        _ds = ds;
        _configuration = configuration;
        _hashing = hashing;
    }

    [HttpPut]
    public IActionResult SignIn(LoginUserModel model)
    {
        var user = _ds.GetUser(model.Username);
        if (user == null)
        {
            return BadRequest("Wrong username or password");
        }



        if (!_hashing.Verify(model.Password, user.Password, user.Salt))
        {
            return BadRequest();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };

        var secret = _configuration.GetSection("Auth:Secret").Value;
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secret));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(8),
            signingCredentials: creds

            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(new { username = user.Username, token = jwt });
    }

    [HttpPost("CreateUser")]
    public IActionResult CreateUser([FromBody] CreateUserModel userModel)
    {

        var user = _ds.GetUser(userModel.Username);

        if (user != null || 
            string.IsNullOrEmpty(userModel.Password) ||
            _ds.IsEmailUsed(userModel.Email)
            )
        {
            return BadRequest();
        }
        (var hashedPassword, var salt) = _hashing.Hash(userModel.Password);
        userModel.Password = hashedPassword;


        var result = _ds.CreateUser(userModel, salt);


        return Ok(result);
    }

    [HttpGet("{userId}", Name = nameof(GetUser))]
    public IActionResult GetUser(int userId)
    {
        var user = CreateUserModel(_ds.GetUser(userId));
        if (user == null)
        {
            return BadRequest();
        }
        return Ok(user);
    }

    [HttpGet(Name = nameof(GetUsers))]
    
    public IActionResult GetUsers()
    {
        var users = _ds.GetUsers().Select(x => CreateUserModel(x)).ToList();

        if (users == null || users.Count() == 0)
        {
            return BadRequest();
        }
        return Ok(users);
    }
    /*
    [HttpGet("{userId}/sessions", Name = nameof(GetUserSessions))]
    public IActionResult GetUserSessions(int userId)
    {
        var sessions = _ds.GetSessions(userId).Select(x => CreateSessionModel(x));

        var UserSessions = sessions.Select(session => 
        {
            var user = CreateUserModel(_ds.GetUser(session.UserId));
            session.User = user;
            return session;
                
        });
        

        if (UserSessions.Count() == 0)
        {
            return NotFound();
        }

        return Ok(UserSessions);

    }
    */

    /*
    [HttpGet("{userId}/current_session", Name = nameof(GetCurrentUserSessions))]
    public IActionResult GetCurrentUserSessions(int userId)
    {
        var session = CreateSessionModel(_ds.GetCurrentSession(userId));

        if (session == null)
        {
            return NotFound();
        }

        var user = CreateUserModel(_ds.GetUser(session.UserId));
        session.User = user;

        return Ok(session);
    }
    */


    [HttpGet("{userId}/bookmarks", Name = nameof(GetUserBookmarks))]
    public IActionResult GetUserBookmarks(int userId)
    {
        var bookmarks = _ds.GetBookmarks(userId).Select(x => CreateBookmarkModel(x)).ToList();
        if (bookmarks.Count() == 0)
        {
            return BadRequest();
        }

        return Ok(bookmarks);
    }




    [HttpGet("{userId}/Reviews", Name = nameof(GetUserReviews))]
    public IActionResult GetUserReviews(int userId)
    {
        var reviews = _ds.GetReviews(userId).Select(x => CreateReviewModel(x)).ToList();

        if (reviews.Count() == 0)
        {
            return NotFound();
        }

        return Ok(reviews);
    }

    [HttpGet("{userId}/search_history", Name = nameof(GetUserHistory))]
    public IActionResult GetUserHistory(int userId)
    {
        var searches = _ds.GetHistory(userId).Select(x => CreateSearchModel(x)).ToList();

        if (searches.Count() == 0)
        {
            return NotFound();
        }

        return Ok(searches);

    }


    // models
    public static ReviewModel CreateReviewModel(UserTitleReview review)
    {
        var model = review.Adapt<ReviewModel>();


        return model;
    }

    private SearchModel CreateSearchModel(UserSearch userSearch)
    {
        return userSearch.Adapt<SearchModel>();
    }
    public static UserModel CreateUserModel(User user)
    {
        return user.Adapt<UserModel>();
    }

    public static SessionModel CreateSessionModel(UserSessionsHistory session)
    {
        return session.Adapt<SessionModel>();
    }

    public static BookmarkModel CreateBookmarkModel(UserBookmark bookmark)
    {
        return bookmark.Adapt<BookmarkModel>();
    }


    private static TitleModel CreateTitleModel(Title title)
    {
        return title.Adapt<TitleModel>();
    }
}
