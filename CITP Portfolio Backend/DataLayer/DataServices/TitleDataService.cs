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
using DataLayer.DomainObjects.FunctionResults;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;
using DataLayer.Model.Title;
using MovieWebserver.Model.Title;
using Mapster;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace DataLayer;

public class TitleDataService : ITitleDataService
{
    private MVContext db;

    public UserTitleReview CreateReview(CreateReviewModel model, int userId)
    {
        db = new MVContext();

        var title = db.Titles.Where(x => x.Id == model.TitleId).FirstOrDefault();

        if (title == null) return null;

        db.Database.ExecuteSqlRaw("call rate({0},{1},{2},{3})", title.Id, userId, model.Rating, model.ReviewText);

        var newReview = db.UserReviews
            .Include(x => x.Review)
            .Include(x => x.User)
            .Include(x => x.Title)
            .Where(x => x.UserId == userId)
            .Where(x => x.TitleId == title.Id)
            .FirstOrDefault();

        if (newReview == null) return null;

        return newReview;
    }

    public bool CreateBookmark(string tId, int id)
    {
        db = new MVContext();
        tId = "wp" + tId;
        var created = db.Database.ExecuteSqlRaw("call insert_bookmark({0}, {1})", id, tId) > 0;

        if (created)
        {
            return true;
        }

        return false;
    }


    public Movie? GetMovie(string id)
    {
        db = new MVContext();
        var movie = db.Movies.Include(x => x.Title).FirstOrDefault(x => x.Id == id);

        return movie;
    }

    public int NumberOfMovies()
    {
        db = new MVContext();
        var count = db.Movies.Count();
        return count;
    }

    public IList<Movie> GetMovies(int page, int pageSize)
    {
        db = new MVContext();
        var titles = db.Movies.Include(x => x.Title).Skip(page * pageSize).Take(pageSize).ToList();
        return titles;
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

    public int NumberOfEpisodes()
    {
        db = new MVContext();
        var count = db.Episodes.Count();
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

    public IList<InvolvedIn> GetInvolvedIn(string id)
    {
        db = new MVContext();
        var personInvolvedIn = db.PersonInvolvedIn
            .Include(x => x.Title)
            .Include(x => x.Person)
            .Where(x => x.TitleId == id)
            .OrderBy(x => x.Person.Rating)
            .ToList();

        return personInvolvedIn;
    }

    public IList<InvolvedIn> GetCast(string id)
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


    public IList<SimilarTitle> GetSimilarTitles(string id)
    {
        db = new MVContext();   

        var results = db.SimilarTitles.FromSqlRaw("select * from find_similar_titles({0})", id).ToList();

        return results;

    }


    public IList<UserTitleReview> GetReviews(string tId)
    {
        db = new MVContext();
        var reviews = db.UserReviews
            .Include(x => x.Title)
            .Include(x => x.Review)
            .Include(x => x.User)
            .Where(x => x.TitleId == tId).ToList();
        
        return reviews;
    }


    public bool UpdateReview(string titleId, int userId, int userRating, string inReview)
    {
        db = new MVContext();
        var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
        
        if (user != null)
        {
            db.Database.ExecuteSqlRaw("call rate({0},{1},{2},{3})", titleId, userId, userRating, inReview);
            return true;
        }
        return false;
        
    }
    





}

