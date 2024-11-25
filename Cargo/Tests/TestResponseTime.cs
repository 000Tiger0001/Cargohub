using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Newtonsoft.Json;

public class ResponseTimeTests
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ClientAccess _clientAccess;
    private readonly ClientServices _service;
    private readonly HttpClient _httpClient;

    public ResponseTimeTests()
    {
        // Use an in-memory SQLite database for testing
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=cargohub.db")
                        .Options;

        _dbContext = new ApplicationDbContext(options);

        _dbContext.Database.EnsureCreated();

        // Create a new instance of ClientAccess with the in-memory DbContext
        _clientAccess = new ClientAccess(_dbContext);

        // Create new instance of clientService
        _service = new(_clientAccess);

        // Initialize HttpClient for making API requests
        _httpClient = new HttpClient();
    }

    [Theory]
    [InlineData(1, 5)] // 1 second = 1000 ms, 5 requests per endpoint
    public async Task ApiResponseTime_ShouldBeWithinLimit(int maxResponseTimeSeconds, int requestCount)
    {
        // List of endpoints to test
        var endpoints = new List<string>
        {
            "http://localhost:3000/Cargohub/clients",
            // "http://localhost:3000/Cargohub/inventory",
            // "http://localhost:3000/Cargohub/orders"
            // Add more URLs as needed
        };

        double totalResponseTimeSeconds = 0;

        foreach (var url in endpoints)
        {
            Console.WriteLine($"Testing endpoint: {url}");

            for (int i = 1; i <= requestCount; i++)
            {
                var stopwatch = Stopwatch.StartNew();
                try
                {
                    // Send GET request to the specified URL
                    var response = await _httpClient.GetAsync(url);
                    stopwatch.Stop();

                    // Convert milliseconds to seconds
                    double requestTimeSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                    totalResponseTimeSeconds += requestTimeSeconds;

                    // Ensure the request succeeded
                    response.EnsureSuccessStatusCode();

                    // For debug
                    // var content = await response.Content.ReadAsStringAsync();
                    // var items = JsonConvert.DeserializeObject<List<Client>>(content); // Replace with your model
                    // foreach (var item in items)
                    // {
                    //     Console.WriteLine($"Item: {item.Id}");
                    // }

                    // Print individual request time in seconds for debugging
                    Console.WriteLine($"Request {i} took {requestTimeSeconds:F4} seconds");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Request {i} failed: {ex.Message}");
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            }
        }

        double averageResponseTimeSeconds = totalResponseTimeSeconds / (endpoints.Count * requestCount);
        Console.WriteLine($"Average response time: {averageResponseTimeSeconds:F4} seconds");

        // Ensure that the average response time is within the allowed limit
        Assert.True(averageResponseTimeSeconds <= maxResponseTimeSeconds,
            $"Average response time {averageResponseTimeSeconds:F4} seconds exceeds the limit of {maxResponseTimeSeconds} seconds.");
    }
}

