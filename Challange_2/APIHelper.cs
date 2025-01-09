using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Challenge_2
{
    public static class APIHelper
    {
        private static readonly string LogFilePath = "TestExecutionLogs.txt";

        public static async Task<HttpResponseMessage> TriggerRequest(HttpClient httpClient, HttpMethod httpMethod, string endpoint, object requestData = null)
        {
            HttpRequestMessage request = new HttpRequestMessage(httpMethod, endpoint);
            if (requestData is not null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");
                Log($"Request Body: {JsonConvert.SerializeObject(requestData, Formatting.Indented)}");
            }
            Log($"Request: {httpMethod} {httpClient.BaseAddress}{endpoint}");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            Log($"Response Status: {response.StatusCode}");
            string responseBody = await response.Content.ReadAsStringAsync();
            Log($"Response Body: {responseBody}");

            return response;
        }

        public static async Task<T> ParseResponse<T>(HttpResponseMessage responseMessage)
        {
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        public static void AssertStatusCode(HttpStatusCode responseStatusCode, HttpStatusCode expectedStatusCode)
        {
            Log($"Asserting Status Code: Expected {expectedStatusCode}, Got {responseStatusCode}");
            Assert.That(responseStatusCode, Is.EqualTo(expectedStatusCode), $"Expected status code {expectedStatusCode}, but got {responseStatusCode}");
        }

        public static void AssertJsonData<T>(T expected, T actual, string fieldName)
        {
            Log($"Asserting Field '{fieldName}': Expected {expected}, Got {actual}");
            Assert.That(actual, Is.EqualTo(expected), $"Field '{fieldName}' does not match. Expected Field: {expected}, but got: {actual}");
        }

        private static void Log(string message)
        {
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            Console.WriteLine(logMessage);
            File.AppendAllText(LogFilePath, logMessage + Environment.NewLine);
        }
    }
}
