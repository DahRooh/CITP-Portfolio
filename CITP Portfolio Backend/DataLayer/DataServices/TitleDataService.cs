using DataLayer.DomainObjects;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public int NumberOfSeries()
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
        
        if (movie == null)
        {
            return null;
        }

        return movie;
    }


}
