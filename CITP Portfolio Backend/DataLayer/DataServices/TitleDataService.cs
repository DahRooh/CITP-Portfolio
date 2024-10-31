﻿using DataLayer.DomainObjects;
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

namespace DataLayer;

public class TitleDataService : ITitleDataService
{
    private MVContext db;

    public ReviewModel CreateReview(CreateReviewModel model)
    {
        db = new MVContext();

        var title = db.Titles.Where(x => x.Id == model.TitleId).FirstOrDefault();
        var user = db.Users.Where(x => x.Username == model.Username).FirstOrDefault();

        if (title == null || user == null) return null;
        

        db.Database.ExecuteSqlRaw("call rate({0},{1},{2},{3})", title.Id, user.Id, model.Rating, model.ReviewText);

        var newReview = db.UserReviews.Where(x => x.UserId == user.Id).Where(x => x.TitleId == title.Id).FirstOrDefault();

        if (newReview == null) return null;

        return CreateReviewModel(newReview);
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
            .Where(x => x.TitleId == id).ToList();

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


    public int NumberOfSimilarTitles(string id, int pageSize, int skip)
    {
        return db.SimilarTitles.FromSqlRaw("select * from find_similar_titles({0}, {1}, {2})", id, pageSize, skip).Count();
    }

    public IList<SimilarTitle> GetSimilarTitles(string id, int page, int pageSize)
    {
        db = new MVContext();   

        var skip = page * pageSize;
        var results = db.SimilarTitles.FromSqlRaw("select * from find_similar_titles({0}, {1}, {2})", id, pageSize, skip).ToList();

        return results;

    }



    public IList<Person> GetCoproducersByRating(string id)
    {
        db = new MVContext();
        var findAllTheTitleIdsPersonInvolvedIn = db.PersonInvolvedIn
            .Where(x => x.PersonId == id)
            .Select(x => x.TitleId)
            .ToList();

        var findAllPersonInvoledInSameTitles = db.PersonInvolvedIn
            .Where(x => findAllTheTitleIdsPersonInvolvedIn.Contains(x.TitleId) && x.PersonId != id)
            .Select(x => x.Person)
            .Distinct()
            .OrderBy(p => p.Rating)
            .ToList();


        if (findAllPersonInvoledInSameTitles == null)
        {
            return null;
        }

        return findAllPersonInvoledInSameTitles;
    }


    public static ReviewModel CreateReviewModel(UserTitleReview review)
    {
        var model = review.Adapt<ReviewModel>();
        return model;
    }










}

