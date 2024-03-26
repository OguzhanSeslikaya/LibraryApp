using Stock.EventHandler.Service.Services;
using Stock.Shared.Services.Abstractions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IEventStoreService, Stock.Shared.Services.Concretes.EventStoreService>();
builder.Services.AddSingleton<IMongoDbService, Stock.Shared.Services.Concretes.MongoDbService>();

builder.Services.AddHostedService<Stock.EventHandler.Service.Services.EventStoreService>();

var host = builder.Build();
host.Run();