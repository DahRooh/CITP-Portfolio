using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
using DataLayer.Model.Title;
using DataLayer.DomainObjects.FunctionResults;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using Microsoft.AspNetCore.Authorization;
using System;
using System.IdentityModel.Tokens.Jwt;
using DataLayer.IDataServices;
using System.Runtime.Intrinsics.X86;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/title")]
public class TitleController : BaseController
{
    private readonly ITitleDataService _ds;
    private readonly IUserDataService _userDs;
    private readonly LinkGenerator _linkGenerator;

    public TitleController(ITitleDataService ds, IUserDataService userDs, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
        _userDs = userDs;
        _linkGenerator = linkGenerator;
    }

    [HttpPost("{tId}/review")]
    [Authorize]
    public IActionResult CreateReview([FromBody] CreateReviewModel model, string tId)
    {
        //var username = HttpContext.Request.Headers.Authorization.FirstOrDefault();
        // rating, uid, tid, text, 
        JwtSecurityToken token = GetDecodedToken();
        User user = _userDs.GetUser(token.Claims.FirstOrDefault().Value);

        if (user == null) return Unauthorized();

        if (_ds.GetTitle(tId) == null) return NotFound();

        var review = _ds.CreateReview(model, user.Id, tId);
        var newReview = CreateReviewModel(review);

        if (newReview == null) return BadRequest();
        
        return Created(nameof(CreateReview), newReview);
    }

    [HttpPost("{tId}/bookmark")]
    [Authorize]
    public IActionResult CreateBookmark(string tId)
    {
        JwtSecurityToken token = GetDecodedToken();
        User user = _userDs.GetUser(token.Claims.FirstOrDefault().Value);

        if (user == null)
        {
            return BadRequest();
        }

        var bookmark = _ds.CreateBookmark(tId, user.Id);

        if (bookmark != null)
        {
            return Ok(bookmark);
        }

        return BadRequest();

    }


    [HttpGet("movie/{id}", Name = nameof(GetMovie))]
    public IActionResult GetMovie(string id)
    {
        var movie = _ds.GetMovie(id);
        if (movie == null)
        {
            return NotFound();
        }

        return Ok(movie);

    }

    [HttpGet("movies", Name = nameof(GetMovies))]
    public IActionResult GetMovies(int page = 1, int pageSize = 20)
    {

        var movies = _ds.GetMovies(page, pageSize).Select(x => CreateMovieModel(x)).ToList();
        var numberOfItems = _ds.NumberOfMovies();

        object result = CreatePaging(
            nameof(GetMovies),
            "Title",
            page,
            pageSize,
            numberOfItems,
            movies);
        return Ok(result);
    }

    [HttpGet("episode/{id}", Name = nameof(GetEpisode))]
    public IActionResult GetEpisode(string id)
    {
        var episode = _ds.GetEpisode(id);

        if (episode == null)
        {
            return NotFound();
        }

        return Ok(episode);
    }

    [HttpGet("{id}", Name = nameof(GetTitle))]
    public IActionResult GetTitle(string id)
    {
        var title = _ds.GetTitle(id);
        if (title == null)
        {
            return NotFound();
        }

        return Ok(title);

    }

    [HttpGet("episodes", Name = nameof(GetEpisodes))]
    public IActionResult GetEpisodes(int page, int pageSize)
    {
        var episodes = _ds.GetEpisodes(page, pageSize).Select(x => CreateEpisodeModel(x)).ToList();

        var numberOfItems = _ds.NumberOfEpisodes();

        object result = CreatePaging(
            nameof(GetEpisodes),
            "Title",
            page,
            pageSize,
            numberOfItems,
            episodes);
        return Ok(result);

    }

    
    
    [HttpGet("personsInvolvedInTitle/{id}", Name = nameof(GetInvolvedIn))]
    public IActionResult GetInvolvedIn(string id)
    {
        var peopleInvolvedInTitle = _ds.GetInvolvedIn(id).Select(x => CreateInvolvedTitleModel(x)).ToList();

        if (peopleInvolvedInTitle == null)
        {
            return NotFound();
        }
        return Ok(peopleInvolvedInTitle);
    }



    [HttpGet("cast/{id}", Name = nameof(GetCastFromTitle))]
    public IActionResult GetCastFromTitle(string id)
    {
        var cast = _ds.GetCast(id).Select(x => CreateCastModel(x)).ToList();
        if (cast == null)
        {
            return NotFound();
        }
        return Ok(cast);
    }



    [HttpGet("genre/{id}", Name = nameof(GetGenre))]
    public IActionResult GetGenre(string id)
    {
        var genre = _ds.GetGenre(id);
        if (genre == null)
        {
            return NotFound();
        }
        return Ok(genre);
    }

    [HttpGet("similartitles", Name = nameof(GetSimilarTitles))]
    public IActionResult GetSimilarTitles([FromQuery] string id)
    {
        var similarTitles = _ds.GetSimilarTitles(id).Select(x => CreateSimilarTitlesModel(x)).ToList();
 
        return Ok(similarTitles);
    }


    [HttpGet("{tId}/reviews")]
    public IActionResult GetReviews(string tId)
    {
        var reviews = _ds.GetReviews(tId);
        if (reviews == null)
        {
            return NotFound();
        }
        var models = reviews.Select(x => CreateReviewModel(x));
        
        return Ok(models);
    }

    [HttpPut("{tId}/review/{revId}")]
    [Authorize]
    public IActionResult UpdateReview(string tId, [FromQuery] int userRating, [FromQuery] string inReview)
    {
        var token = GetDecodedToken();
        User user = _userDs.GetUser(token.Claims.FirstOrDefault().Value);

        if (user != null)
        {
            var update = _ds.UpdateReview(tId, user.Id, userRating, inReview);

            if (update)
            {
                return Ok(update);
            }
            else
            {
                return NotFound();
            }

        }

        return Unauthorized();

    }





    // CreateModel

    private MovieModel? CreateMovieModel(Movie movie)
    {
        var model = movie.Adapt<MovieModel>();
        var url = GetWebpageUrl(nameof(GetMovies), "Title", new { movie.Id });
        model.Url = url;
        model.IsAdult = movie.Title.IsAdult;
        model.Released = movie.Title.Released;
        model.Language = movie.Title.Language;
        model.Country = movie.Title.Country;
        model.RunTime = movie.Title.RunTime;
        model.Poster = movie.Title.Poster;
        return model;
    }

    private EpisodeModel? CreateEpisodeModel(Episode episode)
    {
        var model = episode.Adapt<EpisodeModel>();
        var url = GetWebpageUrl(nameof(GetEpisodes), "Title", new { episode.Id });
        model.Url = url;
        model.Name = episode.Title._Title;
        model.Plot = episode.Title.Plot;
        model.Rating = episode.Title.Rating;
        model.IsAdult = episode.Title.IsAdult;
        model.Released = episode.Title.Released;
        model.Language = episode.Title.Language;
        model.Country = episode.Title.Country;
        model.RunTime = episode.Title.RunTime;
        model.Poster = episode.Title.Poster;
        return model;
    }

    private InvolvedInModel? CreateInvolvedTitleModel(InvolvedIn involvedIn)
    {
        var model = involvedIn.Adapt<InvolvedInModel>();
        var url = GetWebpageUrl(nameof(GetInvolvedIn), "Title", new { involvedIn.TitleId });
        model.Url = url;
        model.TitleName = involvedIn.Title._Title;
        model.Person = involvedIn.Person.Name;
        model.Job = involvedIn.Job;
        model.Rating = involvedIn.Person.Rating;
        return model;
    }

    private CastModel? CreateCastModel(InvolvedIn involvedIn)
    {
        var model = involvedIn.Adapt<CastModel>();
        var url = GetWebpageUrl(nameof(GetCastFromTitle), "Title", new { involvedIn.TitleId });
        model.Url = url;
        model.TitleName = involvedIn.Title._Title;
        model.Person = involvedIn.Person.Name;
        model.Character = involvedIn.Character;
        return model;
    }

    private SimilarTitlesModel? CreateSimilarTitlesModel(SimilarTitle similarTitle)
    {
        var model = similarTitle.Adapt<SimilarTitlesModel>();
        var url = GetWebpageUrl(nameof(GetSimilarTitles), "Title", new { similarTitle.SimilarTitleId });
        model.Url = url;
        model.SimilarTitleId = similarTitle.SimilarTitleId;
        model.SimilarTitle = similarTitle.SimilarTitleName;
        model.AmountOfSimilarGenres = similarTitle.MultipleSameGenre;
        return model;

    }

    private static ReviewModel CreateReviewModel(UserTitleReview review)
    {
        var model = review.Adapt<ReviewModel>();
        model.Text = review.Review.Text;
        model.Username = review.User.Username;
        return model;
    }



}

