using Identity.API;
using Identity.API.Contexts;
using Identity.API.Entities.Models;
using Identity.API.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "User ID=admin;Password=123;Host=localhost;Port=5432;Database=IdentityDb;";
//SeedData.EnsureSeedData(connectionString);
var assembly = typeof(Program).Assembly.GetName().Name;

//builder.Services.AddCors(options
//    => options.AddPolicy("policy", policy =>
//policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true)));

builder.Services.AddDbContext<IdentityAPIDbContext>(options => options.UseNpgsql(connectionString, b => b.MigrationsAssembly(assembly)));

builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<IdentityAPIDbContext>();

builder.Services
    .AddIdentityServer()
    .AddAspNetIdentity<AppUser>()
    .AddConfigurationStore(options => options.ConfigureDbContext = b => b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(assembly)))
    .AddOperationalStore(options => options.ConfigureDbContext = b => b.UseNpgsql(connectionString, opt => opt.MigrationsAssembly(assembly)))
    .AddDeveloperSigningCredential()
    .AddProfileService<CustomProfileService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

//app.UseCors("policy");

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseIdentityServer();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=login}");

app.Run();