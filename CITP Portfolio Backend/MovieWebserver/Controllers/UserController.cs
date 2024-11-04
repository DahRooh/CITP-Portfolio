﻿using Microsoft.AspNetCore.Mvc;
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
using Microsoft.AspNetCore.Authorization;

namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : BaseController
{
    IUserDataService _ds;
    ITitleDataService _titleDs;
    private readonly IConfiguration _configuration;
    private readonly Hashing _hashing;

    public UserController(
        IUserDataService ds,
        ITitleDataService titleDs,
        LinkGenerator linkGenerator,
        IConfiguration configuration,
        Hashing hashing) : base(linkGenerator)
    {
        _ds = ds;
        _titleDs = titleDs;
        _configuration = configuration;
        _hashing = hashing;
    }

    // creates
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



        var result = _ds.CreateUser(userModel);


        return Created(nameof(CreateUser), CreateUserModel(result));
    }


    // gets

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

    [HttpGet("{userId}/likes")]
    [Authorize]
    public IActionResult GetUserLike(int userId)
    {
        var user = AuthorizeUser(userId);
        if (user == null) { return BadRequest(); }

        var likes = _ds.GetLikes(userId)
            .Select(x => CreateLikeModel(x)).ToList();
        
        return Ok(likes);
    }
    
    
    [HttpGet("{userId}/bookmarks", Name = nameof(GetUserBookmarks))]
    public IActionResult GetUserBookmarks(int userId)
    {
        var user = AuthorizeUser(userId);

        if (user == null) { return BadRequest(); }

        var bookmarks = _ds.GetBookmarks(userId).Select(x => CreateBookmarkModel(x)).ToList();

        return Ok(bookmarks);
    }
    

    [HttpGet("{userId}/reviews", Name = nameof(GetUserReviews))]
    public IActionResult GetUserReviews(int userId)
    {
        var user = AuthorizeUser(userId);
        if (user == null) { return BadRequest(); }

        var reviews = _ds.GetReviews(userId).Select(x => CreateReviewModel(x)).ToList();
        
        return Ok(reviews);
    }

    [HttpGet("{userId}/search_history", Name = nameof(GetUserHistory))]
    public IActionResult GetUserHistory(int userId)
    {
        var user = AuthorizeUser(userId);
        if (user == null) { return BadRequest(); }

        var searches = _ds.GetHistory(userId).Select(x => CreateSearchModel(x)).ToList(); // change UserSearch to Search object to adapt?

        return Ok(searches);
    }


    // update 
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

        JwtSecurityToken token = CreateToken(user, _configuration);

        // write token to string
        var jwt = new JwtSecurityTokenHandler().WriteToken(token); 

        return Ok(new { username = user.Username, token = jwt });
    }

    

    [HttpPut("{userId}/update_email")]
    [Authorize]
    public IActionResult UpdateEmail(int userId, [FromBody] UpdateEmailModel model)
    {
        var user = AuthorizeUser(userId);


        if (userId != user.Id)
        { 
            return Unauthorized();
        }
        var updated = _ds.UpdateEmail(user.Username, model.Email);

        if (updated)
        {
            return Ok(CreateUserModel(_ds.GetUser(userId)));
        }
        return BadRequest(updated);
        
        
        //if (email == null) return NotFound();
        
        // xxxxxx    _ds.GetUser(userId).Email;
        
        // "nulstil" email på en eller anden måde (null???)
        // returner den nye email med en tilknyttet funktionalitet (at den kan blive lavet på ny)
        // ny model?
        
    }

    [HttpPut("{userId}/update_password")]
    [Authorize]
    public IActionResult UpdatePassword(int userId, [FromBody] UpdatePasswordModel model)
    {
        var user = AuthorizeUser(userId);

        if (userId != user.Id)
        {
            return Unauthorized();
        }

        var updated = _ds.UpdatePassword(userId, model.Password);

        if (updated)
        {
            return Ok(CreateUserModel(_ds.GetUser(userId)));
        }
        return BadRequest(updated);
    }



    // deletes


    [HttpDelete("{userId}/review/{reviewId}")]
    [Authorize]
    public IActionResult DeleteReview(int userId, int reviewId)
    {
        JwtSecurityToken token = GetDecodedToken();
        User user = _ds.GetUser(token.Claims.FirstOrDefault().Value);

        if (user == null || userId != user.Id)
        {
            return BadRequest();
        }

        var review = _ds.GetReview(reviewId);

        if (review == null) return NotFound();

        var deleted = _ds.DeleteReview(reviewId);

        if (deleted) return Ok();

        return NotFound();
    }

    [HttpDelete("{userId}/bookmark/{bookmarkId}")]
    [Authorize]
    public IActionResult DeleteBookmark(int userId, string bookmarkId)
    {
        var user = AuthorizeUser(userId);

        var deleted = _ds.DeleteBookmark(bookmarkId);

        if (deleted) return Ok();

        return NotFound();
    }



    [HttpDelete("{userId}")]
    [Authorize]
    public IActionResult DeleteUser(int userId)
    {
        var user = AuthorizeUser(userId);

        if (user == null || userId != user.Id) return BadRequest();

        var deleted = _ds.DeleteUser(userId);

        if (deleted) return Ok();

        return NotFound();
    }


    // models
    private static ReviewModel CreateReviewModel(UserTitleReview review)
    {
        var model = review.Adapt<ReviewModel>();
        model.Username = review.User.Username;
        model.Text = review.Review.Text;
        return model;
    }

    private LikeModel CreateLikeModel(UserLikesReview likes)
    {

         var model = likes.Adapt<LikeModel>();
         model.Username = likes.User.Username;
         model.Title = likes.Review.createdBy.Title._Title;
         return model;
    }

    private SearchModel CreateSearchModel(UserSearch userSearch)
    {
        var model = userSearch.Adapt<SearchModel>();
        model.Keyword = userSearch.Search.Keyword;
        model.CreatedAt = userSearch.Search.CreatedAt;

        return model;
    }
    private static UserModel CreateUserModel(User user)
    {
        return user.Adapt<UserModel>();
    }

    private static SessionModel CreateSessionModel(UserSessionsHistory session)
    {
        return session.Adapt<SessionModel>();
    }


    private BookmarkModel CreateBookmarkModel(Bookmark bookmark)
    {
        var model = bookmark.Adapt<BookmarkModel>();

        var title = _titleDs.GetTitleFromId(bookmark.WebpageBookmark.Webpage.TitleId);


        model.Url = GetWebpageUrlByAction(nameof(TitleController.GetTitle), title.Id, "Title");
        model.Title = title._Title;
        model.Poster = title.Poster;
        
        return model;
    }
    
    private static TitleModel CreateTitleModel(Title title)
    {
        return title.Adapt<TitleModel>();
    }
}
