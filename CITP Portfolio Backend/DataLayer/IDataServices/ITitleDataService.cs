﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;
using DataLayer.DomainObjects.Relations;
using DataLayer.Model.Title;
using MovieWebserver.Model.Title;


namespace DataLayer;

public interface ITitleDataService
{
    // create
    bool CreateReview(CreateReviewModel model, int userId, string tId);
    Bookmark CreateBookmark(string tId, int id);


    // get
    Title GetTitleFromId(string id);

    IList<Episode> GetEpisodes(int page, int pageSize);
    Episode GetEpisode(string id);

    public IList<Title> GetAllSeries(int page, int pageSize);

    public IList<Series> GetEpisodesFromParentId(string parentId);
    IList<Title> GetMovies(int page, int pageSize);
    Movie? GetMovie(string id);
    IList<InvolvedIn> GetInvolvedIn(string id, int page, int pageSize);
    IList<InvolvedIn> GetCast(string id, int page, int pageSize);

    IList<TitleGenre> GetGenre(string id);
    IList<SimilarTitle> GetSimilarTitles(string id, int page, int pageSize);
    IList<UserTitleReview> GetReviews(string tId);

    int NumberOfCast(string id);
    int NumberOfCrew(string id);
    int NumberOfMovies();
    int NumberOfEpisodes();
    public int NumberOfSimilarTitles(string id);

    int GetSeriesCount();
    bool UpdateReview(string titleId, int userId, int userRating, string inReview, int reviewId, string inCaption);
    public bool LikeReview(int userId, int revId, int like);
    bool DeleteLike(int revId, int id);
}
