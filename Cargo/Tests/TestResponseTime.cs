// using System.Diagnostics;
// using Microsoft.EntityFrameworkCore;
// using Xunit;
// // make sure the server is running
// // Run the tests using: dotnet test --logger:"console;verbosity=detailed"
// public class ResponseTimeTests
// {
//     private readonly ApplicationDbContext _dbContext;
//     private readonly HttpClient _httpClient;

//     public ResponseTimeTests()
//     {
//         // Use an in-memory SQLite database
//         var options = new DbContextOptionsBuilder<ApplicationDbContext>()
//                         .UseSqlite("Data Source=cargohub.db")
//                         .Options;

//         _dbContext = new ApplicationDbContext(options);
//         _dbContext.Database.EnsureCreated();

//         // Initialize a single HttpClient instance
//         _httpClient = new HttpClient();
//     }

//     [Theory]
//     [InlineData(5)] // requestCount
//     public async Task ApiResponseTime(int requestCount)
//     {
//         // List of endpoints to test
//         var endpoints = new List<string>
//         {
//             "http://localhost:3000/Cargohub/clients",
//             "http://localhost:3000/Cargohub/client/1",

//             "http://localhost:3000/Cargohub/inventories",
//             "http://localhost:3000/Cargohub/inventory/1",
//             "http://localhost:3000/Cargohub/inventories/item/1",
//             "http://localhost:3000/Cargohub/inventories/item/1/totals",

//             "http://localhost:3000/Cargohub/items",
//             "http://localhost:3000/Cargohub/item/1",

//             "http://localhost:3000/Cargohub/item-types",
//             "http://localhost:3000/Cargohub/item-type/1",
//             "http://localhost:3000/Cargohub/item-type/1/items",

//             "http://localhost:3000/Cargohub/item-lines",
//             "http://localhost:3000/Cargohub/item-line/1",
//             "http://localhost:3000/Cargohub/item-line/1/items",

//             "http://localhost:3000/Cargohub/item-groups",
//             "http://localhost:3000/Cargohub/item-group/1",
//             "http://localhost:3000/Cargohub/item-group/1/items",

//             "http://localhost:3000/Cargohub/locations",
//             "http://localhost:3000/Cargohub/location/1",

//             "http://localhost:3000/Cargohub/orders",
//             "http://localhost:3000/Cargohub/order/1",
//             "http://localhost:3000/Cargohub/order/1/items",
//             "http://localhost:3000/Cargohub/client/1/orders",
//             "http://localhost:3000/Cargohub/shipment/1/orders",

//             "http://localhost:3000/Cargohub/locations",
//             "http://localhost:3000/Cargohub/location/1",

//             "http://localhost:3000/Cargohub/shipments",
//             "http://localhost:3000/Cargohub/shipment/1",
//             "http://localhost:3000/Cargohub/shipment/1/items",

//             "http://localhost:3000/Cargohub/suppliers",
//             "http://localhost:3000/Cargohub/supplier/1",

//             "http://localhost:3000/Cargohub/transfers",
//             "http://localhost:3000/Cargohub/transfer/1",
//             "http://localhost:3000/Cargohub/transfer/1/items",

//             "http://localhost:3000/Cargohub/warehouse/1/locations",

//             "http://localhost:3000/Cargohub/warehouses",
//             "http://localhost:3000/Cargohub/warehouse/1",
//         };

//         // Track total response time across all requests
//         double totalResponseTimeSeconds;
//         var tasks = new List<Task<double>>(); // Each task will return its response time

//         // Loop through each endpoint and create tasks for concurrent requests
//         foreach (var url in endpoints)
//         {
//             Console.WriteLine($"Testing endpoint: {url}");

//             for (int i = 1; i <= requestCount; i++)
//             {
//                 tasks.Add(SendRequestAsync(_httpClient, url, i));
//             }
//         }

//         // Wait for all requests to complete
//         var responseTimes = await Task.WhenAll(tasks);
//         totalResponseTimeSeconds = responseTimes.Sum();

//         double averageResponseTimeSeconds = totalResponseTimeSeconds / (endpoints.Count * requestCount);
//         Console.WriteLine($"Average response time: {averageResponseTimeSeconds:F4} seconds");
//     }

//     private async Task<double> SendRequestAsync(HttpClient client, string url, int requestNumber)
//     {
//         var stopwatch = Stopwatch.StartNew();
//         try
//         {
//             // Send GET request to the specified URL
//             var response = await client.GetAsync(url);
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
//             stopwatch.Stop();
//             Console.WriteLine($"Request {requestNumber} to {url} failed: {ex.Message}");
//             throw;
//         }
//     }
// }
