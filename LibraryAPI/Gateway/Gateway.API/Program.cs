using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options
    => options.AddPolicy("policy", policy =>
policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(origin => true)));

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", false, true);
builder.Services.AddOcelot();
var app = builder.Build();

app.UseCors("policy");
await app.UseOcelot();

app.Run();