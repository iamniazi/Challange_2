using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using Newtonsoft.Json.Linq;
using NUnit.Framework.Interfaces;
using System.Net;

namespace Challenge_2
{
    [AllureNUnit]
    [AllureEpic("Json placeholder tests")]
    [AllureFeature("Post api tests")]
    public class Tests
    {
        private HttpClient _httpClient;
        private readonly string _endpointPosts = "/posts/1";
        private const string _APIBaseUrl = "https://jsonplaceholder.typicode.com";
        private string _testUuid;
        [SetUp]
        public void Setup()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(_APIBaseUrl)
            };
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Taimor khan")]
        [AllureTag("API", "GET", "Posts")]
        [AllureFeature("Retrieve Posts")]
        [AllureStory("Retrieve a single post by ID")]
        [AllureDescription("This test fetches a specific post (ID = 1) from the JSONPlaceholder API and validates the response data against the expected values.")] // Detailed description
        public async Task GetPost_ShouldReturnExpctedResponse()
        {
            //Arrange
            var expectedData = new
            {
                userId = 1,
                id = 1,
                title = "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
                body = "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
            };

            //Act
            HttpResponseMessage response = await APIHelper.TriggerRequest(_httpClient, HttpMethod.Get, _endpointPosts);
            JObject responseJson = await APIHelper.ParseResponse<JObject>(response);

            //Assert
            APIHelper.AssertStatusCode(response.StatusCode, HttpStatusCode.OK);
            APIHelper.AssertJsonData(expectedData.userId, responseJson["userId"]?.Value<int>(), "userId");
            APIHelper.AssertJsonData(expectedData.id, responseJson["id"]?.Value<int>(), "id");
            APIHelper.AssertJsonData(expectedData.title, responseJson["title"]?.Value<string>(), "title");
            APIHelper.AssertJsonData(expectedData.body, responseJson["body"]?.Value<string>(), "body");
        }
        [Test]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Taimor Khan")]
        [AllureTag("API", "POST", "Posts")]
        [AllureFeature("Create Posts")]
        [AllureDescription("This test sends a POST request to create a new post and validates the response against the input data.")]
        public async Task CreatePost_ShouldReturnNewPostDetails()
        {
            var endpoint = "/posts";
            var postToCreate = new
            {
                userId = 1,
                title = "New Post Title",
                body = "This is the body of the new post."
            };

            HttpResponseMessage response = await APIHelper.TriggerRequest(_httpClient, HttpMethod.Post, endpoint, postToCreate);
            JObject responseJson = await APIHelper.ParseResponse<JObject>(response);

            APIHelper.AssertStatusCode(response.StatusCode, HttpStatusCode.Created);
            APIHelper.AssertJsonData(postToCreate.userId, responseJson["userId"]?.Value<int>(), "userId");
            APIHelper.AssertJsonData(postToCreate.title, responseJson["title"]?.Value<string>(), "title");
            APIHelper.AssertJsonData(postToCreate.body, responseJson["body"]?.Value<string>(), "body");
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Taimor Khan")]
        [AllureTag("API", "PUT", "Posts")]
        [AllureFeature("Update Posts")]
        [AllureDescription("This test sends a PUT request to update an existing post and validates the response against the updated data.")]
        public async Task UpdatePost_ShouldReturnUpdatedPostDetails()
        {
            var postToUpdate = new
            {
                userId = 1,
                title = "Updated Post Title",
                body = "This is the updated body of the post."
            };

            var response = await APIHelper.TriggerRequest(_httpClient, HttpMethod.Put, _endpointPosts, postToUpdate);
            JObject responseJson = await APIHelper.ParseResponse<JObject>(response);

            APIHelper.AssertStatusCode(response.StatusCode, HttpStatusCode.OK);
            APIHelper.AssertJsonData(postToUpdate.userId, responseJson["userId"]?.Value<int>(), "userId");
            APIHelper.AssertJsonData(postToUpdate.title, responseJson["title"]?.Value<string>(), "title");
            APIHelper.AssertJsonData(postToUpdate.body, responseJson["body"]?.Value<string>(), "body");
        }

        [Test]
        [AllureSeverity(SeverityLevel.critical)]
        [AllureOwner("Taimor Khan")]
        [AllureTag("API", "DELETE", "Posts")]
        [AllureFeature("Delete Posts")]
        [AllureDescription("This test sends a DELETE request to remove an existing post and validates the response for successful deletion.")]
        public async Task DeletePost_ShouldReturnSuccess()
        {
            var response = await APIHelper.TriggerRequest(_httpClient, HttpMethod.Delete, _endpointPosts);
            var jsonResponse = await APIHelper.ParseResponse<Dictionary<string, object>>(response);

            Assert.IsTrue(
                response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.NoContent,
                $"Expected status code 200 (OK) or 204 (No Content), but got {(int)response.StatusCode} ({response.StatusCode})"
            );
            Assert.IsNotNull(jsonResponse, "Response body is not a valid JSON object");
            Assert.IsEmpty(jsonResponse, "Response body is not an empty dictionary");
        }


        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }
    }
}