﻿using DataLayer.DomainObjects;
using DataLayer.DomainObjects.Entities;
using DataLayer.DomainObjects.FunctionResults;
using DataLayer.DomainObjects.Relations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Emit;

namespace DataLayer;

public class MVContext : DbContext
{
    // entities
    public DbSet<Title> Titles { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Search> Searches { get; set; }
    public DbSet<Episode> Episodes { get; set; }
    public DbSet<Person> People { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Webpage> Webpages { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Review> Reviews { get; set; }


    // relations (mostly used)
    public DbSet<UserSessionsHistory> SessionHistory { get; set; }

    public DbSet<WebpageBookmark> WebpageBookmarks { get; set; }
    public DbSet<UserBookmark> UserBookmarks { get; set; }
    public DbSet<UserTitleReview> UserReviews { get; set; }
    public DbSet<UserLikesReview> UserLikesReviews { get; set; }
    public DbSet<InvolvedIn> PersonInvolvedIn { get; set; }
    public DbSet<TitleGenre> TitlesGenres { get; set; }
    public DbSet<UserSearch> UserSearches { get; set; }
    public DbSet<SearchResult> SearchResults { get; set; }
    public DbSet<SearchResultPerson> SearchResultsPerson { get; set; }
    public DbSet<SimilarTitle> SimilarTitles { get; set; }
    public DbSet<CoActor> CoActors { get; set; }
    public DbSet<KnownFor> KnownFor { get; set; }
    public DbSet<Series> Series { get; set; }


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
        MapBookmark(modelBuilder);
        MapPersonProfession(modelBuilder);
        MapTitleGenre(modelBuilder);
        MapReview(modelBuilder);
        MapSession(modelBuilder);

        MapEpisodeAndMovie(modelBuilder);
        MapSimilarTitle(modelBuilder);
        MapCoActor(modelBuilder);
        MapKnownFor(modelBuilder);

        MapPersonInvolvedTitle(modelBuilder);
        MapUsers(modelBuilder);
        MapUserReview(modelBuilder);
        MapUserLikes(modelBuilder);
        MapUserSession(modelBuilder);
        MapUserBookmark(modelBuilder);
        MapUserSearch(modelBuilder);

        MapWebpage(modelBuilder);
        MapSearchResults(modelBuilder);
        MapSearchResultsPerson(modelBuilder);
        MapSeries(modelBuilder);

    }

    private void MapWebpage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Webpage>().ToTable("webpage").HasKey(x => x.Id);

        modelBuilder.Entity<Webpage>().Property(x => x.Id).HasColumnName("wp_id");

        modelBuilder.Entity<Webpage>().Property(x => x.PersonId).HasColumnName("p_id");
        modelBuilder.Entity<Webpage>().Property(x => x.TitleId).HasColumnName("t_id");

        modelBuilder.Entity<Webpage>().Property(x => x.Url).HasColumnName("url");
        modelBuilder.Entity<Webpage>().Property(x => x.ViewCount).HasColumnName("wp_view_count");

        modelBuilder.Entity<Webpage>()
            .HasOne(x => x.Person)
            .WithMany()
            .HasForeignKey(x => x.PersonId).IsRequired(false);

        modelBuilder.Entity<Webpage>()
            .HasOne(x => x.Title)
            .WithMany()
            .HasForeignKey(x => x.TitleId).IsRequired(false);
        
        modelBuilder.Entity<WebpageBookmark>().ToTable("wp_bookmarks").HasKey(x => new { x.BookmarkId, x.WebpageId });
        modelBuilder.Entity<WebpageBookmark>().Property(x => x.BookmarkId).HasColumnName("bookmark_id");
        modelBuilder.Entity<WebpageBookmark>().Property(x => x.WebpageId).HasColumnName("wp_id");

        modelBuilder.Entity<WebpageBookmark>()
            .HasOne(x => x.Bookmark)
            .WithOne(x => x.WebpageBookmark)
            .HasForeignKey<WebpageBookmark>(x => x.BookmarkId);

        modelBuilder.Entity<WebpageBookmark>()
            .HasOne(x => x.Webpage)
            .WithMany(x => x.Bookmarks)
            .HasForeignKey(x => x.WebpageId);
    }

    private static void MapUsers(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users").HasKey(x => x.Id);

        modelBuilder.Entity<User>().Property(x => x.Id).HasColumnName("u_id");
        modelBuilder.Entity<User>().Property(x => x.Password).HasColumnName("password");
        modelBuilder.Entity<User>().Property(x => x.Username).HasColumnName("username");
        modelBuilder.Entity<User>().Property(x => x.Email).HasColumnName("email");
        modelBuilder.Entity<User>().Property(x => x.Salt).HasColumnName("salt");
    }

    private static void MapSearchResults(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchResult>().HasNoKey();
        
        modelBuilder.Entity<SearchResult>().Property(x => x.Id).HasColumnName("id");
        modelBuilder.Entity<SearchResult>().Property(x => x.Text).HasColumnName("title_name");
        modelBuilder.Entity<SearchResult>().Property(x => x.Rating).HasColumnName("title_rating");
        modelBuilder.Entity<SearchResult>().Property(x => x.Poster).HasColumnName("title_poster");
        modelBuilder.Entity<SearchResult>().Property(x => x.Type).HasColumnName("type");
    }

    private static void MapSearchResultsPerson(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SearchResultPerson>().HasNoKey();

        modelBuilder.Entity<SearchResultPerson>().Property(x => x.WebpageId).HasColumnName("webpage_id");
    }
    


    private static void MapReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Review>().ToTable("review").HasKey(x => x.Id);

        modelBuilder.Entity<Review>().Property(x => x.Id).HasColumnName("rev_id");
        modelBuilder.Entity<Review>().Property(x => x.Likes).HasColumnName("likes");
        modelBuilder.Entity<Review>().Property(x => x.Text).HasColumnName("review");
        modelBuilder.Entity<Review>().Property(x => x.Caption).HasColumnName("caption");
    }

    private static void MapGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().ToTable("genre").HasKey(x => x._Genre);

        modelBuilder.Entity<Genre>().Property(x => x._Genre).HasColumnName("genre");  
    }

    private static void MapTitles(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Title>().ToTable("title").HasKey(x => x.Id);;


        modelBuilder.Entity<Title>().Property(x => x.Id).HasColumnName("t_id");
        modelBuilder.Entity<Title>().Property(x => x._Title).HasColumnName("title");
        modelBuilder.Entity<Title>().Property(x => x.Plot).HasColumnName("plot");
        modelBuilder.Entity<Title>().Property(x => x.Rating).HasColumnName("rating");
        modelBuilder.Entity<Title>().Property(x => x.Type).HasColumnName("type");
        modelBuilder.Entity<Title>().Property(x => x.Titletype).HasColumnName("titletype");
        modelBuilder.Entity<Title>().Property(x => x.IsAdult).HasColumnName("isadult");
        modelBuilder.Entity<Title>().Property(x => x.Released).HasColumnName("released");
        modelBuilder.Entity<Title>().Property(x => x.Language).HasColumnName("language");
        modelBuilder.Entity<Title>().Property(x => x.Country).HasColumnName("country");
        modelBuilder.Entity<Title>().Property(x => x.RunTime).HasColumnName("runtime");
        modelBuilder.Entity<Title>().Property(x => x.Poster).HasColumnName("poster");

    }

   private static void MapEpisodeAndMovie(ModelBuilder modelBuilder)
    {
        // movies

        modelBuilder.Entity<Movie>().ToTable("movie").HasKey(x => x.Id);
        modelBuilder.Entity<Movie>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<Movie>().Property(x => x.Id).HasColumnName("mov_id");


        modelBuilder.Entity<Movie>()
               .HasOne(x => x.Title)
               .WithOne()
               .HasForeignKey<Movie>(e => e.TitleId);


        // episodes
        
        modelBuilder.Entity<Episode>().ToTable("episode").HasKey(x => x.Id);

        modelBuilder.Entity<Episode>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<Episode>().Property(x => x.Id).HasColumnName("ep_id");
        modelBuilder.Entity<Episode>().Property(x => x.Type).HasColumnName("titletype");

        modelBuilder.Entity<Episode>().Property(x => x.SeasonNumber).HasColumnName("season_num");
        modelBuilder.Entity<Episode>().Property(x => x.EpisodeNumber).HasColumnName("ep_num");

        modelBuilder.Entity<Episode>()
                       .HasOne(x => x.Title)
                       .WithOne()
                       .HasForeignKey<Episode>(e => e.TitleId);


    }

    private static void MapPerson(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Person>().ToTable("person")
                                     .HasKey(x => x.Id);

        modelBuilder.Entity<Person>().Property(x => x.Id).HasColumnName("p_id");
        modelBuilder.Entity<Person>().Property(x => x.Name).HasColumnName("name");
        modelBuilder.Entity<Person>().Property(x => x.BirthYear).HasColumnName("birth_year");
        modelBuilder.Entity<Person>().Property(x => x.DeathYear).HasColumnName("death_year");
        modelBuilder.Entity<Person>().Property(x => x.PersonRating).HasColumnName("person_rating");
    }


    private static void MapProfession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profession>().ToTable("profession").HasKey(x => x.Name);
        modelBuilder.Entity<Profession>().Property(x => x.Name).HasColumnName("profession");
    }

    private static void MapSession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSessionsHistory>().HasNoKey();
        modelBuilder.Entity<UserSessionsHistory>().Property(x => x.Id).HasColumnName("session_id");
        modelBuilder.Entity<UserSessionsHistory>().Property(x => x.UserId).HasColumnName("user_id");
        modelBuilder.Entity<UserSessionsHistory>().Property(x => x.SessionStart).HasColumnName("timecreated");
        modelBuilder.Entity<UserSessionsHistory>().Property(x => x.SessionEnd).HasColumnName("timeended");
        modelBuilder.Entity<UserSessionsHistory>().Property(x => x.Expiration).HasColumnName("expired");

        modelBuilder.Entity<Session>().ToTable("session").HasKey(x => x.Id);
        modelBuilder.Entity<Session>().Property(x => x.Id).HasColumnName("session_id");
        modelBuilder.Entity<Session>().Property(x => x.SessionStart).HasColumnName("session_start");
        modelBuilder.Entity<Session>().Property(x => x.SessionEnd).HasColumnName("session_end");
        modelBuilder.Entity<Session>().Property(x => x.Expiration).HasColumnName("expiration");
    }

    private static void MapBookmark(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Bookmark>().ToTable("bookmark").HasKey(x => x.Id);

        modelBuilder.Entity<Bookmark>().Property(x => x.Id).HasColumnName("bookmark_id");
        modelBuilder.Entity<Bookmark>().Property(x => x.CreatedAt).HasColumnName("bookmarked_at");
    }




    // RELATIONS 

    private static void MapPersonInvolvedTitle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<InvolvedIn>().ToTable("person_involved_title").HasKey(x => new { x.Id, x.TitleId});

        modelBuilder.Entity<InvolvedIn>().Property(x => x.Id).HasColumnName("p_id");
        modelBuilder.Entity<InvolvedIn>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<InvolvedIn>().Property(x => x.Job).HasColumnName("job");
        modelBuilder.Entity<InvolvedIn>().Property(x => x.Character).HasColumnName("character");


       modelBuilder.Entity<InvolvedIn>()
            .HasOne(x => x.Person)
            .WithMany(p => p.InvolvedIn)
            .HasForeignKey(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<InvolvedIn>()
            .HasOne(x => x.Title)
            .WithMany(p => p.PeopleInvolved)
            .HasForeignKey(x => x.TitleId)
            .OnDelete(DeleteBehavior.Cascade);

    }
    private static void MapPersonProfession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PersonProfession>().ToTable("person_has_a").HasKey(x => new { x.ProfessionName, x.PersonId });

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


    private static void MapTitleGenre(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TitleGenre>().ToTable("title_is").HasKey(x => new { x.GenreName, x.TitleId });

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

    private static void MapUserReview(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserTitleReview>().ToTable("rates").HasKey(x => new { x.ReviewId, x.UserId, x.TitleId });

        modelBuilder.Entity<UserTitleReview>().Property(x => x.ReviewId).HasColumnName("rev_id");
        modelBuilder.Entity<UserTitleReview>().Property(x => x.TitleId).HasColumnName("t_id");
        modelBuilder.Entity<UserTitleReview>().Property(x => x.UserId).HasColumnName("u_id");
        modelBuilder.Entity<UserTitleReview>().Property(x => x.Rating).HasColumnName("rating");


        modelBuilder.Entity<UserTitleReview>()
                    .HasOne(x => x.Review)
                    .WithOne(p => p.createdBy)
                    .HasForeignKey<UserTitleReview>(x => x.ReviewId)
                    .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<UserTitleReview>()
                    .HasOne(x => x.User)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<UserTitleReview>()
                    .HasOne(x => x.Title)
                    .WithMany(p => p.Reviews)
                    .HasForeignKey(x => x.TitleId)
                    .OnDelete(DeleteBehavior.Cascade);
    }

    private static void MapUserLikes(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserLikesReview>().ToTable("likes").HasKey(x => new { x.ReviewId, x.UserId });

        modelBuilder.Entity<UserLikesReview>().Property(x => x.UserId).HasColumnName("u_id");
        modelBuilder.Entity<UserLikesReview>().Property(x => x.ReviewId).HasColumnName("rev_id");
        modelBuilder.Entity<UserLikesReview>().Property(x => x.Liked).HasColumnName("liked");
        
        modelBuilder.Entity<UserLikesReview>()
            .HasOne(x => x.Review)
            .WithMany(r => r.UserLikes)
            .HasForeignKey(x => x.ReviewId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserLikesReview>()
            .HasOne(x => x.User)
            .WithMany(u => u.UserLikes)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void MapUserSession(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserSession>().ToTable("user_sessions").HasKey(x => new { x.SessionId, x.UserId });
        
        modelBuilder.Entity<UserSession>().Property(x => x.SessionId).HasColumnName("session_id");
        modelBuilder.Entity<UserSession>().Property(x => x.UserId).HasColumnName("u_id");
        
        modelBuilder.Entity<UserSession>()
            .HasOne(x => x.Session)
            .WithOne(us => us.UserSessions)
            .HasForeignKey<UserSession>(x => x.SessionId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<UserSession>()
            .HasOne(x => x.User)
            .WithMany(us => us.UserSessions)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void MapUserBookmark(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserBookmark>().ToTable("user_bookmarks").HasKey(x => new { x.BookmarkId, x.UserId });

        modelBuilder.Entity<UserBookmark>().Property(x => x.BookmarkId).HasColumnName("bookmark_id");
        modelBuilder.Entity<UserBookmark>().Property(x => x.UserId).HasColumnName("u_id");

        modelBuilder.Entity<UserBookmark>()
            .HasOne(x => x.Bookmark)
            .WithOne(us => us.BookmarkedBy)
            .HasForeignKey<UserBookmark>(x => x.BookmarkId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserBookmark>()
            .HasOne(x => x.User)
            .WithMany(us => us.UserBookmarks)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void MapUserSearch(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Search>().ToTable("search").HasKey(x => x.Id);
        modelBuilder.Entity<Search>().Property(x => x.Id).HasColumnName("search_id");
        modelBuilder.Entity<Search>().Property(x => x.Keyword).HasColumnName("keyword");
        modelBuilder.Entity<Search>().Property(x => x.CreatedAt).HasColumnName("searched_at");




        modelBuilder.Entity<UserSearch>().ToTable("history").HasKey(x => new { x.SearchId, x.UserId });

        modelBuilder.Entity<UserSearch>().Property(x => x.SearchId).HasColumnName("search_id");
        modelBuilder.Entity<UserSearch>().Property(x => x.UserId).HasColumnName("u_id");

        modelBuilder.Entity<UserSearch>()
            .HasOne(x => x.Search)
            .WithOne(us => us.UserSearch)
            .HasForeignKey<UserSearch>(x => x.SearchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserSearch>()
            .HasOne(x => x.User)
            .WithMany(us => us.Searches)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public static void MapSimilarTitle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SimilarTitle>().HasNoKey();

        modelBuilder.Entity<SimilarTitle>().Property(x => x.Id).HasColumnName("similar_title_id");
        modelBuilder.Entity<SimilarTitle>().Property(x => x._Title).HasColumnName("similar_title");
        modelBuilder.Entity<SimilarTitle>().Property(x => x.Rating).HasColumnName("title_rating");
        modelBuilder.Entity<SimilarTitle>().Property(x => x.CommonGenres).HasColumnName("multiple_same_genre");

    }

    public static void MapSeries(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Series>().HasNoKey();

        modelBuilder.Entity<Series>().Property(x => x.Id).HasColumnName("e_id");
        modelBuilder.Entity<Series>().Property(x => x.Name).HasColumnName("ep_name");
        modelBuilder.Entity<Series>().Property(x => x.Plot).HasColumnName("ep_plot");
        modelBuilder.Entity<Series>().Property(x => x.Rating).HasColumnName("ep_rating");
        modelBuilder.Entity<Series>().Property(x => x.Type).HasColumnName("ep_type");
        modelBuilder.Entity<Series>().Property(x => x.IsAdult).HasColumnName("adult");
        modelBuilder.Entity<Series>().Property(x => x.Released).HasColumnName("release_date");
        modelBuilder.Entity<Series>().Property(x => x.Language).HasColumnName("language");
        modelBuilder.Entity<Series>().Property(x => x.Country).HasColumnName("country");
        modelBuilder.Entity<Series>().Property(x => x.RunTime).HasColumnName("runtime");
        modelBuilder.Entity<Series>().Property(x => x.Poster).HasColumnName("poster");
        modelBuilder.Entity<Series>().Property(x => x.Titletype).HasColumnName("title_type");
        modelBuilder.Entity<Series>().Property(x => x.EpisodeNum).HasColumnName("episode_number");
        modelBuilder.Entity<Series>().Property(x => x.SeasonNum).HasColumnName("season_number");
    }

    public static void MapCoActor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CoActor>().HasNoKey();

        modelBuilder.Entity<CoActor>().Property(x => x.PersonId).HasColumnName("person_id");
        modelBuilder.Entity<CoActor>().Property(x => x.Name).HasColumnName("co_actors");
        modelBuilder.Entity<CoActor>().Property(x => x.Rating).HasColumnName("co_rating");
       

    }

    public static void MapKnownFor(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<KnownFor>().HasNoKey();

        modelBuilder.Entity<KnownFor>().Property(x => x.KnownForTitle).HasColumnName("knownfortitles");
        modelBuilder.Entity<KnownFor>().Property(x => x.KnownForId).HasColumnName("titleid");
        modelBuilder.Entity<KnownFor>().Property(x => x.Rating).HasColumnName("rating");


    }

}
