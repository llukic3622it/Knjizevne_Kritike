using KnjizevneKritikeApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System;

var builder = WebApplication.CreateBuilder(args);

// Dodaj MVC i sesiju
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Registruj MongoDbService sa konekcijom i bazom
builder.Services.AddSingleton<MongoDbService>(sp =>
{
    var client = new MongoClient("mongodb://localhost:27017"); // Promeni connection string po potrebi
    var database = client.GetDatabase("KnjizevneKritike");   // Ime tvoje baze u MongoDB-u
    return new MongoDbService(database);
});

builder.Services.AddSingleton<KorisnikService>(sp =>
{
    var mongoDbService = sp.GetRequiredService<MongoDbService>();
    return new KorisnikService(mongoDbService);
});


var app = builder.Build();

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
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default ruta na Account/Login

app.Run();
