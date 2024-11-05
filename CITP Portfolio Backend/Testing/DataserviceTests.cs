﻿using DataLayer;
using DataLayer.DataServices;
using DataLayer.HelperMethods;
using DataLayer.Model.User;
using Xunit;

namespace Testing;

public class DataserviceTests
{
    [Fact]
    public void CreateUserInDatabase()
    {
        var ds = new UserDataService();
        var newUser = new CreateUserModel()
        {
            Username = "Test",
            Password = "Test",
            Email = "Test"
        };
        var user = ds.CreateUser(newUser);

        Assert.Equal("Test", user.Username);
        Assert.Equal("Test", user.Email);

        // cleanup
        ds.DeleteUser(user.Id);
    }

    [Fact]
    public void UserLoginWithValidPassword()
    {
        var ds = new UserDataService();
        var newUser = new CreateUserModel()
        {
            Username = "Test",
            Password = "Test",
            Email = "Test"
        };
        var user = ds.CreateUser(newUser);
        var hashing = new Hashing();
        var login = hashing.Verify("Test", user.Password, user.Salt);

        Assert.True(login);
        ds.DeleteUser(user.Id);
    }

    [Fact]
    public void NewUserShouldHaveEmptyLists()
    {
        var ds = new UserDataService();
        var newUser = new CreateUserModel()
        {
            Username = "Test",
            Password = "Test",
            Email = "Test"
        };
        var user = ds.CreateUser(newUser);
        var newlyCreatedUser = ds.GetUser(user.Id);

        Assert.Equal(0, newlyCreatedUser.UserSessions.Count());
        Assert.Equal(0, newlyCreatedUser.Searches.Count());
        Assert.Equal(0, newlyCreatedUser.UserBookmarks.Count());

        ds.DeleteUser(user.Id);
    }

    [Fact]
    public void UserCanCreateBookmarkAndRetrieveIt()
    {
        var ds = new UserDataService();
        var tds = new TitleDataService();
        var newUser = new CreateUserModel()
        {
            Username = "Test",
            Password = "Test",
            Email = "Test"
        };
        var user = ds.CreateUser(newUser);
        var newlyCreatedUser = ds.GetUser(user.Id);
        tds.CreateBookmark("tt10382912", user.Id);
        var bookmarks = ds.GetBookmarks(user.Id);

        Assert.Equal(1, bookmarks.Count());
        Assert.Equal("tt10382912", bookmarks.First().WebpageBookmark.Webpage.TitleId);
        
        ds.DeleteUser(user.Id);
    }

    [Fact]
    public void UserCanUpdateItsEmail()
    {
        var ds = new UserDataService();
        var newUser = new CreateUserModel()
        {
            Username = "Test",
            Password = "Test",
            Email = "Test"
        };
        var user = ds.CreateUser(newUser);

        var updated = ds.UpdateEmail(user.Username, "UpdatedMail");
        var updatedUser = ds.CreateUser(newUser);

        Assert.True(updated);
        Assert.Equal("UpdatedMail", updatedUser.Email);

        ds.DeleteUser(user.Id);
    }

    [Fact]
    public void AnyoneCanGetAPageOfTitles()
    {
        var tds = new TitleDataService();
        var movies = tds.GetMovies(1, 20);

        Assert.Equal(20, movies.Count());
    }

    [Fact]
    public void AnyoneCanFindCastOfATitle()
    {
        var tds = new TitleDataService();
        var cast = tds.GetCast("tt10382912");

        Assert.Equal("['Amadeu Matheus']", cast[0].Character);
    }

    [Fact]
    public void AnyoneCanFindCoactors()
    {
        var pds = new PersonDataService();
        var coactors = pds.GetCoActors("nm0005212", 1, 20);

        Assert.Equal("Claire Danes", coactors[0].Name);
        Assert.Equal(20, coactors.Count());
    }

}