using System;
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
    UserTitleReview CreateReview(CreateReviewModel model, int userId, string tId);
    Bookmark CreateBookmark(string tId, int id);


    // get
    Title GetTitle(string id);

    IList<Episode> GetEpisodes(int page, int pageSize);
    Episode GetEpisode(string id);


    IList<Movie> GetMovies(int page, int pageSize);
    Movie? GetMovie(string id);
    IList<InvolvedIn> GetInvolvedIn(string id);
    IList<InvolvedIn> GetCast(string id);

    IList<TitleGenre> GetGenre(string id);
    IList<SimilarTitle> GetSimilarTitles(string id);
    IList<UserTitleReview> GetReviews(string tId);

    int NumberOfMovies();
    int NumberOfEpisodes();

    bool UpdateReview(string titleId, int userId, int userRating, string inReview, int reviewId);

}
