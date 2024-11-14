using Microsoft.EntityFrameworkCore;
using System.Text;
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

// Access to DB
builder.Services.AddTransient<ClientAccess>();
builder.Services.AddTransient<LocationAccess>();
builder.Services.AddTransient<InventoryAccess>();
builder.Services.AddTransient<ItemAccess>();

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


using var scope = app.Services.CreateScope();
var clientAccess = scope.ServiceProvider.GetRequiredService<ClientAccess>();

string folderPath = "data";
string path = $"{folderPath}/clients.json";

// Ensure the folder exists, create otherwise
if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);


StreamReader reader;
List<Client> items = new();

if (File.Exists(path))
{
    reader = new(path);
    string content = await reader.ReadToEndAsync();
    // Optionally decode any Unicode escape sequences if required
    content = DecodeUnicodeEscapeSequences(content);
    items = JsonConvert.DeserializeObject<List<Client>>(content) ?? new List<Client>();
    reader.Close();
    reader.Dispose();
}

foreach (var item in items)
{
    Console.WriteLine($"Client ID: {item.Id}, Name: {item.Name}");
    await clientAccess.Add(item);
    if (clientAccess.GetById(item.Id) == null)
    {
        Console.WriteLine(item.Id);
        break;
    }
}


static string DecodeUnicodeEscapeSequences(string input)
{
    byte[] byteArray = Encoding.Default.GetBytes(input);
    return Encoding.UTF8.GetString(byteArray);
}


app.Run();