using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataLayer;

public class MVContext : DbContext
{
    public DbSet<Title> Titles { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
        optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        optionsBuilder.UseNpgsql("host=cit.ruc.dk;db=cit16;uid=cit16;pwd=eEWPIoEJet9J");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        MapTitles(modelBuilder);

    }

    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title");

        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("t_id");
        modelBuilder.Entity<Title>().Property(x => x._Title).HasColumnName("title");
    }
}
