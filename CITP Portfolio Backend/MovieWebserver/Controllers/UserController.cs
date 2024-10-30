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



    [HttpPost("CreateUser")]
    public IActionResult CreateUser([FromBody] UserModel userModel)
    {
        var result = _ds.CreateUser(userModel);
        if (result != null) 
        {
            NotFound();
        }

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
        model.Title = CreateTitleModel(review.Title);

        var allUsersThatLiked = review.Review.UserLikes.Select(x => CreateUserModel(x.User)).ToList();
        model.UserLikes = allUsersThatLiked;

        model.CreatedBy = CreateUserModel(review.User);


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
