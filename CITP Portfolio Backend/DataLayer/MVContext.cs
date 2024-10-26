using DataLayer.DomainObjects;
using DataLayer.DomainObjects.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

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
        MapTitles(modelBuilder);
        MapPerson(modelBuilder);
        MapProfession(modelBuilder);

        MapPersonProfession(modelBuilder);
        MapTitleGenre(modelBuilder);
        MapReview(modelBuilder);

     //   MapEpisode(modelBuilder);
        MapPersonInvolvedTitle(modelBuilder);
        MapUsers(modelBuilder);
        MapUserReview(modelBuilder);

    }



    private static void MapUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users")
                                    .HasKey(x => x.Id);

        modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("u_id");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");

        /*modelBuilder.Entity<User>()
            .HasMany(u => u.reviews)
            .WithOne(r => r.)
            .HasForeignKey(r => r.)*/

    }

    private static void MapReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>().ToTable("review")
        .HasKey(x => x.Id);

        modelBuilder.Entity<Review>().Property(x => x.Id).HasColumnName("rev_id");
        modelBuilder.Entity<Review>().Property(x => x.Likes).HasColumnName("likes");
        modelBuilder.Entity<Review>().Property(x => x.Text).HasColumnName("review");
    }

    private static void MapEpisode(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Episode>().ToTable("episodes")
                                      .HasKey(x => x.TitleId);
        
        modelBuilder.Entity<Episode>().Property(x => x.SeasonNumber).HasColumnName("season_num");
        modelBuilder.Entity<Episode>().Property(x => x.EpisodeNumber).HasColumnName("ep_num");
    }

    private static void MapGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().ToTable("genre")
                                    .HasKey(x => x._Genre);

        modelBuilder.Entity<Genre>().Property(x => x._Genre).HasColumnName("genre");



    }

    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title")
                                    .HasKey(x => x.TitleId);

        modelBuilder.Entity<Title>().Property(x => x.TitleId).HasColumnName("t_id");
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




    }
    private static void MapPerson(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("person")
                                     .HasKey(x => x.PersonId);

        modelBuilder.Entity<Person>().Property(x => x.PersonId).HasColumnName("p_id");
        modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("death_year");
        modelBuilder.Entity<Person>().Property(x => x.Rating).HasColumnName("person_rating");


    }

    private static void MapProfession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profession>()
                             .ToTable("profession")
                             .HasKey(x => x.Name);

        modelBuilder.Entity<Profession>().Property(x => x.Name).HasColumnName("profession");

    }


    // RELATIONS 

    private static void MapPersonInvolvedTitle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonInvolvedIn>()
                     .ToTable("person_involved_title")
                     .HasKey(x => new { x.PersonId, x.TitleId});

        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.PersonId).HasColumnName("p_id");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.Job).HasColumnName("job");
        modelBuilder.Entity<PersonInvolvedIn>().Property(x => x.Character).HasColumnName("character");


       modelBuilder.Entity<PersonInvolvedIn>()
            .HasOne(x => x.Person)
            .WithMany(p => p.InvolvedIn)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<PersonInvolvedIn>()
            .HasOne(x => x.Title)
            .WithMany(p => p.PeopleInvolved)
            .HasForeignKey(x => x.TitleId)
            .OnDelete(DeleteBehavior.Cascade);

    }
    private static void MapPersonProfession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonProfession>().ToTable("person_has_a")
                                        .HasKey(x => new { x.ProfessionName, x.PersonId });

        modelBuilder.Entity<PersonProfession>().Property(x => x.PersonId).HasColumnName("p_id");
        modelBuilder.Entity<PersonProfession>().Property(x => x.ProfessionName).HasColumnName("profession");



        modelBuilder.Entity<PersonProfession>()
            .HasOne(x => x.Person)
            .WithMany(p => p.Professions)
            .HasForeignKey(x => x.PersonId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<PersonProfession>()
            .HasOne(x => x.Profession)
            .WithMany(p => p.Persons)
            .HasForeignKey(x => x.ProfessionName)
            .OnDelete(DeleteBehavior.Cascade);
    }


    private void MapTitleGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TitleGenre>().ToTable("title_is")
             .HasKey(x => new { x.TitleId, x.GenreName });

        modelBuilder.Entity<TitleGenre>().Property(x => x.GenreName).HasColumnName("genre");
        modelBuilder.Entity<TitleGenre>().Property(x => x.TitleId).HasColumnName("t_id");


        modelBuilder.Entity<TitleGenre>()
            .HasOne(x => x.Title)
            .WithMany(p => p.Genres)
            .HasForeignKey(x => x.TitleId)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<TitleGenre>()
            .HasOne(x => x.Genre)
            .WithMany(p => p.Titles)
            .HasForeignKey(x => x.GenreName)
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void MapUserReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserReview>().ToTable("rates")
             .HasKey(x => new { x.ReviewId, x.UserId });

        modelBuilder.Entity<UserReview>().Property(x => x.ReviewId).HasColumnName("rev_id");
        modelBuilder.Entity<UserReview>().Property(x => x.UserId).HasColumnName("u_id");


        modelBuilder.Entity<UserReview>()
                    .HasOne(x => x.Review)
                    .WithOne(p => p.createdBy)
                    .HasForeignKey<UserReview>(x => x.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<UserReview>()
                    .HasOne(x => x.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
    }



}
/* INHERITANCE STARTUP NEED SOMETHING
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