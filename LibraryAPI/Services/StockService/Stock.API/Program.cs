using IdentityServer4.AccessTokenValidation;
using MassTransit;
using RabbitMQ.Shared;
using Stock.API.Consumers;
using Stock.Shared;
using Stock.Shared.Services.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication("Bearer")
    .AddIdentityServerAuthentication("Bearer", opt =>
    {
        opt.ApiName = "StockAPI";
        opt.Authority = "https://localhost:7045";
        opt.SupportedTokens = SupportedTokens.Jwt;
    });

builder.Services.AddMassTransit(configurator => {
    configurator.AddConsumer<LoanCreatedEventConsumer>();
    configurator.AddConsumer<ReturnLoanEventConsumer>();
    configurator.UsingRabbitMq((context, _configurator) =>
    {
        _configurator.Host("amqp://guest:guest@localhost:5672");
        _configurator.ReceiveEndpoint(RabbitMQSettings.stock_LoanCreatedEventQueue,
            e => e.ConfigureConsumer<LoanCreatedEventConsumer>(context));
        _configurator.ReceiveEndpoint(RabbitMQSettings.stock_LoanReturnEventQueue,
            e => e.ConfigureConsumer<ReturnLoanEventConsumer>(context));
    });
});

builder.Services.addStockSharedServices();

builder.Services.AddSingleton<IEventStoreService, Stock.Shared.Services.Concretes.EventStoreService>();
builder.Services.AddSingleton<IMongoDbService, Stock.Shared.Services.Concretes.MongoDbService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();