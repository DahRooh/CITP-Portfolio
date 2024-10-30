using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects;
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
    IList<PersonInvolvedIn> GetPersonInvolvedIn(string id);
    IList<PersonInvolvedIn> GetCast(string id);

    IList<TitleGenre> GetGenre(string id);
}
