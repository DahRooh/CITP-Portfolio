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

    public bool CreateReview(CreateReviewModel model, int userId, string tId)
    {
        db = new MVContext();
        var title = db.Titles.Where(x => x.Id == tId).FirstOrDefault();
        var reviewId = 0;

        if (title == null) return false;
        
        var review = db.Reviews
            .Include(x => x.createdBy)
            .Where(x => x.createdBy.UserId == userId)
            .Where(x => x.createdBy.TitleId == tId)
            .FirstOrDefault();

        if (review == null)
        {
            reviewId = db.Reviews.Any() ? db.Reviews.Max(x => x.Id) + 1 : 1; // set to max id + 1, or set to 1 if no reviews
        } else
        {
            reviewId = review.Id;
        }

        db.Database.ExecuteSqlRaw("call rate({0}, {1}, {2}, {3}, {4}, {5})", title.Id, userId, model.Rating, reviewId, model.ReviewText, model.CaptionText);

        var newReview = db.Reviews
            .Where(x => x.Id == reviewId)
            .FirstOrDefault();


        if (newReview == null) return false;

        return true;
    }
    public bool LikeReview(int userId, int revId, int like)
    {
        db = new MVContext();
        db.Database.ExecuteSqlRaw("call like_review({0},{1},{2})", userId, revId, like);
        var liked = db.UserLikesReviews.Where(x => x.UserId == userId && x.ReviewId == revId).FirstOrDefault();

        if (liked == null) return false;
        return true;
    }

    public IList<Series> GetEpisodesFromParentId(string parentId)
    {
        db = new MVContext();

        var results = db.Series.FromSqlRaw("select * from get_series({0})", parentId).ToList();

        return results;
    }



    public IList<Title> GetAllSeries(int page, int pageSize)
    {
        db = new MVContext();

        var series = db.Titles
            .Where(t => t.Titletype == "series")
            .OrderByDescending(t => t.Rating)
            .Skip(page * pageSize).Take(pageSize)
            .ToList();

        return series;
    }

    public Bookmark CreateBookmark(string tId, int userId)
    {
        db = new MVContext();
        var wpId = "wp" + tId;
        db.Database.ExecuteSqlRaw("call insert_bookmark({0},{1})", userId, wpId);


        var bookmark = db.Bookmarks
            .Include(x => x.WebpageBookmark).ThenInclude(x => x.Webpage)
            .Include(x => x.BookmarkedBy).ThenInclude(x => x.User)  
            .Where(x => x.BookmarkedBy.UserId == userId)
            .Where(x => x.WebpageBookmark.Webpage.TitleId == tId)
            .FirstOrDefault();

        return bookmark;
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

    public int GetSeriesCount()
    {
        db = new MVContext();

        var numberOfSeries = db.Titles
            .Where(t => t.Titletype == "series")
            .Count();


        return numberOfSeries;
    }

    public IList<Title> GetMovies(int page, int pageSize)
    {
        db = new MVContext();
        var titles = db.Titles
            .Where(x => x.Titletype=="movie")
            .OrderByDescending(t => t.Rating)
            .Skip(page * pageSize).Take(pageSize).ToList();
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
    public Title GetTitleFromId(string id)
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

    public IList<InvolvedIn> GetInvolvedIn(string id, int page, int pageSize)
    {
        db = new MVContext();
        var personInvolvedIn = db.PersonInvolvedIn
            .Include(x => x.Title)
            .Include(x => x.Person)
            .AsSplitQuery()
            .Where(x => x.TitleId == id)
            .OrderByDescending(x => x.Person.PersonRating)
            .Skip((page-1)*pageSize).Take(pageSize)
            .ToList();
        
        var distinctPersonInvolvedIn = personInvolvedIn.DistinctBy(x => x.Id).ToList();

        return distinctPersonInvolvedIn;
    }

    public int NumberOfCast(string id)
    {
        db = new MVContext();
        var count = db.PersonInvolvedIn
        .Where(x => x.TitleId == id && x.Character != null)
        .Count();
        return count;
    }

    public int NumberOfCrew(string id)
    {
        db = new MVContext();
        var count = db.PersonInvolvedIn
        .Where(x => x.TitleId == id && x.Character == null)
        .Count();
        return count;
    }

    public IList<InvolvedIn> GetCast(string id, int page, int pageSize) 
    {
        db = new MVContext();
        var cast = db.PersonInvolvedIn
            .Include(x => x.Title)
            .Include(x => x.Person)
            .AsSplitQuery()
            .Where(x => x.TitleId == id && x.Character != null)
            .OrderByDescending(x => x.Person.PersonRating)
            .Skip((page - 1) * pageSize).Take(pageSize)
            .Select(x => new InvolvedIn {
                Id = x.Id, 
                TitleId = x.TitleId, 
                Character = x.Character, 
                Job = x.Job, 
                Person = x.Person, 
                Title = x.Title})
            .ToList();

        return cast;
    }
    
    public IList<TitleGenre> GetGenre(string id)
    {
        db = new MVContext();
        var genre = db.TitlesGenres
            .Include(x => x.Genre)
            .Include(x => x.Title)
            .AsSplitQuery()
            .Where(x => x.TitleId == id).ToList();
        return genre;
    }


    public IList<SimilarTitle> GetSimilarTitles(string id, int page, int pageSize)
    {
        db = new MVContext();   

        var results = db.SimilarTitles.FromSqlRaw("select * from find_similar_titles({0})", id).OrderByDescending(x => x.CommonGenres).Skip((page-1) * pageSize).Take(pageSize).ToList();

        return results;

    }
    public int NumberOfSimilarTitles(string id)
    {
        db = new MVContext();
        var count = db.SimilarTitles.FromSqlRaw("select * from find_similar_titles({0})", id).Count();

        return count;
    }


    public IList<UserTitleReview> GetReviews(string tId)
    {
        db = new MVContext();
        var reviews = db.UserReviews
            .Include(x => x.Title)
            .Include(x => x.Review)
            .Include(x => x.User)
            .AsSplitQuery()
            .Where(x => x.TitleId == tId && x.Review.Caption != null).ToList();
        
        return reviews;
    }


    public bool UpdateReview(string titleId, int userId, int userRating, string inReview, int reviewId, string inCaption)
    {
        db = new MVContext();
        var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
        var review = db.UserReviews.Where(x => x.ReviewId == reviewId && x.UserId == userId && x.TitleId == titleId).FirstOrDefault();

        
        if (user != null && review != null)
        {
            db.Database.ExecuteSqlRaw("call rate({0},{1},{2},{3},{4},{5})", titleId, userId, userRating, reviewId, inReview, inCaption);
            return true;
        }
        return false;
        
    }


    public bool DeleteLike(int revId, int id)
    {
        db = new MVContext();
        try
        {
            db.UserLikesReviews.Remove(db.UserLikesReviews.Single(x => x.ReviewId == revId));
        }
        catch
        {
            return false;
        }

        var saved = db.SaveChanges() > 0;

        return saved;
    }

}

