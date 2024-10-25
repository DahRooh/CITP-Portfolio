namespace DataLayer;

public class Person
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? BirthYear { get; set; }
    public int? DeathYear { get; set; }
    // public int Age { get; set; }
    public double? Rating { get; set; } = 0;
    public List<Profession> Profession { get; set; }

    public List<string> ProfessionNames
    {
        get
        {
            return Profession.Select(x => x.Name).ToList();
        }
    }
    public List<PersonInvolvedIn> InvolvedIn { get; set; }

    public List<(string, string)> TitlesInvolvedIn
    {
        get
        {
            return InvolvedIn.Where(x => x.PersonId == Id)
                .Select(x => (TitleId: x.TitleId, Character: x.Character)).ToList();
        }
    }
}




