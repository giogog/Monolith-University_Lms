using System.Net.Http.Json;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<ValidationService>();
//builder.Services.AddScoped<NavigationManager>();
// Startup.cs or Program.cs
builder.Services.AddBlazoredSessionStorage();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddHttpClient();




builder.Services.AddBlazoredLocalStorage();


//builder.Services.AddScoped<NavigationManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
