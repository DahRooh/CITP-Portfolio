using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects;


namespace DataLayer;

public interface ITitleDataService
{
    IList<Episode> GetEpisodes(int page, int pageSize);
    Episode GetEpisode(string id);
    IList<Movie> GetMovies(int page, int pageSize);
    int NumberOfSeries();
    Movie? GetMovie(string id);

    int NumberOfMovies();



}
