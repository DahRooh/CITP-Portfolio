using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer;

public class MVContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> People { get; set; }
    // public DbSet<User> Users { get; set; }
    // public DbSet<Bookmark> Bookmarks { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=cit.ruc.dk;db=cit16;uid=cit16;pwd=eEWPIoEJet9J");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapGenre(modelBuilder);
        MapUsers(modelBuilder);
     //   MapEpisode(modelBuilder);
        MapTitles(modelBuilder);
        MapPerson(modelBuilder);
        MapProfession(modelBuilder);
        MapPersonInvolvedTitle(modelBuilder);
    }

    private static void MapUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users").HasKey(x => x.Id);

        modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("u_id");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
    }

    private static void MapEpisode(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Episode>().ToTable("episodes")
                                      .HasKey(x => x.Id);
        
        modelBuilder.Entity<Episode>().Property(x => x.SeasonNumber).HasColumnName("season_num");
        modelBuilder.Entity<Episode>().Property(x => x.EpisodeNumber).HasColumnName("ep_num");
    }
    
    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title")
                                    .HasKey(x => x.Id);

        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("t_id");
        modelBuilder.Entity<Title>().Property(x => x._Title).HasColumnName("title");
        modelBuilder.Entity<Title>().Property(x => x.Plot).HasColumnName("plot");
        modelBuilder.Entity<Title>().Property(x => x.Rating).HasColumnName("rating");
        modelBuilder.Entity<Title>().Property(x => x.Type).HasColumnName("type");
        modelBuilder.Entity<Title>().Property(x => x.IsAdult).HasColumnName("isadult");
        modelBuilder.Entity<Title>().Property(x => x.Released).HasColumnName("released");
        modelBuilder.Entity<Title>().Property(x => x.Language).HasColumnName("language");
        modelBuilder.Entity<Title>().Property(x => x.Country).HasColumnName("country");
        modelBuilder.Entity<Title>().Property(x => x.RunTime).HasColumnName("runtime");
        modelBuilder.Entity<Title>().Property(x => x.Poster).HasColumnName("poster");

/*
        modelBuilder.Entity<Title>().HasDiscriminator<string>("type")
            .HasValue<Movie>("tvShort")
            .HasValue<Movie>("movie")
            .HasValue<Movie>("tvMovie")
            .HasValue<Movie>("short")
            .HasValue<Episode>("tvMiniSeries")
            .HasValue<Episode>("tvEpisode")
            .HasValue<Episode>("tvSpecial")
            .HasValue<Episode>("tvSeries");
*/

        modelBuilder.Entity<Title>()
                    .HasMany(p => p.PeopleInvolved)
                    .WithOne(pi => pi.Title)
                    .HasForeignKey(pi => pi.TitleId);



        modelBuilder.Entity<Title>()
            .HasMany(t => t.Genres)
            .WithMany(g => g.Titles)
            .UsingEntity<Dictionary<string, string>>(
                "title_is",

                g => g.HasOne<Genre>()
                      .WithMany()
                      .HasForeignKey("genre")
                      .HasPrincipalKey(g => g._Genre),

                t => t.HasOne<Title>()
                      .WithMany()
                      .HasForeignKey("t_id")
                      .HasPrincipalKey(t => t.Id),

                j =>
                {
                    j.HasKey("t_id", "genre");
                });
    }

    private static void MapGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().ToTable("genre")
                                    .HasKey(x => x._Genre);

        modelBuilder.Entity<Genre>().Property(x => x._Genre).HasColumnName("genre");


    }

    private static void MapProfession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profession>().ToTable("profession")
                             .HasKey(x => x.Name);

        modelBuilder.Entity<Profession>().Property(x => x.Name).HasColumnName("profession");
    }

    private static void MapPersonInvolvedTitle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonInvolvedIn>().ToTable("person_involved_title")
                     .HasKey(x => new { x.TitleId, x.PersonId });

        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.PersonId).HasColumnName("p_id");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.Job).HasColumnName("job");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.Character).HasColumnName("character");
    }

    private static void MapPerson(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("person")
                                     .HasKey(x => x.Id);
        
        modelBuilder.Entity<Person>().Property(x => x.Id).HasColumnName("p_id");
        modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("death_year");
        modelBuilder.Entity<Person>().Property(x => x.Rating).HasColumnName("person_rating");


        modelBuilder.Entity<Person>()
            .HasMany(p => p.Profession)
            .WithMany(pr => pr.Persons)
            .UsingEntity<Dictionary<string, string>>(
                "person_has_a", 
                pr => pr.HasOne<Profession>()
                      .WithMany()
                      .HasForeignKey("profession") 
                      .HasPrincipalKey(pr => pr.Name), 

                p => p.HasOne<Person>()
                      .WithMany()
                      .HasForeignKey("p_id") 
                      .HasPrincipalKey(p => p.Id), 

                j =>
                {
                    j.HasKey("p_id", "profession"); 
                });



        modelBuilder.Entity<Person>()
            .HasMany(p => p.InvolvedIn)
            .WithOne(pi => pi.Person)
            .HasForeignKey(pi => pi.PersonId);

    }
}
