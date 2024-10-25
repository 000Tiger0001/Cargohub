WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseDeveloperExceptionPage();
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.UseSession();
app.UseAuthorization();
app.Urls.Add("http://localhost:6000");

app.Run();