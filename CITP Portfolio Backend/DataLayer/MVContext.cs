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
        MapTitles(modelBuilder);
        MapPerson(modelBuilder);
        MapProfession(modelBuilder);
        MapPersonInvolvedTitle(modelBuilder);
    }

    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title")
                                    .HasKey(x => x.Id);

        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("t_id");
        modelBuilder.Entity<Title>().Property(x => x._Title).HasColumnName("title");

        modelBuilder.Entity<Title>()
                    .HasMany(p => p.peopleInvolved)
                    .WithOne(pi => pi.Title)
                    .HasForeignKey(pi => pi.TitleId);
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
            .UsingEntity<Dictionary<string, object>>(
            "person_has_a", 
            j => j.HasOne<Profession>()
                  .WithMany()
                  .HasForeignKey("profession") 
                  .HasPrincipalKey(pr => pr.Name), 

            j => j.HasOne<Person>()
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
