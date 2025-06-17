var builder = WebApplication.CreateBuilder(args);

// Dodavanje servisa u DI container
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSingleton<MongoDbService>(); // Tvoj servis za MongoDB

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Recenzije}/{action=Index}/{id?}");

app.Run();
