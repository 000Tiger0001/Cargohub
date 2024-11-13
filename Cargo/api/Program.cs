using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddControllers();


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

builder.Services.AddTransient<LocationAccess>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));



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

app.UseAuthorization();
app.Urls.Add("http://localhost:3000");



string folderPath = "data";
string path = $"{folderPath}/clients.json";

// Ensure the folder exists, create otherwise
if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

StreamReader reader;
List<Client> items = [];

if (File.Exists(path))
{
    reader = new(path);
    string content = await reader.ReadToEndAsync();
    items = JsonConvert.DeserializeObject<List<Client>>(content) ?? new List<Client>();
    reader.Close();
    reader.Dispose();
}
Console.WriteLine(items);
foreach (var item in items)
{
    Console.WriteLine(item.Name);
}
app.Run();