using System.Net;
using System.Text.Json.Nodes;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;


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
        public async Task CanGuestCreateAnAccount()
        {
            var newUser = new
            {
                username = "Hoostdorf",
                password = "123",
                email = "Æmail@ømail.åk"
            };

            var (theUser, userResponse) = await PostData($"{userApi}", newUser);
            Assert.Equal(HttpStatusCode.Created, userResponse);

            var signData = new
            {
                username = "Hoostdorf",
                password = "123"
            };

            var (signInData, response) = await PutData($"{userApi}/sign_in", signData);
            Assert.Equal(HttpStatusCode.Created, response);
            var token = signInData?.Value("token");

            // Clean up
            var deleteResponse = await DeleteData($"{userApi}/1", token);
            Assert.Equal(HttpStatusCode.OK, deleteResponse);


        }


        [Fact]
        public async Task CanUserSignIn_Valid()
        {

            var signData = new
            {
                username = "Hallo123",
                password = "123456"
            };

            var (signInData, response) = await PutData($"{userApi}/sign_in", signData);
            Assert.Equal(HttpStatusCode.Created, response);

        }

        [Fact]
        public async Task CanUserSignIn_Invalid()
        {

            var signData = new
            {
                username = "Hallo1234",
                password = "123456"
            };

            var (signInData, response) = await PutData($"{userApi}/sign_in", signData);
            Assert.Equal(HttpStatusCode.Created, response);


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

}
static class HelperExt
{
    public static string? Value(this JsonNode node, string name)
    {
        var value = node[name];
        return value?.ToString();
    }

}


