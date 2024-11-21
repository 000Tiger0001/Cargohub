using Microsoft.EntityFrameworkCore;
using System.Diagnostics;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// test
// test
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

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxResponseBufferSize = 104857600; // 100 MB
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
//app.UseSession();
app.UseAuthorization();
app.Urls.Add("http://localhost:3000");


var stopwatch = Stopwatch.StartNew();
// Call the method you want to benchmark
await JsonToDb.TransferJsonToDb(app);

stopwatch.Stop();
var elapsedTime = stopwatch.Elapsed;
int minutes = elapsedTime.Minutes;
int seconds = elapsedTime.Seconds;
int milliseconds = elapsedTime.Milliseconds;

Console.WriteLine($"Time taken by TransferJsonToDb: {minutes} minutes, {seconds} seconds, and {milliseconds} milliseconds");
Console.WriteLine($"Time taken by TransferJsonToDb: {elapsedTime.TotalMinutes:F2} minutes");
Console.WriteLine($"Time taken by TransferJsonToDb: {elapsedTime.TotalSeconds:F2} seconds");
Console.WriteLine($"Time taken by TransferJsonToDb: {elapsedTime.TotalMilliseconds:F2} milliseconds");

app.Run();