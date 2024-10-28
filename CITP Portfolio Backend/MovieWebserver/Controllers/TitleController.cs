using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/")]
public class TitleController : BaseController
{
    private readonly ITitleDataService _ds;
    private readonly LinkGenerator _linkGenerator;

    public TitleController(ITitleDataService ds, LinkGenerator linkGenerator) : base(linkGenerator)
    {
        _ds = ds;
        _linkGenerator = linkGenerator;
    }


    [HttpGet("movies", Name = nameof(GetMovies))]
    public IActionResult GetMovies(int page, int pageSize)
    {
        var movies = _ds.GetMovies(page, pageSize).Select(x => CreateMovieModel(x));
        var numberOfItems = _ds.NumberOfMovies();

        object result = CreatePaging(
            nameof(GetMovies),
            page,
            pageSize,
            numberOfItems,
            movies);
        return Ok(result);
    }

    [HttpGet("movie/{id}", Name = nameof(GetMovie))]
    public IActionResult GetMovie(string id)
    {
        var movie = _ds.GetMovie(id);
        if (movie == null)
        {
            return NotFound();
        }

        var model = CreateMovieModel(movie);

        return Ok(model);

    }

    [HttpGet("episodes", Name = nameof(GetEpisodes))]
    public IActionResult GetEpisodes(int page, int pageSize)
    {
        var episodes = _ds.GetEpisodes(page, pageSize).Select(x => CreateEpisodeModel(x));

        var numberOfItems = _ds.NumberOfSeries();

        object result = CreatePaging(
            nameof(GetEpisodes),
            page,
            pageSize,
            numberOfItems,
            episodes);
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


    private MovieModel? CreateMovieModel(Movie movie)
    {
        if(movie == null)
        {
            return null;
        }

        var newMovie = movie.Adapt<MovieModel>();
        newMovie.Url = GetWebpageUrl(nameof(GetMovie), new { movie.Id });

        newMovie.IsAdult = movie.Title.IsAdult;
        newMovie.Released = movie.Title.Released;
        newMovie.Language = movie.Title.Language;
        newMovie.Country = movie.Title.Country;
        newMovie.RunTime = movie.Title.RunTime;
        newMovie.Poster = movie.Title.Poster;

        return newMovie;
    }

    private EpisodeModel? CreateEpisodeModel(Episode episode)
    {
        if (episode == null)
        {
            return null;
        }

        var newSerie = episode.Adapt<EpisodeModel>();
        newSerie.Url = GetWebpageUrl(nameof(GetEpisode), new { episode.Id});

        newSerie.Name = episode.Title._Title;
        newSerie.Plot = episode.Title.Plot;
        newSerie.Rating = episode.Title.Rating;
        newSerie.IsAdult = episode.Title.IsAdult;
        newSerie.Released = episode.Title.Released;
        newSerie.Language = episode.Title.Language;
        newSerie.Country = episode.Title.Country;
        newSerie.RunTime = episode.Title.RunTime;
        newSerie.Poster = episode.Title.Poster;

        return newSerie;
    }




}
