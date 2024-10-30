using DataLayer.DomainObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects.Relations;

namespace DataLayer;

public class TitleDataService : ITitleDataService
{
    private MVContext db;

    public int NumberOfMovies()
    {
        db = new MVContext();
        var count = db.Movies.Count();
        return count;
    }

    public IList<Episode> GetEpisodes(int page, int pageSize)
    {
        db = new MVContext();
        var serie = db.Episodes
            .Include(x => x.Title)
            .Skip(page * pageSize).Take(pageSize)
            .ToList();

        if (serie == null || serie.Count() == 0)
        {
            return null;
        }

        return serie;
    }

    public Episode GetEpisode(string id)
    {
        db = new MVContext();
        var serie = db.Episodes
            .Include(x => x.Title)
            .FirstOrDefault(x => x.Id == id);

        if (serie == null)
        {
            return null;
        }

        return serie;
    }

    public Title GetTitle(string id)
    {
        db = new MVContext();
        var title = db.Titles
            .Where(x => x.Id == id)
            .FirstOrDefault();

        if (title == null)
        {
            return null;
        }

        return title;
    }

    public IList<PersonInvolvedIn> GetPersonInvolvedIn(string id)
    {
        db = new MVContext();
        var personInvolvedIn = db.PersonInvolvedIn
            .Include(x => x.Title)
            .Include(x => x.Person)
            .Where(x => x.TitleId == id).ToList();

        return personInvolvedIn;
    }

    public IList<PersonInvolvedIn> GetCast(string id)
    {
        db = new MVContext();
        var cast = db.PersonInvolvedIn
            .Include(x => x.Title)
            .Include(x => x.Person)
            .Where(x => x.TitleId == id)
            .Where(x => x.Character != null).ToList();
        //.Any(p => p.ProfessionName == "actor" || p.ProfessionName == "actress")).ToList();

        return cast;
    }

    public IList<TitleGenre> GetGenre(string id)
    {
        db = new MVContext();
        var genre = db.TitlesGenres.Include(x => x.Genre)
            .Where(x => x.TitleId == id).ToList();
        
        return genre;
    }

    public int NumberOfEpisodes()
    {
        db = new MVContext();
        var count = db.Episodes.Count();
        return count;
    }

    public IList<Movie> GetMovies(int page, int pageSize)
    {
        db = new MVContext();
        var titles = db.Movies.Include(x => x.Title).Skip(page * pageSize).Take(pageSize).ToList();
        return titles;
    }

    public Movie? GetMovie(string id)
    {
        db = new MVContext();
        var movie = db.Movies.Include(x => x.Title).FirstOrDefault(x => x.Id == id);

        return movie;
    }
}

