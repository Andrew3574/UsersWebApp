using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using System.Text;
using WebApp_Tak4.Data;
using WebApp_Tak4.Extensions;
using WebApp_Tak4.Models;
using WebApp_Tak4.Services.Auth;
using WebApp_Task4.Extensions;
using WebApp_Task4.Repositories;
using WebApp_Task4.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Task4DbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("default"));
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddSingleton<EncryptionService>();
builder.Services.AddSingleton<JwtAuthenticationService>();
var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.ApplyMigrations();
}*/

app.ErrorHandlerMiddleware();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
