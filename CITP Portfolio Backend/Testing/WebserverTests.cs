﻿using System.Net;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using DataLayer.DataServices;


namespace Testing;

public class WebserverTests
{
    private const string titleApi = "http://localhost:5001/api/title";
    private const string personApi = "http://localhost:5001/api/person";
    private const string userApi = "http://localhost:5001/api/user";
    private const string searchApi = "http://localhost:5001/api/search";

    [Fact]
    public async Task CanWefindOnePerson()
    {
        var (person, statusCode) = await GetObject($"{personApi}/nm0658077");

        Assert.Equal(HttpStatusCode.OK, statusCode);
        Assert.Equal("Marcos Palmeira", person?.Value("name"));
    }

    [Fact]
    public async Task CanWeFindAPersonWithInvalidId()
    {
        var (_, statusCode) = await GetObject($"{personApi}/nm06580771");

        Assert.Equal(HttpStatusCode.NotFound, statusCode);
    }


   [Fact]
    public async Task CanGuestCreateAnAccount()
    {
        var newUser = new
        {
            username = "Hoostdorf",
            password = "123",
            email = "amail@amail.ak"
        };

        var (theUser, userResponse) = await PostData($"{userApi}", newUser);
        Assert.Equal(HttpStatusCode.Created, userResponse);



        // Clean up
        var signData = new
        {
            username = "Hoostdorf",
            password = "123"
        };

        var (signInData, response) = await PutData($"{userApi}/sign_in", signData);

        var dataService = new UserDataService();
            var userId = dataService.GetUser(theUser?.Value("username")).Id;

        var token = signInData?.Value("token");
        var deleteResponse = await DeleteData($"{userApi}/{userId}", token);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse);
    }



    [Fact]
    public async Task CanUserSignIn_ValidUsername()
    {
        var newUser = new
        {
            username = "Hallo123",
            password = "123456",
            email = "123@abe.avk"
        };

        var (theUser, userResponse) = await PostData($"{userApi}", newUser);
        Assert.Equal(HttpStatusCode.Created, userResponse);

        // Clean up
        var signData = new
        {
            username = "Hallo123",
            password = "123456",
        };

        var (signInData, response) = await PutData($"{userApi}/sign_in", signData);

        var dataService = new UserDataService();
        var userId = dataService.GetUser(theUser?.Value("username")).Id;
        var token = signInData?.Value("token");
        var deleteResponse = await DeleteData($"{userApi}/{userId}", token);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse);

    }

    [Fact]
    public async Task UserCannotSignInWithInvalidCredentials_ReturnsBadRequest()
    {


        // Clean up
        var signData = new
        {
            username = "Hallo123",
            password = "123451236",
        };

        var (signInData, response) = await PutData($"{userApi}/sign_in", signData);
        Assert.Equal(HttpStatusCode.BadRequest, response);

    }


    // Helper methods

    async Task<(JsonObject?, HttpStatusCode)> GetObject(string url)
    {
        var client = new HttpClient();
        var response = client.GetAsync(url).Result;
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }

    async Task<(JsonObject?, HttpStatusCode)> PostData(string url, object content)
    {
        var client = new HttpClient();
        var requestContent = new StringContent(
            JsonSerializer.Serialize(content),
            Encoding.UTF8,
            "application/json");
        var response = await client.PostAsync(url, requestContent);
        var data = await response.Content.ReadAsStringAsync();
        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }


    async Task<(JsonObject?, HttpStatusCode)> PutData(string url, object content)
    {

        var client = new HttpClient();
        var response = await client.PutAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(content),
                Encoding.UTF8,
                "application/json"));

        var data = await response.Content.ReadAsStringAsync();


        return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
    }



    async Task<HttpStatusCode> DeleteData(string url, string token)
    {
        var client = new HttpClient();
        if (token != null)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        var response = await client.DeleteAsync(url);
        return response.StatusCode;
    }





}
static class HelperExt
{
public static string? Value(this JsonNode node, string name)
{
    var value = node[name];
    return value?.ToString();
}

}


