using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using DataLayer.Model.Title;
using DataLayer.DomainObjects.FunctionResults;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using DataLayer.IDataServices;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.User;
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


// MOST ARE GET METHODS

    [HttpGet("movie/{mId}", Name = nameof(GetMovie))]
    public IActionResult GetMovie(string mId)
    {
        var movie = _ds.GetMovie(mId);
        if (movie == null)
        {
            return NotFound();
        }

        var model = CreateMovieModel(movie);


        return Ok(model);

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

    [HttpGet("episode/{eId}", Name = nameof(GetEpisode))]
    public IActionResult GetEpisode(string eId)
    {
        var episode = _ds.GetEpisode(eId);
        if (episode == null)
        {
            return NotFound();
        }
        var model = CreateEpisodeModel(episode);

        return Ok(model);
    }

    [HttpGet("{tId}")]
    public IActionResult GetTitle(string tId)
    {
        var title = _ds.GetTitleFromId(tId);
        if (title == null)
        {
            return NotFound();
        }

        var model = CreateTitleModel(title); 
        return Ok(model);

    }



    [HttpGet("episodes", Name = nameof(GetEpisodes))]
    public IActionResult GetEpisodes(int page = 1, int pageSize = 20)
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



    [HttpGet("{tId}/crew", Name = nameof(GetInvolvedIn))]
    public IActionResult GetInvolvedIn(string tId)
    {
        var peopleInvolvedInTitle = _ds.GetInvolvedIn(tId).Select(x => CreateInvolvedTitleModel(x)).ToList();

        if (peopleInvolvedInTitle == null)
        {
            return NotFound();
        }
        return Ok(peopleInvolvedInTitle);
    }



    [HttpGet("{tId}/cast", Name = nameof(GetCastFromTitle))]
    public IActionResult GetCastFromTitle(string tId)
    {
        var cast = _ds.GetCast(tId).Select(x => CreateCastModel(x)).ToList();
        if (cast == null)
        {
            return NotFound();
        }
        return Ok(cast);
    }



    [HttpGet("{tId}/genre", Name = nameof(GetGenre))]
    public IActionResult GetGenre(string tId)
    {
        var genre = _ds.GetGenre(tId).Select(x => CreateTitleGenreModel(x)).ToList();
        if (genre.Count() == 0)
        {
            return NotFound();
        }
        return Ok(genre);
    }


    [HttpGet("{tId}/similartitles", Name = nameof(GetSimilarTitles))]
    public IActionResult GetSimilarTitles(string tId)
    {
        var similarTitles = _ds.GetSimilarTitles(tId).Select(x => CreateSimilarTitlesModel(x)).ToList();

        return Ok(similarTitles);
    }


    // USER BASED STUFF BELOW

    [HttpGet("{tId}/reviews")]
    public IActionResult GetReviews(string tId)
    {
        var reviews = _ds.GetReviews(tId);
        var models = reviews.Select(x => CreateReviewModel(x));

        return Ok(models);
    }

    [HttpPost("{tId}/review/{revId}")]
    [Authorize]
    public IActionResult LikeReview([FromBody] CreateLikeModel like, string tId, int revId)
    {
        var user = GetUserLoggedIn();

        if (user == null) return Unauthorized();

        if (_ds.GetTitleFromId(tId) == null) return NotFound();

        if (like.Like == 1 || like.Like == -1)
        {
            var liked = _ds.LikeReview(user.Id, revId, like.Like);
            if (liked)
            {
                return Ok();
            }
        }

        return BadRequest();
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

        if (_ds.GetTitleFromId(tId) == null) return NotFound();

        var review = _ds.CreateReview(model, user.Id, tId);
        var newReview = CreateReviewModel(review);

        if (newReview == null) return BadRequest();

        return Created(nameof(CreateReview), newReview);
    }

    [HttpPost("{tId}/bookmark", Name = nameof(CreateBookmark))]
    [Authorize]
    public IActionResult CreateBookmark(string tId)
    {
        JwtSecurityToken token = GetDecodedToken();
        User user = _userDs.GetUser(token.Claims.FirstOrDefault().Value);

        if (user == null)
        {
            return Unauthorized();
        }
        if (_ds.GetTitleFromId(tId) == null) return BadRequest();

        var bookmark = _ds.CreateBookmark(tId, user.Id); 

        if (bookmark != null)
        {
            return Created(nameof(CreateBookmark), CreateBookmarkModel(bookmark));
        }

        return BadRequest();

    }

    [HttpPut("{tId}/review/{revId}")]
    [Authorize]
    public IActionResult UpdateReview(string tId, int revId, [FromBody] UpdateReviewModel updateReview)
    {
        var token = GetDecodedToken();
        User user = _userDs.GetUser(token.Claims.FirstOrDefault().Value);

        if (user != null)
        {
            var update = _ds.UpdateReview(tId, user.Id, updateReview.Rating, updateReview.Text, revId);

            if (update)
            {
                return Ok(update);
            }
            else
            {
                return BadRequest();
            }
        }

        return Unauthorized();
    }


    [HttpDelete("{tId}/review/{revId}")]
    [Authorize]
    public IActionResult DeleteLike(int revId)
    {
        var user = GetUserLoggedIn();
        if (user == null) return BadRequest();


        var deleted = _ds.DeleteLike(revId, user.Id);

        if (deleted) return NoContent();
        return NotFound();
    }

    // CreateModel

    private MovieModel? CreateMovieModel(Movie movie)
    {
        var model = movie.Adapt<MovieModel>();
        var url = GetWebpageUrl(nameof(GetMovie), "Title", new { mId = movie.Id });
        model.Url = url;
        model._Title = movie.Title._Title;
        model.Plot = movie.Title.Plot;
        model.Rating = movie.Title.Rating;
        model.Type = movie.Title.Type;
        model.IsAdult = movie.Title.IsAdult;
        model.Released = movie.Title.Released;
        model.Language = movie.Title.Language;
        model.Country = movie.Title.Country;
        model.RunTime = movie.Title.RunTime;
        model.Poster = movie.Title.Poster;
        model.Poster = movie.Title.Poster;
        
        return model;
    }
    private BookmarkModel CreateBookmarkModel(Bookmark bookmark)
    {
        var title = _ds.GetTitleFromId(bookmark.WebpageBookmark.Webpage.TitleId);
        var model = bookmark.Adapt<BookmarkModel>();
        model.Url = GetWebpageUrl(nameof(GetTitle), "Title", new { tId = title.Id });
        model.Title = title._Title;
        model.Poster = title.Poster;
        return model;
    }
    private EpisodeModel? CreateEpisodeModel(Episode episode)
    {
        var model = episode.Adapt<EpisodeModel>();
        var url = GetWebpageUrl(nameof(GetEpisode), "Title", new { eId = episode.Id });
        model.Url = url;
        model._Title = episode.Title._Title;
        model.Plot = episode.Title.Plot;
        model.Rating = episode.Title.Rating;
        model.TitleType = episode.Title.Titletype;
        model.IsAdult = episode.Title.IsAdult;
        model.Released = episode.Title.Released;
        model.Language = episode.Title.Language;
        model.Country = episode.Title.Country;
        model.RunTime = episode.Title.RunTime;
        model.Poster = episode.Title.Poster;
        return model;
    }

    private TitleModel CreateTitleModel(Title title)
    {
        var model = title.Adapt<TitleModel>();
        model.Url = GetWebpageUrl(nameof(GetTitle), "Title", new { tId = title.Id });
        return model;
    }

    private InvolvedInModel? CreateInvolvedTitleModel(InvolvedIn involvedIn)
    {
        var model = involvedIn.Adapt<InvolvedInModel>();
        var url = GetWebpageUrl(nameof(PersonController.GetPerson), "Person", new { pId = involvedIn.PersonId });
        model.Url = url;
        model.Person = involvedIn.Person.Name;
        model.Job = involvedIn.Job;
        model.Rating = involvedIn.Person.Rating;
        return model;
    }

    private CastModel? CreateCastModel(InvolvedIn involvedIn)
    {
        var model = involvedIn.Adapt<CastModel>();
        var url = GetWebpageUrl(nameof(PersonController.GetPerson), "Person", new { pId = involvedIn.PersonId });
        model.Url = url;
        model.Person = involvedIn.Person.Name;
        model.Character = involvedIn.Character;
        return model;
    }

    private SimilarTitlesModel? CreateSimilarTitlesModel(SimilarTitle similarTitle)
    {
        var model = similarTitle.Adapt<SimilarTitlesModel>();
        var url = GetWebpageUrl(nameof(GetTitle), "Title", new { tId = similarTitle.SimilarTitleId }); // TO DO: Wrong url! query url
        model.Url = url;

        model.TitleId = similarTitle.SimilarTitleId;
        model._Title = similarTitle.SimilarTitleName;
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
    private static GenreModel CreateTitleGenreModel(TitleGenre titleGenres)
    {
        return new GenreModel
        {
            Genre = titleGenres.Genre._Genre
        };
    }


}

