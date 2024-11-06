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
app.UseSession();
app.UseAuthorization();
app.Urls.Add("http://localhost:6000");

app.Run();