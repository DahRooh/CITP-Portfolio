using System.Net;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Testing
{
    public class WebserverTests
    {
        private const string titleApi = "http://localhost:5001/api/title";
        private const string personApi = "http://localhost:5001/api/person";
        private const string userApi = "http://localhost:5001/api/user";
        private const string searchApi = "http://localhost:5001/api/search";

        [Fact]
        public async Task GetWithNoArguments_OkOnePerson()
        {
            var (person, statusCode) = await GetObject($"{personApi}/nm0658077");

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.Equal("Marcos Palmeira", person?.Value("name"));
        }

        [Fact]
        public async Task GetWithInvalidId_ReturnsNotFound()
        {
            var (_, statusCode) = await GetObject($"{personApi}/nm06580771");

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task PutWithValidPassword_Ok()
        {

            var newUser = new
            {
                username = "Andreas Hoostdorf",
                password = "123",
                email = "Æmail@ømail.åk"
            };
            var (theUser, _) = await PostData($"{userApi}", newUser);



            var updatedPassword = new
            {
                username = theUser.Value("username"),
                password = theUser.Value("password") + "456",
                email = theUser.Value("email")
            };

            var statusCode = await PutData($"{userApi}", updatedPassword);
            Assert.Equal(HttpStatusCode.OK, statusCode);

            var (use, _) = await GetObject($"{userApi}");
            Assert.Equal(theUser.Value("username"), use?.Value("username"));
            Assert.Equal(theUser.Value("password") + "456", use?.Value("password"));
            Assert.Equal(theUser.Value("email"), use?.Value("email"));

        }




        async Task<(JsonArray?, HttpStatusCode)> GetArray(string url)
        {
            var client = new HttpClient();
            var response = client.GetAsync(url).Result;
            var data = await response.Content.ReadAsStringAsync();
            return (JsonSerializer.Deserialize<JsonArray>(data), response.StatusCode);
        }

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

        async Task<HttpStatusCode> PutData(string url, object content)
        {
            var client = new HttpClient();
            var response = await client.PutAsync(
                url,
                new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    "application/json"));
            return response.StatusCode;
        }

        async Task<HttpStatusCode> DeleteData(string url)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync(url);
            return response.StatusCode;
        }


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