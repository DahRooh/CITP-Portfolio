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

    [HttpGet("{userId}/sessions", Name = nameof(GetUserSessions))]
    public IActionResult GetUserSessions(int userId)
    {
        var sessions = _ds.GetSessions(userId).Select(x => CreateSessionModel(x));

        var UserSessions = sessions.Select(session => 
        {
            var user = CreateUserModel(_ds.GetUser(session.UserId));
            session.User = user;
            Console.WriteLine(session.User);
            return session;
                
        });
        

        if (UserSessions.Count() == 0)
        {
            return NotFound();
        }

        return Ok(UserSessions);

    }

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
    [HttpGet("{userId}/Reviews/{reviewId}", Name = nameof(GetUserReviewWithId))]
    public IActionResult GetUserReviewWithId(int userId, int reviewId)
    {
        var review = _ds.GetReview(reviewId);

    }


    private UserModel CreateUserModel(User user)
    {
        return user.Adapt<UserModel>();
    }

    public static SessionModel CreateSessionModel(UserSessionsHistory session)
    {
        return session.Adapt<SessionModel>();
    }

    public static BookmarkModel CreateBookmarkModel(Bookmark bookmark)
    {
        return bookmark.Adapt<BookmarkModel>();
    }
    public static ReviewModel CreateReviewModel(Review review)
    {
        return review.Adapt<ReviewModel>();
    }



}
