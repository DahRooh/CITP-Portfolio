using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public interface ITitleDataService
{
    //string GetTitle(string title);
    IList<Title> GetTitles();
    IList<Movie> GetMovies();
}
