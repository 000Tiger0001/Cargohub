// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using System.Net.Http;
// using System.Threading.Tasks;
// using Microsoft.EntityFrameworkCore;
// using Xunit;

// // dotnet test --logger:"console;verbosity=detailed"
// public class ResponseTimeTests
// {
//     private readonly ApplicationDbContext _dbContext;
//     private readonly HttpClient _httpClient;

//     public ResponseTimeTests()
//     {
//         // Use an in-memory SQLite database for testing
//         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                         .UseSqlite("Data Source=cargohub.db")
//                         .Options;

//         _dbContext = new ApplicationDbContext(options);
//         _dbContext.Database.EnsureCreated();

//         // Initialize HttpClient for making API requests
//         _httpClient = new HttpClient();
//     }

//     [InlineData(5, 20)] // 1 second = 1000 ms, n requests per endpoint
//     public async Task ApiResponseTime_ShouldBeWithinLimit(int maxResponseTimeSeconds, int requestCount)
//     {
//         // List of endpoints to test
//         var endpoints = new List<string>
//         {
//             "http://localhost:3000/Cargohub/clients",
//             "http://localhost:3000/Cargohub/get-orders",
//             "http://localhost:3000/Cargohub/locations",
//         };

//         // Track total response time across all requests
//         double totalResponseTimeSeconds = 0;
//         var tasks = new List<Task<double>>(); // Each task will return its response time

//         // Loop through each endpoint separately
//         foreach (var url in endpoints)
//         {
//             Console.WriteLine($"Testing endpoint: {url}");

//             var clientForEndpoint = new HttpClient { BaseAddress = new Uri(url) };

//             for (int i = 1; i <= requestCount; i++)
//             {
//                 tasks.Add(SendRequestAsync(clientForEndpoint, url, i));
//             }
//         }

//         var responseTimes = await Task.WhenAll(tasks);
//         totalResponseTimeSeconds = responseTimes.Sum();

//         double averageResponseTimeSeconds = totalResponseTimeSeconds / (endpoints.Count * requestCount);
//         Console.WriteLine($"Average response time: {averageResponseTimeSeconds:F4} seconds");

//         // Ensure that the average response time is within the allowed limit
//         Assert.True(averageResponseTimeSeconds <= maxResponseTimeSeconds,
//             $"Average response time {averageResponseTimeSeconds:F4} seconds exceeds the limit of {maxResponseTimeSeconds} seconds.");
//     }

//     private async Task<double> SendRequestAsync(HttpClient client, string url, int requestNumber)
//     {
//         var stopwatch = Stopwatch.StartNew();
//         try
//         {
//             // Send GET request to the specified URL
//             var response = await client.GetAsync("");
//             stopwatch.Stop();

//             // Convert milliseconds to seconds
//             double requestTimeSeconds = stopwatch.ElapsedMilliseconds / 1000.0;

//             // Ensure the request succeeded
//             response.EnsureSuccessStatusCode();

//             Console.WriteLine($"Request {requestNumber} to {url} took {requestTimeSeconds:F4} seconds");
//             return requestTimeSeconds;
//         }
//         catch (HttpRequestException ex)
//         {
//             Console.WriteLine($"Request {requestNumber} to {url} failed: {ex.Message}");
//             throw;
//         }
//     }
// }
