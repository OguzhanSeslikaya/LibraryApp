using Stock.Outbox.Publisher.Service.Jobs;
using MassTransit;
using Quartz;
using Stock.Shared.Services.Abstractions;
using Stock.Shared.Services.Concretes;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IMongoDbService,MongoDbService>();

builder.Services.AddMassTransit(configurator =>
{
    configurator.UsingRabbitMq((context, _configure) =>
    {
        _configure.Host("amqp://guest:guest@localhost:5672");
    });
});

builder.Services.AddQuartz(configurator =>
{
    JobKey jobKey = new("StockOutboxPublishJob");
    configurator.AddJob<StockOutboxPublishJob>(options => options.WithIdentity(jobKey));

    TriggerKey triggerKey = new("StockOutboxPublishTrigger");
    configurator.AddTrigger(options => options.ForJob(jobKey)
    .WithIdentity(triggerKey)
    .StartAt(DateTime.UtcNow)
    .WithSimpleSchedule(builder => builder
        .WithIntervalInSeconds(5)
        .RepeatForever()));
});

builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

var host = builder.Build();
host.Run();