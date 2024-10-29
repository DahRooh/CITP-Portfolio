using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.DomainObjects;


namespace DataLayer;

public interface ITitleDataService
{
    int NumberOfEpisodes();
    IList<Episode> GetEpisodes(int page, int pageSize);
    Episode GetEpisode(string id);
    int NumberOfMovies();
    IList<Movie> GetMovies(int page, int pageSize);
    Movie? GetMovie(string id);



}
