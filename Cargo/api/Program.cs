using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(@"C:\DataProtectionKeys"))
    .SetApplicationName("MyApp");
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".MyApp.Session";
});
builder.Services.AddTransient<ClientServices>();
builder.Services.AddTransient<InventoryServices>();
builder.Services.AddTransient<ItemGroupServices>();
builder.Services.AddTransient<ItemLineServices>();
builder.Services.AddTransient<ItemServices>();
builder.Services.AddTransient<ItemTypeServices>();
builder.Services.AddTransient<LocationServices>();
builder.Services.AddTransient<OrderServices>();
builder.Services.AddTransient<ShipmentServices>();
builder.Services.AddTransient<SupplierServices>();
builder.Services.AddTransient<TransferServices>();
builder.Services.AddTransient<WarehouseServices>();
builder.Services.AddTransient<UserServices>();

// Access to DB
builder.Services.AddTransient<ClientAccess>();
builder.Services.AddTransient<InventoryAccess>();
builder.Services.AddTransient<ItemAccess>();
builder.Services.AddTransient<ItemGroupAccess>();
builder.Services.AddTransient<ItemLineAccess>();
builder.Services.AddTransient<ItemTypeAccess>();
builder.Services.AddTransient<LocationAccess>();
builder.Services.AddTransient<OrderAccess>();
builder.Services.AddTransient<ShipmentAccess>();
builder.Services.AddTransient<SupplierAccess>();
builder.Services.AddTransient<TransferAccess>();
builder.Services.AddTransient<WarehouseAccess>();
builder.Services.AddTransient<OrderItemMovementAccess>();
builder.Services.AddTransient<TransferItemMovementAccess>();
builder.Services.AddTransient<ShipmentItemMovementAccess>();
builder.Services.AddTransient<UserAccess>();

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Cargo API",
        Version = "v1.0.1"
    });

    c.AddServer(new Microsoft.OpenApi.Models.OpenApiServer
    {
        Url = "https://localhost:3000"
    });
});

// DB
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxResponseBufferSize = 104857600; // 100 MB
});


WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.yaml", "API v1.0.1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.MapControllers();
app.UseAuthorization();
app.Urls.Add("https://localhost:3000");


var stopwatch = Stopwatch.StartNew();

// Call the method to benchmark
await JsonToDb.TransferJsonToDb(app);

stopwatch.Stop();
var elapsedTime = stopwatch.Elapsed;
int minutes = elapsedTime.Minutes;
int seconds = elapsedTime.Seconds;
int milliseconds = elapsedTime.Milliseconds;

Console.WriteLine($"Time taken by TransferJsonToDb: {minutes} minutes, {seconds} seconds, and {milliseconds} milliseconds");

app.Run();