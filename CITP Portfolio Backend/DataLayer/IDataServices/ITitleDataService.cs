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
    int NumberOfEpisodes();
    IList<Episode> GetEpisodes(int page, int pageSize);
    Episode GetEpisode(string id);
    Title GetTitle(string id);
    int NumberOfMovies();
    IList<Movie> GetMovies(int page, int pageSize);
    Movie? GetMovie(string id);
    IList<InvolvedIn> GetInvolvedIn(string id);
    IList<InvolvedIn> GetCast(string id);

    IList<TitleGenre> GetGenre(string id);
    IList<SimilarTitle> GetSimilarTitles(string id);
    UserTitleReview CreateReview(CreateReviewModel model, int userId);
    IList<UserTitleReview> GetReviews(string tId);
    bool CreateBookmark(string tId, int id);

    bool UpdateReview(string titleId, int userId, int userRating, string inReview);

}
