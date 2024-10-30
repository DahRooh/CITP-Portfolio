using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects;
using DataLayer.DomainObjects.FunctionResults;
using DataLayer.DomainObjects.Relations;


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
    int NumberOfSimilarTitles(string id);
    IList<SimilarTitle> GetSimilarTitles(string id, int page, int pageSize);
    int NumberOfCoProducers();
    IList<Person> GetCoProducersByRating(string id, int page, int pageSize);


}
