using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CalculatorService.Client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var baseUrl = "https://localhost:7252"; // Replace with the actual base URL of your CalculatorService

            using (var httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(baseUrl);
                
                await SumTestAsync(httpClient);
                await SubsTractTestAsync(httpClient);
                await MultiplyTestAsync(httpClient);
                await DivideTestAsync(httpClient);
                await SquareRootTestAsync(httpClient);

                await TrackingIdTestAsync(httpClient);
            }

            Console.ReadLine();
        }

        private static async Task SumTestAsync(HttpClient httpClient)
        {
            //Tracking Test
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-Evi-Tracking-Id", "657");

            var sum = new List<double> { 3, 3, 2 };
            var request = new { Addends = sum };
            var response = await PostAsync<string>(httpClient, "/api/calculator/add", request);
            Console.WriteLine($"Addition Result: {response}");

            httpClient.DefaultRequestHeaders.Remove("X-Evi-Tracking-Id");

        }

        private static async Task SubsTractTestAsync(HttpClient httpClient)
        {
            
            //// Perform subtraction
            var minuend = new List<double> { 10, 7, 5 };
            var subtrahend = new List<double> { 2, 1, 3 };
            var request = new { Minuend = minuend, Subtrahend = subtrahend};
            var response = await PostAsync<double>(httpClient, "/api/calculator/subtract", request);
            Console.WriteLine($"Subtraction Result: {response}");


        }

        private static async Task MultiplyTestAsync(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var multiply = new List<double> { 1543, 2, 3 };
            var request = new { Factors = multiply };
            var response = await PostAsync<double>(httpClient, "/api/calculator/multiply", request);
            Console.WriteLine($"multiply Result: {response}");


        }

        private static async Task DivideTestAsync(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var divisor = new List<double> { 10, 7, 5 };
            var dividend = new List<double> { 2, 1, 3 };
            var request = new { Divisor = divisor, Dividend = dividend };
            var response = await PostAsync<double>(httpClient, "/api/calculator/divide", request);
            Console.WriteLine($"divide Result: {response}");


        }

        private static async Task SquareRootTestAsync(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var numbers = new List<double> { 10, 7, 5 };
            var request = new { Numbers = numbers };
            var response = await PostAsync<double>(httpClient, "/api/calculator/sqrt", request);
            Console.WriteLine($"SquareRoot Result: {response}");


        }

        private static async Task TrackingIdTestAsync(HttpClient httpClient)
        {
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var trackingId = "657";
            var journalResponse = await GetAsync<string>(httpClient, $"/api/calculator/journal/{trackingId}");
            Console.WriteLine("Journal Entries:" + journalResponse);

        }

        private static async Task<string> PostAsync<T>(HttpClient httpClient, string url, object data)
        {
            string responseContent = "";
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error" + ex.Message);
            }
            return responseContent;
        }

        private static async Task<string> GetAsync<T>(HttpClient httpClient, string url)
        {
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            return responseContent;
        }

        //private static async Task<T> PostAsync<T>(HttpClient httpClient, string url, object data)
        //{
        //    var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        //    var response = await httpClient.PostAsync(url, content);
        //    response.EnsureSuccessStatusCode();
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<T>(responseContent);
        //}

        //private static async Task<T> GetAsync<T>(HttpClient httpClient, string url)
        //{
        //    var response = await httpClient.GetAsync(url);
        //    response.EnsureSuccessStatusCode();
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    return JsonSerializer.Deserialize<T>(responseContent);
        //}
    }
}