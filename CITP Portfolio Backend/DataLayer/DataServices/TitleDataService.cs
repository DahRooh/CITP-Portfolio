using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer;

public class TitleDataService : ITitleDataService
{
    private MVContext db;

    public IList<Title> GetTitles()
    {
        db = new MVContext();
        var titles = db.Titles
            .Include(x => x.Genres)
            .Include(x => x.PeopleInvolved)
            .Take(5).ToList();

        if (titles == null || titles.Count() == 0)
        {
            return null;
        }

        return titles;
    }
}
