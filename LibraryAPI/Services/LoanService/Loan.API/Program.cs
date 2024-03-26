using Loan.API.Consumers;
using Loan.API.Contexts;
using Loan.API.Services.Abstractions;
using Loan.API.Services.Concretes;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Shared;

var builder = WebApplication.CreateBuilder(args);

var connectionString = "User ID=admin;Password=123;Host=localhost;Port=5432;Database=LoanDB;";
var assembly = typeof(Program).Assembly.GetName().Name;

builder.Services.AddControllers();
builder.Services.AddScoped<ILoanService, LoanService>();

builder.Services.AddMassTransit(configurator => {
    configurator.AddConsumer<LoanStateEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("amqp://guest:guest@localhost:5672");
        _configurator.ReceiveEndpoint(RabbitMQSettings.loan_StockStateEventQueue,
            e => e.ConfigureConsumer<LoanStateEventConsumer>(context));
    });
});

builder.Services.AddDbContext<LoanAPIDbContext>(opt =>
{
    opt.UseNpgsql(connectionString, b => b.MigrationsAssembly(assembly));
});

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", opt =>
    {
        opt.ApiName = "LoanAPI";
        opt.Authority = "https://localhost:7045";
    });

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();