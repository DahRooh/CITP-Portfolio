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
    public IActionResult GetMovies(int page, int pageSize)
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

    [HttpGet("similartitles/{id}", Name = nameof(GetSimilarTitles))]
    public IActionResult GetSimilarTitles([FromQuery] string id, [FromQuery] int page, [FromQuery] int pageSize)
    {
        var similarTitles = _ds.GetSimilarTitles(id, page, pageSize).Select(x => CreateSimilarTitlesModel(x)).ToList();

        var numberOfSimilarTitles = _ds.NumberOfSimilarTitles(id);

        var results = CreatePaging(nameof(GetSimilarTitles), "Title", page, pageSize, numberOfSimilarTitles, similarTitles, id);
        return Ok(results);
    }

    [HttpGet("findcoactors/{id}", Name = nameof(GetCoProducersByRating))]
    public IActionResult GetCoProducersByRating(string id, int page, int pageSize)
    {
        var coProducers = _ds.GetCoProducersByRating(id, page, pageSize).Select(x => CreateCoProducersModel(x)).ToList();
        var numberOfItems = _ds.NumberOfCoProducers();

        object result = CreatePaging(
            nameof(GetCoProducersByRating),
            "Title",
            page,
            pageSize,
            numberOfItems,
            coProducers);
        return Ok(result);

    }


    private Model? CreateModel<Model, Entity>(Entity entity, string entityName, object args)
    {

        var model = entity.Adapt<Model>();
        var url = GetWebpageUrl(entityName,"Title", args);

    if (model is MovieModel movieModel && entity is Movie movie)
    {
    MovieModel(movieModel, movie, url);
    }
    else if (model is EpisodeModel episodeModel && entity is Episode episode)
    {
        EpisodeModel(episodeModel, episode, url);
    }
    else if (model is InvolvedInModel involvedInModel &&
                entity is InvolvedIn involvedIn)
    {
        InvolvedInModel(involvedInModel, involvedIn, url);
    }
    else if (model is CastModel castModel && entity is InvolvedIn cast)
    {
        CastModel(castModel, cast, url);
    }
    else if (model is SimilarTitlesModel similarTitlesModel && entity is SimilarTitle similarTitle)
    {
        SimilarTitlesModel(similarTitlesModel, similarTitle, url);
    }
    else if (model is CoProducersModel coProducersModel && entity is Person person)
    {
        CoProducersModel(coProducersModel, person, url);
    }
        return model;
    }

    private void MovieModel(MovieModel movieModel, Movie movie, string? url)
    {
        movieModel.Url = url;
        movieModel.IsAdult = movie.Title.IsAdult;
        movieModel.Released = movie.Title.Released;
        movieModel.Language = movie.Title.Language;
        movieModel.Country = movie.Title.Country;
        movieModel.RunTime = movie.Title.RunTime;
        movieModel.Poster = movie.Title.Poster;
    }

    private void EpisodeModel(EpisodeModel episodeModel, Episode episode, string? url)
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

    private void CastModel(CastModel castModel, InvolvedIn involvedIn, string? url)
    {
        castModel.Url = url;
        castModel.Person = involvedIn.Person.Name;
        castModel.Character = involvedIn.Character;
    }

    private void InvolvedInModel(InvolvedInModel involvedInModel, InvolvedIn involvedIn, string? url)
    {
        involvedInModel.Url = url;
        involvedInModel.TitleName = involvedIn.Title._Title;
        involvedInModel.Person = involvedIn.Person.Name;
    }

    private void SimilarTitlesModel(SimilarTitlesModel similarTitlesModel, SimilarTitle similarTitle, string? url)
    {
        similarTitlesModel.Url = url;
        similarTitlesModel.SimilarTitleId = similarTitle.SimilarTitleId;
        similarTitlesModel.SimilarTitle = similarTitle.SimilarTitleName;
        similarTitlesModel.AmountOfSimilarGenres = similarTitle.MultipleSameGenre;
    }
    private void CoProducersModel(CoProducersModel coProducersModel, Person person, string? url)
    {
        coProducersModel.Url = url;
        coProducersModel.Title = person.InvolvedIn.Select(x => x.Title._Title).FirstOrDefault(); 
        coProducersModel.Person =  person.Name;
        coProducersModel.Job = person.InvolvedIn.Select(x => x.Job).FirstOrDefault();
        coProducersModel.Rating = person.Rating;
    }


    private MovieModel? CreateMovieModel(Movie movie)
    {
        return CreateModel<MovieModel, Movie>(movie, nameof(GetMovie), new { movie.Id });
    }

    private EpisodeModel? CreateEpisodeModel(Episode episode)
    {
        return CreateModel<EpisodeModel, Episode>(episode, nameof(GetEpisode), new { episode.Id });
    }

    private InvolvedInModel? CreateInvolvedTitleModel(InvolvedIn involvedIn)
    {
        return CreateModel<InvolvedInModel, InvolvedIn>(involvedIn, nameof(GetInvolvedIn), new { involvedIn.TitleId });
    }
    
    private CastModel? CreateCastModel(InvolvedIn involvedIn)
    {
        return CreateModel<CastModel, InvolvedIn>(involvedIn, nameof(GetCastFromTitle), new { involvedIn.TitleId });
    }

    private SimilarTitlesModel? CreateSimilarTitlesModel(SimilarTitle similarTitle)
    {
        return CreateModel<SimilarTitlesModel, SimilarTitle>(similarTitle, nameof(GetSimilarTitles), new { similarTitle.SimilarTitleId });
    }
    private CoProducersModel? CreateCoProducersModel(Person person)
    {
        return CreateModel<CoProducersModel, Person>(person, nameof(GetCoProducersByRating), new { person.Id });
    }


}

