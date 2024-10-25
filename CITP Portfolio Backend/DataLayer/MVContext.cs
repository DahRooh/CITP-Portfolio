using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataLayer;

public class MVContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    public DbSet<Person> People { get; set; }
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
    }

    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title");

        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("t_id");
        modelBuilder.Entity<Title>().Property(x => x._Title).HasColumnName("title");
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
            .HasMany(p => p.Professions)
            .WithMany(pr => pr.Persons)
            .UsingEntity(
                "person_has_a",
                p => p.HasOne(typeof(Person))
                                            .WithMany()
                                            .HasForeignKey("p_id")
                                            .HasPrincipalKey(nameof(Person.Id)), 
                pr => pr.HasOne(typeof(Profession))
                                            .WithMany()
                                            .HasForeignKey("profession")
                                            .HasPrincipalKey(nameof(Profession.Name)),
                j => j.HasKey("p_id", "profession"));
        
                                        
        modelBuilder.Entity<Person>()
            .HasMany(p => p.InvolvedIn)
            .WithOne(pi => pi.Person)
            .HasForeignKey(pi => pi.PersonId);
    }
}
