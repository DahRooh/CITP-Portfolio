using Microsoft.AspNetCore.Mvc;
using DataLayer;
using MovieWebserver.Model.Title;
using Mapster;
using DataLayer.DomainObjects;
using MovieWebserver.Model.Person;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.InteropServices;
namespace MovieWebserver.Controllers;

[ApiController]
[Route("api/title")]
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

        return Ok(movie);

    }

    [HttpGet("episodes", Name = nameof(GetEpisodes))]
    public IActionResult GetEpisodes(int page, int pageSize)
    {
        var episodes = _ds.GetEpisodes(page, pageSize).Select(x => CreateEpisodeModel(x));

        var numberOfItems = _ds.NumberOfEpisodes();

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

    [HttpGet("personInvolvedInTitle/{id}", Name = nameof(GetPeopleInvolvedIn))]
    public IActionResult GetPeopleInvolvedIn(string id)
    {
        var peopleInvolvedIn = _ds.GetPersonInvolvedIn(id).Select(x => CreatePersonInvolvedTitleModel(x));

        if (peopleInvolvedIn == null)
        {
            return NotFound();
        }
        return Ok(peopleInvolvedIn);
    }

    [HttpGet("cast/{id}", Name = nameof(GetCast))]
    public IActionResult GetCast(string id)
    {
        var cast = _ds.GetCast(id);
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
    
        private Model? CreateModel<Model, Entity>(Entity entity, string entityName, object args) where Model : class // where Model : class: Generic constraint, specifies that the type parameter Model must be a reference type (i.e., a class)
    {
        if (entity == null) return null;

        var model = entity.Adapt<Model>();
        var url = GetWebpageUrl(entityName, args);

        if (model is MovieModel movieModel && entity is Movie movie)
        {
            MovieModel(movieModel, movie, url);
        }
        else if (model is EpisodeModel episodeModel && entity is Episode episode)
        {
            EpisodeModel(episodeModel, episode, url);
        }
        else if (model is PersonInvolvedTitleModel personInvolvedTitleModel &&
                 entity is PersonInvolvedIn personInvolvedIn)
        {
            PersonInvolvedTitleModel(personInvolvedTitleModel, personInvolvedIn);
        }
        return model;
    }

    private void MovieModel(MovieModel movieModel, Movie movie, string url)
    {
        movieModel.Url = url;
        movieModel.IsAdult = movie.Title.IsAdult;
        movieModel.Released = movie.Title.Released;
        movieModel.Language = movie.Title.Language;
        movieModel.Country = movie.Title.Country;
        movieModel.RunTime = movie.Title.RunTime;
        movieModel.Poster = movie.Title.Poster;
    }

    private void EpisodeModel(EpisodeModel episodeModel, Episode episode, string url)
    {
        episodeModel.Url = url;
        episodeModel.Name = episode.Title._Title;
        episodeModel.Plot = episode.Title.Plot;
        episodeModel.Rating = episode.Title.Rating;
        episodeModel.IsAdult = episode.Title.IsAdult;
        episodeModel.Released = episode.Title.Released;
        episodeModel.Language = episode.Title.Language;
        episodeModel.Country = episode.Title.Country;
        episodeModel.RunTime = episode.Title.RunTime;
        episodeModel.Poster = episode.Title.Poster;
    }

    private void PersonInvolvedTitleModel(PersonInvolvedTitleModel personInvolvedTitleModel,
        PersonInvolvedIn personInvolvedIn)
    {
        personInvolvedTitleModel.TitleName = personInvolvedIn.Title._Title;
        personInvolvedTitleModel.Person = personInvolvedIn.Person.Name;
    }

    private MovieModel? CreateMovieModel(Movie movie)
    {
        return CreateModel<MovieModel, Movie>(movie, nameof(GetMovie), new { movie.Id });
    }

    private EpisodeModel? CreateEpisodeModel(Episode episode)
    {
        return CreateModel<EpisodeModel, Episode>(episode, nameof(GetEpisode), new { episode.Id });
    }

    private PersonInvolvedTitleModel? CreatePersonInvolvedTitleModel(PersonInvolvedIn personInvolvedIn)
    {
        return CreateModel<PersonInvolvedTitleModel, PersonInvolvedIn>(personInvolvedIn, nameof(GetPeopleInvolvedIn),
            new { personInvolvedIn.TitleId });
    }

}


