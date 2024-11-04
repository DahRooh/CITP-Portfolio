using System.Net;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DataLayer.HelperMethods;

namespace Testing
{
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
        public async Task CanWeUpdateAUserPassword()
        {
            var newUser = new
            {
                username = "Andreas Hoostdorf",
                password = "123",
                email = "Æmail@ømail.åk"
            };

            var (theUser, userResponse) = await PostData($"{userApi}/CreateUser", newUser);
            Assert.Equal(HttpStatusCode.Created, userResponse);


            var signData = new
            {
                username = "Andreas Hoostdorf",
                password = "123"
            };

            var (signInData, response) = await PutData($"{userApi}/sign_in", signData);
            var token = signInData?.Value("token");


            var updatedPassword = new
            {
                username = "Andreas Hoostdorf",
                password = "123456",
                email = "Æmail@ømail.åk"
            };

            var (updatedUser, statusCode) = await PutDataWithAuth($"{userApi}/1/update_password", updatedPassword, token);
            Assert.Equal(HttpStatusCode.OK, statusCode);

            var newSignIn = new
            {
                username = "Andreas Hoostdorf",
                password = "123456"
            };

            
            var (newSignInData, newResponse) = await PutData($"{userApi}/sign_in", newSignIn);

            Assert.Equal(signInData?.Value("username"), newSignInData?.Value("username"));
            Assert.Equal(signInData?.Value("password"), newSignInData?.Value("password"));
            Assert.Equal(signInData?.Value("email"), newSignInData?.Value("email"));
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

        async Task<(JsonObject?, HttpStatusCode)> PutDataWithAuth(string url, object content, string token)
        {
            var client = new HttpClient();
            if (token != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.PutAsync(
                url,
                new StringContent(
                    JsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    "application/json"));
            var data = await response.Content.ReadAsStringAsync();

            return (JsonSerializer.Deserialize<JsonObject>(data), response.StatusCode);
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